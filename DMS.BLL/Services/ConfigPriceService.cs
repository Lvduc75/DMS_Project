using DMS.BLL.Interfaces;
using DMS.DAL.Interfaces;
using DMS.Models.Entities;

namespace DMS.BLL.Services
{
    public class ConfigPriceService : IConfigPriceService
    {
        private readonly IConfigPriceRepository _configPriceRepository;

        public ConfigPriceService(IConfigPriceRepository configPriceRepository)
        {
            _configPriceRepository = configPriceRepository;
        }

        public async Task<IEnumerable<ConfigPrice>> GetAllAsync()
        {
            return await _configPriceRepository.GetAllAsync();
        }

        public async Task<ConfigPrice?> GetByIdAsync(int id)
        {
            return await _configPriceRepository.GetByIdAsync(id);
        }

        public async Task<ConfigPrice?> GetByTypeAsync(string type)
        {
            return await _configPriceRepository.GetByTypeAsync(type);
        }

        public async Task<ConfigPrice> CreateAsync(ConfigPrice configPrice)
        {
            // Check if type already exists
            if (await _configPriceRepository.TypeExistsAsync(configPrice.Type))
            {
                throw new InvalidOperationException($"Configuration for type '{configPrice.Type}' already exists");
            }

            return await _configPriceRepository.CreateAsync(configPrice);
        }

        public async Task<ConfigPrice> UpdateAsync(int id, ConfigPrice configPrice)
        {
            var existing = await _configPriceRepository.GetByIdAsync(id);
            if (existing == null)
            {
                throw new ArgumentException("ConfigPrice not found");
            }

            existing.Type = configPrice.Type;
            existing.UnitPrice = configPrice.UnitPrice;

            return await _configPriceRepository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(int id)
        {
            var config = await _configPriceRepository.GetByIdAsync(id);
            if (config == null)
            {
                throw new ArgumentException("ConfigPrice not found");
            }

            await _configPriceRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ConfigPrice>> InitializeDefaultPricesAsync()
        {
            // Check if already initialized
            if (await _configPriceRepository.AnyAsync())
            {
                throw new InvalidOperationException("Prices have already been initialized");
            }

            var defaultPrices = new List<ConfigPrice>
            {
                new ConfigPrice { Type = "room", UnitPrice = 1500000M, EffectiveFrom = DateOnly.FromDateTime(DateTime.Today) }, // 1.5M VND per month
                new ConfigPrice { Type = "electricity", UnitPrice = 3500M, EffectiveFrom = DateOnly.FromDateTime(DateTime.Today) }, // 3,500 VND per kWh
                new ConfigPrice { Type = "water", UnitPrice = 15000M, EffectiveFrom = DateOnly.FromDateTime(DateTime.Today) }, // 15,000 VND per m3
            };

            return await _configPriceRepository.CreateMultipleAsync(defaultPrices);
        }
    }
} 