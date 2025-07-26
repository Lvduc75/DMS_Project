using DMS.DAL.Interfaces;
using DMS.Models.Entities;
using DMS.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DMS.DAL.Repositories
{
    public class RoomFacilityRepository : IRoomFacilityRepository
    {
        private readonly DormManagementContext _context;

        public RoomFacilityRepository(DormManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RoomFacility>> GetAllAsync()
        {
            return await _context.RoomFacilities.ToListAsync();
        }

        public async Task<IEnumerable<object>> GetAllWithDetailsAsync()
        {
            return await _context.RoomFacilities
                .Include(rf => rf.Room)
                .ThenInclude(r => r.Dormitory)
                .Include(rf => rf.Facility)
                .OrderBy(rf => rf.Room.Code)
                .ThenBy(rf => rf.Facility.Name)
                .Select(rf => new
                {
                    rf.Id,
                    rf.RoomId,
                    rf.FacilityId,
                    rf.Quantity,
                    Room = rf.Room != null ? new
                    {
                        rf.Room.Id,
                        rf.Room.Code,
                        rf.Room.Capacity,
                        rf.Room.Status,
                        Dormitory = rf.Room.Dormitory != null ? new
                        {
                            rf.Room.Dormitory.Id,
                            rf.Room.Dormitory.Name
                        } : null
                    } : null,
                    Facility = rf.Facility != null ? new
                    {
                        rf.Facility.Id,
                        rf.Facility.Name,
                        rf.Facility.UnitPrice
                    } : null
                })
                .ToListAsync();
        }

        public async Task<RoomFacility?> GetByIdAsync(int id)
        {
            return await _context.RoomFacilities.FindAsync(id);
        }

        public async Task<object?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.RoomFacilities
                .Include(rf => rf.Room)
                .ThenInclude(r => r.Dormitory)
                .Include(rf => rf.Facility)
                .Where(rf => rf.Id == id)
                .Select(rf => new
                {
                    rf.Id,
                    rf.RoomId,
                    rf.FacilityId,
                    rf.Quantity,
                    Room = rf.Room != null ? new
                    {
                        rf.Room.Id,
                        rf.Room.Code,
                        rf.Room.Capacity,
                        rf.Room.Status,
                        Dormitory = rf.Room.Dormitory != null ? new
                        {
                            rf.Room.Dormitory.Id,
                            rf.Room.Dormitory.Name
                        } : null
                    } : null,
                    Facility = rf.Facility != null ? new
                    {
                        rf.Facility.Id,
                        rf.Facility.Name,
                        rf.Facility.UnitPrice
                    } : null
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<object>> GetByRoomWithDetailsAsync(int roomId)
        {
            return await _context.RoomFacilities
                .Include(rf => rf.Room)
                .ThenInclude(r => r.Dormitory)
                .Include(rf => rf.Facility)
                .Where(rf => rf.RoomId == roomId)
                .OrderBy(rf => rf.Facility.Name)
                .Select(rf => new
                {
                    rf.Id,
                    rf.RoomId,
                    rf.FacilityId,
                    rf.Quantity,
                    Room = rf.Room != null ? new
                    {
                        rf.Room.Id,
                        rf.Room.Code,
                        rf.Room.Capacity,
                        rf.Room.Status,
                        Dormitory = rf.Room.Dormitory != null ? new
                        {
                            rf.Room.Dormitory.Id,
                            rf.Room.Dormitory.Name
                        } : null
                    } : null,
                    Facility = rf.Facility != null ? new
                    {
                        rf.Facility.Id,
                        rf.Facility.Name,
                        rf.Facility.UnitPrice
                    } : null
                })
                .ToListAsync();
        }

        public async Task<RoomFacility> CreateAsync(RoomFacility roomFacility)
        {
            _context.RoomFacilities.Add(roomFacility);
            await _context.SaveChangesAsync();
            return roomFacility;
        }

        public async Task<RoomFacility> UpdateAsync(RoomFacility roomFacility)
        {
            _context.RoomFacilities.Update(roomFacility);
            await _context.SaveChangesAsync();
            return roomFacility;
        }

        public async Task DeleteAsync(int id)
        {
            var roomFacility = await _context.RoomFacilities.FindAsync(id);
            if (roomFacility != null)
            {
                _context.RoomFacilities.Remove(roomFacility);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.RoomFacilities.AnyAsync(e => e.Id == id);
        }

        public async Task<bool> ExistsByRoomAndFacilityAsync(int roomId, int facilityId, int? excludeId = null)
        {
            var query = _context.RoomFacilities
                .Where(rf => rf.RoomId == roomId && rf.FacilityId == facilityId);

            if (excludeId.HasValue)
            {
                query = query.Where(rf => rf.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<IEnumerable<RoomFacility>> GetByRoomIdAsync(int roomId)
        {
            return await _context.RoomFacilities
                .Include(rf => rf.Room)
                .Include(rf => rf.Facility)
                .Where(rf => rf.RoomId == roomId)
                .ToListAsync();
        }

        public async Task<IEnumerable<RoomFacility>> GetByFacilityIdAsync(int facilityId)
        {
            return await _context.RoomFacilities
                .Include(rf => rf.Room)
                .Include(rf => rf.Facility)
                .Where(rf => rf.FacilityId == facilityId)
                .ToListAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.RoomFacilities.CountAsync();
        }
    }
} 