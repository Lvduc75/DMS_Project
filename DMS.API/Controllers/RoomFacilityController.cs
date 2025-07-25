using Microsoft.AspNetCore.Mvc;
using DMS.Models.Entities;
using System.Linq;

namespace DMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomFacilityController : ControllerBase
    {
        private readonly DormManagementContext _context;
        public RoomFacilityController(DormManagementContext context)
        {
            _context = context;
        }

        // GET: api/RoomFacility
        [HttpGet]
        public IActionResult GetAll()
        {
            var list = _context.RoomFacilities
                .Select(rf => new {
                    rf.Id,
                    rf.RoomId,
                    RoomName = rf.Room.Code,
                    rf.FacilityId,
                    FacilityName = rf.Facility.Name,
                    rf.Quantity
                })
                .ToList();
            return Ok(list);
        }

        // POST: api/RoomFacility
        [HttpPost]
        public IActionResult Create([FromBody] RoomFacility rf)
        {
            if (rf.Quantity < 0)
                return BadRequest("Số lượng phải >= 0");
            _context.RoomFacilities.Add(rf);
            _context.SaveChanges();
            return Ok(rf);
        }

        // PUT: api/RoomFacility/{id}
        [HttpPut("{id}")]
        public IActionResult Edit(int id, [FromBody] RoomFacility rf)
        {
            var existing = _context.RoomFacilities.FirstOrDefault(x => x.Id == id);
            if (existing == null) return NotFound();
            if (rf.Quantity < 0)
                return BadRequest("Số lượng phải >= 0");
            existing.RoomId = rf.RoomId;
            existing.FacilityId = rf.FacilityId;
            existing.Quantity = rf.Quantity;
            _context.SaveChanges();
            return Ok(existing);
        }

        // DELETE: api/RoomFacility/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var rf = _context.RoomFacilities.FirstOrDefault(x => x.Id == id);
            if (rf == null) return NotFound();
            _context.RoomFacilities.Remove(rf);
            _context.SaveChanges();
            return Ok();
        }
    }
} 