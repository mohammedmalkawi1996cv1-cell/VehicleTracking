using Microsoft.AspNetCore.Mvc;
using VehicleTrackingSystem.API.DTOs;
using VehicleTrackingSystem.API.Services.Interfaces;

namespace VehicleTrackingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehiclesController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        // GET: api/vehicles
        [HttpGet]
        public async Task<IActionResult> GetVehicles(
            string? search,
            int? dealerId,
            string? sortBy = "name",
            bool descending = false,
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

            var result = await _vehicleService.GetAllAsync(
                search,
                dealerId,
                sortBy,
                descending,
                pageNumber,
                pageSize);

            return Ok(result);
        }

        // GET: api/vehicles/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicleById(int id)
        {
            var vehicle = await _vehicleService.GetByIdAsync(id);

            if (vehicle == null)
            {
                return NotFound("Vehicle was not found.");
            }

            return Ok(vehicle);
        }

        // POST: api/vehicles
        [HttpPost]
        public async Task<IActionResult> CreateVehicle(
            CreateVehicleDto dto)
        {
            var vehicle = await _vehicleService.CreateAsync(dto);

            if (vehicle == null)
            {
                return NotFound("Dealer was not found.");
            }

            return CreatedAtAction(
                nameof(GetVehicleById),
                new { id = vehicle.Id },
                vehicle);
        }

        // PUT: api/vehicles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicle(
            int id,
            UpdateVehicleDto dto)
        {
            var vehicle = await _vehicleService.UpdateAsync(id, dto);

            if (vehicle == null)
            {
                return NotFound(
                    "Vehicle or dealer was not found.");
            }

            return Ok(vehicle);
        }

        // DELETE: api/vehicles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var deleted = await _vehicleService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound("Vehicle was not found.");
            }

            return NoContent();
        }
    }
}
