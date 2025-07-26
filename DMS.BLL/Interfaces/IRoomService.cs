using DMS.Models.Entities;

namespace DMS.BLL.Interfaces
{
    public interface IRoomService
    {
        Task<IEnumerable<object>> GetAllAsync(int? dormitoryId);
        Task<object?> GetByIdAsync(int id);
        Task<object?> GetByStudentAsync(int studentId);
        Task<Room> CreateAsync(Room room);
        Task<Room> UpdateAsync(int id, Room room);
        Task DeleteAsync(int id);
    }
} 