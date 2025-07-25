using Microsoft.AspNetCore.Mvc;
using DMS.Models.Entities;
using System.Linq;

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

        // GET: api/Facility
        [HttpGet]
        public IActionResult GetAll()
        {
            var facilities = _context.Facilities
                .Select(f => new {
                    f.Id,
                    f.Name,
                    f.UnitPrice
                })
                .ToList();
            return Ok(facilities);
        }

        // POST: api/Facility
        [HttpPost]
        public IActionResult Create([FromBody] Facility facility)
        {
            if (string.IsNullOrWhiteSpace(facility.Name))
                return BadRequest("Tên thiết bị không được để trống");
            _context.Facilities.Add(facility);
            _context.SaveChanges();
            return Ok(facility);
        }

        // PUT: api/Facility/{id}
        [HttpPut("{id}")]
        public IActionResult Edit(int id, [FromBody] Facility facility)
        {
            var existing = _context.Facilities.FirstOrDefault(f => f.Id == id);
            if (existing == null) return NotFound();
            if (string.IsNullOrWhiteSpace(facility.Name))
                return BadRequest("Tên thiết bị không được để trống");
            existing.Name = facility.Name;
            existing.UnitPrice = facility.UnitPrice;
            _context.SaveChanges();
            return Ok(existing);
        }

        // DELETE: api/Facility/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var facility = _context.Facilities.FirstOrDefault(f => f.Id == id);
            if (facility == null) return NotFound();
            var hasDorm = _context.DormFacilities.Any(df => df.FacilityId == id);
            var hasRoom = _context.RoomFacilities.Any(rf => rf.FacilityId == id);
            if (hasDorm || hasRoom)
                return BadRequest("Thiết bị này đang được sử dụng");
            _context.Facilities.Remove(facility);
            _context.SaveChanges();
            return Ok();
        }
    }
} 