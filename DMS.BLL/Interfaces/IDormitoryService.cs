using DMS.Models.Entities;

namespace DMS.BLL.Interfaces
{
    public interface IDormitoryService
    {
        Task<IEnumerable<object>> GetAllAsync();
        Task<object?> GetByIdAsync(int id);
        Task<Dormitory> CreateAsync(Dormitory dormitory);
        Task<Dormitory> UpdateAsync(int id, Dormitory dormitory);
        Task DeleteAsync(int id);
    }
} 