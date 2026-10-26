
using System.Threading.Tasks;
using VehicleTrackingSystem.API.Common;
using VehicleTrackingSystem.API.DTOs;

namespace VehicleTrackingSystem.API.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<VehicleDto?> CreateAsync(CreateVehicleDto dto);

        Task<VehicleDto?> GetByIdAsync(int id);

        Task<VehicleDto?> UpdateAsync(
            int id,
            UpdateVehicleDto dto);

        Task<bool> DeleteAsync(int id);

        Task<PagedResult<VehicleDto>> GetAllAsync(
            string? search,
            int? dealerId,
            string? sortBy,
            bool descending,
            int pageNumber,
            int pageSize);
    }
}
