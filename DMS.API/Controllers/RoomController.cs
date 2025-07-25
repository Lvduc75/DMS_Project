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
        public IActionResult GetAll(int? dormitoryId, string status = null)
        {
            var query = _context.Rooms.AsQueryable();
            if (dormitoryId.HasValue)
                query = query.Where(r => r.DormitoryId == dormitoryId.Value);
            if (!string.IsNullOrEmpty(status))
                query = query.Where(r => r.Status == status);
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

        // PUT: api/Room/{id}/status
        [HttpPut("{id}/status")]
        public IActionResult UpdateStatus(int id, [FromBody] string status)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.Id == id);
            if (room == null) return NotFound();
            room.Status = status;
            _context.SaveChanges();
            return Ok(room);
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

        public class StudentAssignRequest { public int StudentId { get; set; } }
        [HttpPost("{roomId}/assign-student")]
        public IActionResult AssignStudent(int roomId, [FromBody] StudentAssignRequest req)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.Id == roomId);
            if (room == null) return NotFound();
            var currentCount = _context.StudentRooms.Count(sr => sr.RoomId == roomId && sr.EndDate == null);
            if (currentCount >= room.Capacity)
                return BadRequest("Phòng đã đủ sức chứa");
            var sr = new StudentRoom
            {
                StudentId = req.StudentId,
                RoomId = roomId,
                StartDate = DateOnly.FromDateTime(DateTime.Now)
            };
            _context.StudentRooms.Add(sr);
            _context.SaveChanges();
            // Trả về DTO đơn giản, tránh vòng lặp
            return Ok(new { sr.Id, sr.StudentId, sr.RoomId, sr.StartDate, sr.EndDate });
        }
    }
} 