using DMS.DAL.Interfaces;
using DMS.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DMS.DAL.Repositories
{
    public class ConfigPriceRepository : IConfigPriceRepository
    {
        private readonly DormManagementContext _context;

        public ConfigPriceRepository(DormManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ConfigPrice>> GetAllAsync()
        {
            return await _context.ConfigPrices.ToListAsync();
        }

        public async Task<ConfigPrice?> GetByIdAsync(int id)
        {
            return await _context.ConfigPrices.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<ConfigPrice?> GetByTypeAsync(string type)
        {
            return await _context.ConfigPrices.FirstOrDefaultAsync(c => c.Type == type);
        }

        public async Task<ConfigPrice> CreateAsync(ConfigPrice configPrice)
        {
            _context.ConfigPrices.Add(configPrice);
            await _context.SaveChangesAsync();
            return configPrice;
        }

        public async Task<ConfigPrice> UpdateAsync(ConfigPrice configPrice)
        {
            _context.ConfigPrices.Update(configPrice);
            await _context.SaveChangesAsync();
            return configPrice;
        }

        public async Task DeleteAsync(int id)
        {
            var config = await _context.ConfigPrices.FindAsync(id);
            if (config != null)
            {
                _context.ConfigPrices.Remove(config);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> TypeExistsAsync(string type)
        {
            return await _context.ConfigPrices.AnyAsync(c => c.Type == type);
        }

        public async Task<bool> AnyAsync()
        {
            return await _context.ConfigPrices.AnyAsync();
        }

        public async Task<IEnumerable<ConfigPrice>> CreateMultipleAsync(IEnumerable<ConfigPrice> configPrices)
        {
            _context.ConfigPrices.AddRange(configPrices);
            await _context.SaveChangesAsync();
            return configPrices;
        }
    }
} 