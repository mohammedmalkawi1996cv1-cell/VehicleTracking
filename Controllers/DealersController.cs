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
    public class DealersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DealersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Dealers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DealerDto>>> GetDealers()
        {
            var dealers = await _context.Dealers
                .Select(d => new DealerDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Address = d.Address
                })
                .ToListAsync();

            return Ok(dealers);
        }

        // GET: api/Dealers/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<DealerDto>> GetDealerById(int id)
        {
            var dealer = await _context.Dealers
                .Where(d => d.Id == id)
                .Select(d => new DealerDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Address = d.Address
                })
                .FirstOrDefaultAsync();

            if (dealer == null)
            {
                return NotFound();
            }

            return Ok(dealer);
        }

        // POST: api/Dealers
        [HttpPost]
        public async Task<ActionResult<DealerDto>> CreateDealer(
            CreateDealerDto dealerDto)
        {
            var dealer = new Dealer
            {
                Name = dealerDto.Name,
                Address = dealerDto.Address
            };

            _context.Dealers.Add(dealer);
            await _context.SaveChangesAsync();

            var responseDto = new DealerDto
            {
                Id = dealer.Id,
                Name = dealer.Name,
                Address = dealer.Address
            };

            return CreatedAtAction(
                nameof(GetDealerById),
                new { id = dealer.Id },
                responseDto);
        }

        // PUT: api/Dealers/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateDealer(
            int id,
            UpdateDealerDto updatedDealer)
        {
            var dealer = await _context.Dealers
                .FirstOrDefaultAsync(d => d.Id == id);

            if (dealer == null)
            {
                return NotFound();
            }

            dealer.Name = updatedDealer.Name;
            dealer.Address = updatedDealer.Address;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Dealers/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteDealer(int id)
        {
            var dealer = await _context.Dealers
                .FirstOrDefaultAsync(d => d.Id == id);

            if (dealer == null)
            {
                return NotFound();
            }

            _context.Dealers.Remove(dealer);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
