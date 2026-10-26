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
    [Route("api/[controller]")]
    public class DevicesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DevicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Devices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeviceDto>>> GetDevices()
        {
            var devices = await _context.Devices
                .Select(d => new DeviceDto
                {
                    Id = d.Id,
                    DeviceId = d.DeviceId,
                    VehicleId = d.VehicleId
                })
                .ToListAsync();

            return Ok(devices);
        }

        // GET: api/Devices/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<DeviceDto>> GetDeviceById(int id)
        {
            var device = await _context.Devices
                .Where(d => d.Id == id)
                .Select(d => new DeviceDto
                {
                    Id = d.Id,
                    DeviceId = d.DeviceId,
                    VehicleId = d.VehicleId
                })
                .FirstOrDefaultAsync();

            if (device == null)
            {
                return NotFound();
            }

            return Ok(device);
        }

        // POST: api/Devices
        [HttpPost]
        public async Task<ActionResult<DeviceDto>> CreateDevice(
            CreateDeviceDto deviceDto)
        {
            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(v =>
                    v.Id == deviceDto.VehicleId &&
                    !v.IsDeleted);

            if (vehicle == null)
            {
                return BadRequest(
                    "The specified vehicle does not exist or is deleted.");
            }

            var vehicleAlreadyHasDevice = await _context.Devices
                .AnyAsync(d => d.VehicleId == deviceDto.VehicleId);

            if (vehicleAlreadyHasDevice)
            {
                return BadRequest(
                    "This vehicle already has a GPS device.");
            }

            var deviceIdExists = await _context.Devices
                .AnyAsync(d => d.DeviceId == deviceDto.DeviceId);

            if (deviceIdExists)
            {
                return Conflict(
                    "A device with this DeviceId already exists.");
            }

            var device = new Device
            {
                DeviceId = deviceDto.DeviceId,
                VehicleId = deviceDto.VehicleId
            };

            _context.Devices.Add(device);
            await _context.SaveChangesAsync();

            var responseDto = new DeviceDto
            {
                Id = device.Id,
                DeviceId = device.DeviceId,
                VehicleId = device.VehicleId
            };

            return CreatedAtAction(
                nameof(GetDeviceById),
                new { id = device.Id },
                responseDto);
        }
    }
}
