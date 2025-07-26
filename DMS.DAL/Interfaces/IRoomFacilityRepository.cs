using DMS.Models.Entities;
using DMS.Models.DTOs;

namespace DMS.DAL.Interfaces
{
    public interface IRoomFacilityRepository
    {
        Task<IEnumerable<RoomFacility>> GetAllAsync();
        Task<IEnumerable<object>> GetAllWithDetailsAsync();
        Task<RoomFacility?> GetByIdAsync(int id);
        Task<object?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<object>> GetByRoomWithDetailsAsync(int roomId);
        Task<RoomFacility> CreateAsync(RoomFacility roomFacility);
        Task<RoomFacility> UpdateAsync(RoomFacility roomFacility);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsByRoomAndFacilityAsync(int roomId, int facilityId, int? excludeId = null);
        Task<IEnumerable<RoomFacility>> GetByRoomIdAsync(int roomId);
        Task<IEnumerable<RoomFacility>> GetByFacilityIdAsync(int facilityId);
        Task<int> GetCountAsync();
    }
} 