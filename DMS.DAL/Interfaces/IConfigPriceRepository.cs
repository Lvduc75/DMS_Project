using DMS.Models.Entities;

namespace DMS.DAL.Interfaces
{
    public interface IConfigPriceRepository
    {
        Task<IEnumerable<ConfigPrice>> GetAllAsync();
        Task<ConfigPrice?> GetByIdAsync(int id);
        Task<ConfigPrice?> GetByTypeAsync(string type);
        Task<ConfigPrice> CreateAsync(ConfigPrice configPrice);
        Task<ConfigPrice> UpdateAsync(ConfigPrice configPrice);
        Task DeleteAsync(int id);
        Task<bool> TypeExistsAsync(string type);
        Task<bool> AnyAsync();
        Task<IEnumerable<ConfigPrice>> CreateMultipleAsync(IEnumerable<ConfigPrice> configPrices);
    }
} 