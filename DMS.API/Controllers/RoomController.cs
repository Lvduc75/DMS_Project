using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DMS.Models.Entities;

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

        // GET: api/room
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetRooms([FromQuery] int? dormitoryId = null)
        {
            var rooms = await _context.Rooms
                .Include(r => r.Dormitory)
                .Include(r => r.StudentRooms)
                .ThenInclude(sr => sr.Student)
                .Include(r => r.RoomFacilities)
                .ThenInclude(rf => rf.Facility)
                .Where(r => !dormitoryId.HasValue || r.DormitoryId == dormitoryId.Value)
                .Select(r => new
                {
                    r.Id,
                    r.Code,
                    r.Capacity,
                    r.Status,
                    r.DormitoryId,
                    // Fields bổ sung cho UI
                    RoomNumber = r.Code, // Alias cho Code
                    Occupied = r.StudentRooms.Count(sr => sr.EndDate > DateOnly.FromDateTime(DateTime.Now)),
                    RoomType = GetRoomType(r.Capacity), // Helper function
                    Floor = GetFloorFromCode(r.Code), // Helper function
                    Price = GetRoomPrice(r.Capacity), // Helper function
                    DormitoryName = r.Dormitory.Name,
                    Facilities = r.RoomFacilities.Select(rf => rf.Facility.Name).ToList(),
                    Students = r.StudentRooms
                        .Where(sr => sr.EndDate > DateOnly.FromDateTime(DateTime.Now))
                        .Select(sr => new
                        {
                            sr.Student.Id,
                            sr.Student.Name,
                            StudentId = sr.Student.Email, // Sử dụng email làm StudentId
                            sr.Student.Email
                        }).ToList()
                })
                .ToListAsync();

            return Ok(rooms);
        }

        // GET: api/room/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetRoom(int id)
        {
            var room = await _context.Rooms
                .Include(r => r.Dormitory)
                .Include(r => r.StudentRooms)
                .ThenInclude(sr => sr.Student)
                .Include(r => r.RoomFacilities)
                .ThenInclude(rf => rf.Facility)
                .Where(r => r.Id == id)
                .Select(r => new
                {
                    r.Id,
                    r.Code,
                    r.Capacity,
                    r.Status,
                    r.DormitoryId,
                    RoomNumber = r.Code,
                    Occupied = r.StudentRooms.Count(sr => sr.EndDate > DateOnly.FromDateTime(DateTime.Now)),
                    RoomType = GetRoomType(r.Capacity),
                    Floor = GetFloorFromCode(r.Code),
                    Price = GetRoomPrice(r.Capacity),
                    DormitoryName = r.Dormitory.Name,
                    Facilities = r.RoomFacilities.Select(rf => rf.Facility.Name).ToList(),
                    Students = r.StudentRooms
                        .Where(sr => sr.EndDate > DateOnly.FromDateTime(DateTime.Now))
                        .Select(sr => new
                        {
                            sr.Student.Id,
                            sr.Student.Name,
                            StudentId = sr.Student.Email,
                            sr.Student.Email
                        }).ToList()
                })
                .FirstOrDefaultAsync();

            if (room == null)
            {
                return NotFound();
            }

            return Ok(room);
        }

        // GET: api/room/student/{studentId}
        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<object>> GetRoomByStudent(int studentId)
        {
            var room = await _context.Rooms
                .Include(r => r.Dormitory)
                .Include(r => r.StudentRooms)
                .ThenInclude(sr => sr.Student)
                .Include(r => r.RoomFacilities)
                .ThenInclude(rf => rf.Facility)
                .Where(r => r.StudentRooms.Any(sr => sr.StudentId == studentId && sr.EndDate > DateOnly.FromDateTime(DateTime.Now)))
                .Select(r => new
                {
                    r.Id,
                    r.Code,
                    r.Capacity,
                    r.Status,
                    r.DormitoryId,
                    RoomNumber = r.Code,
                    Occupied = r.StudentRooms.Count(sr => sr.EndDate > DateOnly.FromDateTime(DateTime.Now)),
                    RoomType = GetRoomType(r.Capacity),
                    Floor = GetFloorFromCode(r.Code),
                    Price = GetRoomPrice(r.Capacity),
                    DormitoryName = r.Dormitory.Name,
                    Facilities = r.RoomFacilities.Select(rf => rf.Facility.Name).ToList(),
                    Students = r.StudentRooms
                        .Where(sr => sr.EndDate > DateOnly.FromDateTime(DateTime.Now))
                        .Select(sr => new
                        {
                            sr.Student.Id,
                            sr.Student.Name,
                            StudentId = sr.Student.Email,
                            sr.Student.Email
                        }).ToList()
                })
                .FirstOrDefaultAsync();

            if (room == null)
            {
                return NotFound();
            }

            return Ok(room);
        }

        // Helper methods
        private static string GetRoomType(int capacity)
        {
            return capacity switch
            {
                1 => "Single",
                2 => "Double",
                3 => "Triple",
                4 => "Quad",
                _ => "Multiple"
            };
        }

        private static int GetFloorFromCode(string code)
        {
            if (code.Length >= 3 && int.TryParse(code.Substring(1, 1), out int floor))
            {
                return floor;
            }
            return 1; // Default floor
        }

        private static decimal GetRoomPrice(int capacity)
        {
            return capacity switch
            {
                1 => 2000000m, // 2 triệu
                2 => 1500000m, // 1.5 triệu
                3 => 1200000m, // 1.2 triệu
                4 => 1000000m, // 1 triệu
                _ => 800000m   // 800k
            };
        }
    }
} 