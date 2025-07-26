using DMS.Models.Entities;

namespace DMS.BLL.Interfaces
{
    public interface IDormFacilityService
    {
        Task<IEnumerable<object>> GetAllAsync();
        Task<DormFacility> CreateAsync(DormFacility dormFacility);
        Task<DormFacility> UpdateAsync(int id, DormFacility dormFacility);
        Task DeleteAsync(int id);
    }
} 