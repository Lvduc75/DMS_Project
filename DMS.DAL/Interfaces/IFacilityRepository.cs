using DMS.Models.Entities;

namespace DMS.DAL.Interfaces
{
    public interface IFacilityRepository
    {
        Task<IEnumerable<Facility>> GetAllAsync();
        Task<IEnumerable<object>> GetAllWithDetailsAsync();
        Task<Facility?> GetByIdAsync(int id);
        Task<object?> GetByIdWithDetailsAsync(int id);
        Task<Facility> CreateAsync(Facility facility);
        Task<Facility> UpdateAsync(Facility facility);
        Task DeleteAsync(int id);
    }
} 