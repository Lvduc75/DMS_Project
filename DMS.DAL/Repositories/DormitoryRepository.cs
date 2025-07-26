using DMS.DAL.Interfaces;
using DMS.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DMS.DAL.Repositories
{
    public class DormitoryRepository : IDormitoryRepository
    {
        private readonly DormManagementContext _context;

        public DormitoryRepository(DormManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Dormitory>> GetAllAsync()
        {
            return await _context.Dormitories.ToListAsync();
        }

        public async Task<IEnumerable<object>> GetAllWithDetailsAsync()
        {
            return await _context.Dormitories
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
        }

        public async Task<Dormitory?> GetByIdAsync(int id)
        {
            return await _context.Dormitories.FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<object?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Dormitories
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
        }

        public async Task<Dormitory> CreateAsync(Dormitory dormitory)
        {
            _context.Dormitories.Add(dormitory);
            await _context.SaveChangesAsync();
            return dormitory;
        }

        public async Task<Dormitory> UpdateAsync(Dormitory dormitory)
        {
            _context.Dormitories.Update(dormitory);
            await _context.SaveChangesAsync();
            return dormitory;
        }

        public async Task DeleteAsync(int id)
        {
            var dorm = await _context.Dormitories.FindAsync(id);
            if (dorm != null)
            {
                _context.Dormitories.Remove(dorm);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> HasRoomsAsync(int id)
        {
            return await _context.Rooms.AnyAsync(r => r.DormitoryId == id);
        }

        public async Task<bool> HasFacilitiesAsync(int id)
        {
            return await _context.DormFacilities.AnyAsync(f => f.DormitoryId == id);
        }
    }
} 