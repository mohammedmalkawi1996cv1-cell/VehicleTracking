using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleTrackingSystem.API.Data;
using VehicleTrackingSystem.API.DTOs;
using VehicleTrackingSystem.API.Models;

namespace VehicleTrackingSystem.API.Controllers
{
    [ApiController]
    [Route("api/devices/{deviceId:int}/locations")]
    public class DeviceLocationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DeviceLocationsController(
            ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/devices/1/locations
        // GET: api/devices/1/locations?pageNumber=1&pageSize=20
        [HttpGet]
        public async Task<IActionResult> GetDeviceLocations(
            int deviceId,
            int pageNumber = 1,
            int pageSize = 20)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                return BadRequest(
                    "Page number and page size must be greater than 0.");
            }

            if (pageSize > 100)
            {
                return BadRequest(
                    "Page size cannot exceed 100.");
            }

            var deviceExists = await _context.Devices
                .AnyAsync(d => d.Id == deviceId);

            if (!deviceExists)
            {
                return NotFound("GPS device was not found.");
            }

            var query = _context.DeviceLocations
                .Where(l => l.DeviceId == deviceId);

            var totalCount = await query.CountAsync();

            var locations = await query
                .OrderByDescending(l => l.RecordedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(l => new DeviceLocationDto
                {
                    Id = l.Id,
                    DeviceId = l.DeviceId,
                    Latitude = l.Latitude,
                    Longitude = l.Longitude,
                    RecordedAt = l.RecordedAt
                })
                .ToListAsync();

            var totalPages = (int)Math.Ceiling(
                (double)totalCount / pageSize);

            return Ok(new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Locations = locations
            });
        }
        // GET: api/devices/1/locations/latest
        [HttpGet("latest")]
        public async Task<ActionResult<DeviceLocationDto>>
            GetLatestLocation(int deviceId)
        {
            var deviceExists = await _context.Devices
                .AnyAsync(d => d.Id == deviceId);

            if (!deviceExists)
            {
                return NotFound("GPS device was not found.");
            }

            var location = await _context.DeviceLocations
                .Where(l => l.DeviceId == deviceId)
                .OrderByDescending(l => l.RecordedAt)
                .Select(l => new DeviceLocationDto
                {
                    Id = l.Id,
                    DeviceId = l.DeviceId,
                    Latitude = l.Latitude,
                    Longitude = l.Longitude,
                    RecordedAt = l.RecordedAt
                })
                .FirstOrDefaultAsync();

            if (location == null)
            {
                return NotFound(
                    "No locations have been recorded for this device.");
            }

            return Ok(location);
        }

        // POST: api/devices/1/locations
        [HttpPost]
        public async Task<ActionResult<DeviceLocationDto>>
            CreateDeviceLocation(
                int deviceId,
                CreateDeviceLocationDto locationDto)
        {
            var device = await _context.Devices
                .FirstOrDefaultAsync(d => d.Id == deviceId);

            if (device == null)
            {
                return NotFound("GPS device was not found.");
            }

            var vehicleIsDeleted = await _context.Vehicles
                .AnyAsync(v =>
                    v.Id == device.VehicleId &&
                    v.IsDeleted);

            if (vehicleIsDeleted)
            {
                return BadRequest(
                    "Cannot record a location for a deleted vehicle.");
            }

            var location = new DeviceLocation
            {
                DeviceId = device.Id,
                Latitude = locationDto.Latitude,
                Longitude = locationDto.Longitude,
                RecordedAt = DateTime.UtcNow
            };

            _context.DeviceLocations.Add(location);

            await _context.SaveChangesAsync();

            var locationDtoResponse = new DeviceLocationDto
            {
                Id = location.Id,
                DeviceId = location.DeviceId,
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                RecordedAt = location.RecordedAt
            };

            return CreatedAtAction(
                nameof(GetLatestLocation),
                new { deviceId = deviceId },
                locationDtoResponse);
        }
    }
}