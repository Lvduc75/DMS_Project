using DMS.Models.Entities;

namespace DMS.BLL.Interfaces
{
    public interface IConfigPriceService
    {
        Task<IEnumerable<ConfigPrice>> GetAllAsync();
        Task<ConfigPrice?> GetByIdAsync(int id);
        Task<ConfigPrice?> GetByTypeAsync(string type);
        Task<ConfigPrice> CreateAsync(ConfigPrice configPrice);
        Task<ConfigPrice> UpdateAsync(int id, ConfigPrice configPrice);
        Task DeleteAsync(int id);
        Task<IEnumerable<ConfigPrice>> InitializeDefaultPricesAsync();
    }
} 