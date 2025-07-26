using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DMS.Models.Entities;

namespace DMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacilityController : ControllerBase
    {
        private readonly DormManagementContext _context;

        public FacilityController(DormManagementContext context)
        {
            _context = context;
        }

        // GET: api/facility
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetFacilities()
        {
            var facilities = await _context.Facilities
                .OrderBy(f => f.Name)
                .Select(f => new
                {
                    f.Id,
                    f.Name,
                    f.UnitPrice
                })
                .ToListAsync();
            return Ok(facilities);
        }

        // GET: api/facility/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetFacility(int id)
        {
            var facility = await _context.Facilities
                .Where(f => f.Id == id)
                .Select(f => new
                {
                    f.Id,
                    f.Name,
                    f.UnitPrice
                })
                .FirstOrDefaultAsync();

            if (facility == null)
            {
                return NotFound();
            }
            return Ok(facility);
        }
    }
} 