using DMS.Models.Entities;

namespace DMS.DAL.Interfaces
{
    public interface IRoomRepository
    {
        Task<IEnumerable<Room>> GetAllAsync();
        Task<IEnumerable<object>> GetAllWithDetailsAsync(int? dormitoryId);
        Task<Room?> GetByIdAsync(int id);
        Task<object?> GetByIdWithDetailsAsync(int id);
        Task<object?> GetByStudentWithDetailsAsync(int studentId);
        Task<Room> CreateAsync(Room room);
        Task<Room> UpdateAsync(Room room);
        Task DeleteAsync(int id);
        Task<bool> HasStudentsAsync(int id);
        Task<bool> HasFacilitiesAsync(int id);
    }
} 