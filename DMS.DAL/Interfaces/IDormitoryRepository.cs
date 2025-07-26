using DMS.Models.Entities;

namespace DMS.DAL.Interfaces
{
    public interface IDormitoryRepository
    {
        Task<IEnumerable<Dormitory>> GetAllAsync();
        Task<IEnumerable<object>> GetAllWithDetailsAsync();
        Task<Dormitory?> GetByIdAsync(int id);
        Task<object?> GetByIdWithDetailsAsync(int id);
        Task<Dormitory> CreateAsync(Dormitory dormitory);
        Task<Dormitory> UpdateAsync(Dormitory dormitory);
        Task DeleteAsync(int id);
        Task<bool> HasRoomsAsync(int id);
        Task<bool> HasFacilitiesAsync(int id);
    }
} 