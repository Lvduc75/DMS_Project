using DMS.Models.DTOs;

namespace DMS.BLL.Interfaces
{
    public interface IRoomFacilityService
    {
        Task<IEnumerable<RoomFacilityDTO>> GetAllAsync();
        Task<RoomFacilityDTO?> GetByIdAsync(int id);
        Task<RoomFacilityDTO> CreateAsync(RoomFacilityDTO roomFacilityDto);
        Task<RoomFacilityDTO> UpdateAsync(int id, RoomFacilityDTO roomFacilityDto);
        Task DeleteAsync(int id);
        Task<IEnumerable<RoomFacilityDTO>> GetByRoomIdAsync(int roomId);
        Task<IEnumerable<RoomFacilityDTO>> GetByFacilityIdAsync(int facilityId);
        Task<bool> ExistsAsync(int id);
        Task<int> GetCountAsync();
    }
} 