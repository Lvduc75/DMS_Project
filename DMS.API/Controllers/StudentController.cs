using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DMS.Models.DTOs;
using DMS.Models.Entities;

namespace DMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private readonly DormManagementContext _context;

    public StudentController(DormManagementContext context)
    {
        _context = context;
    }

    [HttpGet("dormitories")]
    public async Task<ActionResult<List<StudentDormitoryDTO>>> GetDormitories()
    {
        try
        {
            var dormitories = await _context.Dormitories
                .Include(d => d.Rooms)
                .Include(d => d.DormFacilities)
                .ThenInclude(df => df.Facility)
                .Select(d => new StudentDormitoryDTO
                {
                    Id = d.Id,
                    Name = d.Name,
                    TotalRooms = d.Rooms.Count,
                    AvailableRooms = d.Rooms.Count(r => r.Status == "Available"),
                    OccupiedRooms = d.Rooms.Count(r => r.Status == "Occupied"),
                    Facilities = d.DormFacilities.Select(df => df.Facility.Name).ToList()
                })
                .ToListAsync();

            return Ok(dormitories);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal server error", error = ex.Message });
        }
    }

    [HttpGet("rooms")]
    public async Task<ActionResult<List<StudentRoomDTO>>> GetRooms([FromQuery] int? dormitoryId = null)
    {
        try
        {
            var query = _context.Rooms
                .Include(r => r.Dormitory)
                .Include(r => r.RoomFacilities)
                .ThenInclude(rf => rf.Facility)
                .Include(r => r.StudentRooms)
                .AsQueryable();

            if (dormitoryId.HasValue)
            {
                query = query.Where(r => r.DormitoryId == dormitoryId.Value);
            }

            var rooms = await query
                .Select(r => new StudentRoomDTO
                {
                    Id = r.Id,
                    RoomNumber = r.Code,
                    Floor = GetFloorFromRoomCode(r.Code),
                    Status = r.Status,
                    RoomType = GetRoomType(r.Capacity),
                    Price = GetRoomPrice(r.Capacity),
                    Capacity = r.Capacity,
                    CurrentOccupants = r.StudentRooms.Count,
                    DormitoryName = r.Dormitory.Name,
                    Facilities = r.RoomFacilities.Select(rf => rf.Facility.Name).ToList()
                })
                .ToListAsync();

            return Ok(rooms);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal server error", error = ex.Message });
        }
    }

    [HttpGet("profile/{userId}")]
    public async Task<ActionResult<StudentProfileDTO>> GetProfile(int userId)
    {
        try
        {
            var user = await _context.Users
                .Include(u => u.StudentRooms)
                .ThenInclude(sr => sr.Room)
                .ThenInclude(r => r.Dormitory)
                .FirstOrDefaultAsync(u => u.Id == userId && u.Role == "Student");

            if (user == null)
            {
                return NotFound(new { message = "Student not found" });
            }

            var currentRoom = user.StudentRooms
                .Where(sr => sr.Room.Status == "Occupied")
                .FirstOrDefault();

            var profile = new StudentProfileDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                Role = user.Role,
                CurrentRoom = currentRoom?.Room?.Code,
                DormitoryName = currentRoom?.Room?.Dormitory?.Name
            };

            return Ok(profile);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal server error", error = ex.Message });
        }
    }

    private int GetFloorFromRoomCode(string roomCode)
    {
        // Giả sử format: A101, B205, etc. - lấy số thứ 2
        if (roomCode.Length >= 2 && char.IsDigit(roomCode[1]))
        {
            return int.Parse(roomCode[1].ToString());
        }
        return 1; // Default
    }

    private string GetRoomType(int capacity)
    {
        return capacity switch
        {
            1 => "Single",
            2 => "Double",
            3 => "Triple",
            4 => "Quad",
            _ => "Other"
        };
    }

    private decimal GetRoomPrice(int capacity)
    {
        return capacity switch
        {
            1 => 1500000, // 1.5M
            2 => 1200000, // 1.2M
            3 => 1000000, // 1M
            4 => 800000,  // 800K
            _ => 1000000
        };
    }
} 