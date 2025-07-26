using DMS.DAL.Interfaces;
using DMS.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DMS.DAL.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly DormManagementContext _context;

        public RoomRepository(DormManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Room>> GetAllAsync()
        {
            return await _context.Rooms.ToListAsync();
        }

        public async Task<IEnumerable<object>> GetAllWithDetailsAsync(int? dormitoryId)
        {
            var query = _context.Rooms
                .Include(r => r.Dormitory)
                .Include(r => r.StudentRooms)
                .ThenInclude(sr => sr.Student)
                .Include(r => r.RoomFacilities)
                .ThenInclude(rf => rf.Facility)
                .AsQueryable();

            if (dormitoryId.HasValue)
            {
                query = query.Where(r => r.DormitoryId == dormitoryId.Value);
            }

            return await query
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
        }

        public async Task<Room?> GetByIdAsync(int id)
        {
            return await _context.Rooms.FindAsync(id);
        }

        public async Task<object?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Rooms
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
        }

        public async Task<object?> GetByStudentWithDetailsAsync(int studentId)
        {
            return await _context.Rooms
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
        }

        public async Task<Room> CreateAsync(Room room)
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            return room;
        }

        public async Task<Room> UpdateAsync(Room room)
        {
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
            return room;
        }

        public async Task DeleteAsync(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> HasStudentsAsync(int id)
        {
            return await _context.StudentRooms.AnyAsync(sr => sr.RoomId == id);
        }

        public async Task<bool> HasFacilitiesAsync(int id)
        {
            return await _context.RoomFacilities.AnyAsync(rf => rf.RoomId == id);
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