using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VehicleTrackingSystem.API.Common;
using VehicleTrackingSystem.API.Data;
using VehicleTrackingSystem.API.DTOs;
using VehicleTrackingSystem.API.Models;
using VehicleTrackingSystem.API.Services.Interfaces;

namespace VehicleTrackingSystem.API.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<VehicleService> _logger;

        public VehicleService(
            ApplicationDbContext context,
            ILogger<VehicleService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<VehicleDto?> CreateAsync(CreateVehicleDto dto)
        {
            var dealerExists = await _context.Dealers
                .AnyAsync(d => d.Id == dto.DealerId);

            if (!dealerExists)
            {
                _logger.LogWarning(
                    "Vehicle creation failed. DealerId {DealerId} was not found.",
                    dto.DealerId);

                return null;
            }

            var vehicle = new Vehicle
            {
                Name = dto.Name,
                Model = dto.Model,
                DealerId = dto.DealerId,
                IsDeleted = false
            };

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Vehicle created successfully. VehicleId: {VehicleId}",
                vehicle.Id);

            return MapToDto(vehicle);
        }

        public async Task<VehicleDto?> GetByIdAsync(int id)
        {
            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(v => v.Id == id && !v.IsDeleted);

            if (vehicle == null)
                return null;

            return MapToDto(vehicle);
        }

        public async Task<VehicleDto?> UpdateAsync(
            int id,
            UpdateVehicleDto dto)
        {
            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(v => v.Id == id && !v.IsDeleted);

            if (vehicle == null)
                return null;

            var dealerExists = await _context.Dealers
                .AnyAsync(d => d.Id == dto.DealerId);

            if (!dealerExists)
                return null;

            vehicle.Name = dto.Name;
            vehicle.Model = dto.Model;
            vehicle.DealerId = dto.DealerId;

            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Vehicle updated successfully. VehicleId: {VehicleId}",
                vehicle.Id);

            return MapToDto(vehicle);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(v => v.Id == id && !v.IsDeleted);

            if (vehicle == null)
                return false;

            vehicle.IsDeleted = true;
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Vehicle soft-deleted. VehicleId: {VehicleId}",
                vehicle.Id);

            return true;
        }

        public async Task<PagedResult<VehicleDto>> GetAllAsync(
            string? search,
            int? dealerId,
            string? sortBy,
            bool descending,
            int pageNumber,
            int pageSize)
        {
            var query = _context.Vehicles
                .Where(v => !v.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(v =>
                    v.Name.Contains(search) ||
                    (v.Model != null && v.Model.Contains(search)));
            }

            if (dealerId.HasValue)
            {
                query = query.Where(v =>
                    v.DealerId == dealerId.Value);
            }

            query = (sortBy?.ToLower()) switch
            {
                "name" => descending
                    ? query.OrderByDescending(v => v.Name)
                    : query.OrderBy(v => v.Name),

                "model" => descending
                    ? query.OrderByDescending(v => v.Model)
                    : query.OrderBy(v => v.Model),

                "id" => descending
                    ? query.OrderByDescending(v => v.Id)
                    : query.OrderBy(v => v.Id),

                _ => query.OrderBy(v => v.Name)
            };

            var totalCount = await query.CountAsync();

            var vehicles = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(v => new VehicleDto
                {
                    Id = v.Id,
                    Name = v.Name,
                    Model = v.Model,
                    DealerId = v.DealerId,
                    IsDeleted = v.IsDeleted
                })
                .ToListAsync();

            var totalPages = (int)Math.Ceiling(
                (double)totalCount / pageSize);

            return new PagedResult<VehicleDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Items = vehicles
            };
        }

        private static VehicleDto MapToDto(Vehicle vehicle)
        {
            return new VehicleDto
            {
                Id = vehicle.Id,
                Name = vehicle.Name,
                Model = vehicle.Model,
                DealerId = vehicle.DealerId,
                IsDeleted = vehicle.IsDeleted
            };
        }
    }
}
