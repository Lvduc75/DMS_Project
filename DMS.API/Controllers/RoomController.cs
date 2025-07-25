using Microsoft.AspNetCore.Mvc;
using DMS.Models.Entities;
using System.Linq;

namespace DMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly DormManagementContext _context;
        public RoomController(DormManagementContext context)
        {
            _context = context;
        }

        // GET: api/Room?dormitoryId=1
        [HttpGet]
        public IActionResult GetAll(int? dormitoryId)
        {
            var query = _context.Rooms.AsQueryable();
            if (dormitoryId.HasValue)
                query = query.Where(r => r.DormitoryId == dormitoryId.Value);
            var rooms = query
                .Select(r => new {
                    r.Id,
                    r.Code,
                    r.Capacity,
                    r.Status,
                    r.DormitoryId,
                    DormitoryName = r.Dormitory.Name
                })
                .ToList();
            return Ok(rooms);
        }

        // POST: api/Room
        [HttpPost]
        public IActionResult Create([FromBody] Room room)
        {
            if (string.IsNullOrWhiteSpace(room.Code) || string.IsNullOrWhiteSpace(room.Status))
                return BadRequest("Thông tin phòng không hợp lệ");
            _context.Rooms.Add(room);
            _context.SaveChanges();
            return Ok(room);
        }

        // PUT: api/Room/{id}
        [HttpPut("{id}")]
        public IActionResult Edit(int id, [FromBody] Room room)
        {
            var existing = _context.Rooms.FirstOrDefault(r => r.Id == id);
            if (existing == null) return NotFound();
            if (string.IsNullOrWhiteSpace(room.Code) || string.IsNullOrWhiteSpace(room.Status))
                return BadRequest("Thông tin phòng không hợp lệ");
            existing.Code = room.Code;
            existing.Capacity = room.Capacity;
            existing.Status = room.Status;
            existing.DormitoryId = room.DormitoryId;
            _context.SaveChanges();
            return Ok(existing);
        }

        // DELETE: api/Room/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.Id == id);
            if (room == null) return NotFound();
            var hasStudents = _context.StudentRooms.Any(s => s.RoomId == id);
            var hasReadings = _context.UtilityReadings.Any(u => u.RoomId == id);
            if (hasStudents || hasReadings)
                return BadRequest("Phòng này đang được sử dụng");
            _context.Rooms.Remove(room);
            _context.SaveChanges();
            return Ok();
        }
    }
} 