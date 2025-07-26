using Microsoft.AspNetCore.Mvc;
using DMS.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DormitoryController : ControllerBase
    {
        private readonly DormManagementContext _context;
        public DormitoryController(DormManagementContext context)
        {
            _context = context;
        }

        // GET: api/Dormitory
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dormitories = await _context.Dormitories
                .Include(d => d.Rooms)
                .Include(d => d.DormFacilities)
                .ThenInclude(df => df.Facility)
                .Select(d => new
                {
                    d.Id,
                    d.Name,
                    // Thông tin bổ sung cho UI
                    Address = $"Địa chỉ {d.Name}", // Placeholder - có thể thêm field Address vào Entity sau
                    PhoneNumber = "0123456789", // Placeholder - có thể thêm field PhoneNumber vào Entity sau
                    Status = "Hoạt động", // Placeholder - có thể thêm field Status vào Entity sau
                    TotalRooms = d.Rooms.Count,
                    AvailableRooms = d.Rooms.Count(r => r.Status == "Available"),
                    OccupiedRooms = d.Rooms.Count(r => r.Status == "Occupied"),
                    Facilities = d.DormFacilities.Select(df => df.Facility.Name).ToList()
                })
                .ToListAsync();

            return Ok(dormitories);
        }

        // GET: api/Dormitory/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dormitory = await _context.Dormitories
                .Include(d => d.Rooms)
                .Include(d => d.DormFacilities)
                .ThenInclude(df => df.Facility)
                .Where(d => d.Id == id)
                .Select(d => new
                {
                    d.Id,
                    d.Name,
                    Address = $"Địa chỉ {d.Name}",
                    PhoneNumber = "0123456789",
                    Status = "Hoạt động",
                    TotalRooms = d.Rooms.Count,
                    AvailableRooms = d.Rooms.Count(r => r.Status == "Available"),
                    OccupiedRooms = d.Rooms.Count(r => r.Status == "Occupied"),
                    Facilities = d.DormFacilities.Select(df => df.Facility.Name).ToList()
                })
                .FirstOrDefaultAsync();

            if (dormitory == null)
                return NotFound();

            return Ok(dormitory);
        }

        // POST: api/Dormitory
        [HttpPost]
        public IActionResult Create([FromBody] Dormitory dorm)
        {
            if (string.IsNullOrWhiteSpace(dorm.Name))
                return BadRequest("Tên ký túc xá không được để trống");
            _context.Dormitories.Add(dorm);
            _context.SaveChanges();
            return Ok(dorm);
        }

        // PUT: api/Dormitory/{id}
        [HttpPut("{id}")]
        public IActionResult Edit(int id, [FromBody] Dormitory dorm)
        {
            var existing = _context.Dormitories.FirstOrDefault(d => d.Id == id);
            if (existing == null) return NotFound();
            if (string.IsNullOrWhiteSpace(dorm.Name))
                return BadRequest("Tên ký túc xá không được để trống");
            existing.Name = dorm.Name;
            _context.SaveChanges();
            return Ok(existing);
        }

        // DELETE: api/Dormitory/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var dorm = _context.Dormitories.FirstOrDefault(d => d.Id == id);
            if (dorm == null) return NotFound();
            var hasRooms = _context.Rooms.Any(r => r.DormitoryId == id);
            var hasFacilities = _context.DormFacilities.Any(f => f.DormitoryId == id);
            if (hasRooms || hasFacilities)
                return BadRequest("Dom này đang được sử dụng");
            _context.Dormitories.Remove(dorm);
            _context.SaveChanges();
            return Ok();
        }
    }
} 