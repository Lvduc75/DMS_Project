using DMS.Models.Entities;

namespace DMS.DAL.Interfaces
{
    public interface IDormFacilityRepository
    {
        Task<IEnumerable<DormFacility>> GetAllAsync();
        Task<IEnumerable<object>> GetAllWithDetailsAsync();
        Task<DormFacility?> GetByIdAsync(int id);
        Task<DormFacility> CreateAsync(DormFacility dormFacility);
        Task<DormFacility> UpdateAsync(DormFacility dormFacility);
        Task DeleteAsync(int id);
    }
} 