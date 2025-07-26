using DMS.Models.Entities;

namespace DMS.BLL.Interfaces
{
    public interface IFacilityService
    {
        Task<IEnumerable<object>> GetAllAsync();
        Task<object?> GetByIdAsync(int id);
        Task<Facility> CreateAsync(Facility facility);
        Task<Facility> UpdateAsync(int id, Facility facility);
        Task DeleteAsync(int id);
    }
} 