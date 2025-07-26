using DMS.BLL.Interfaces;
using DMS.DAL.Interfaces;
using DMS.Models.Entities;

namespace DMS.BLL.Services
{
    public class DormFacilityService : IDormFacilityService
    {
        private readonly IDormFacilityRepository _dormFacilityRepository;

        public DormFacilityService(IDormFacilityRepository dormFacilityRepository)
        {
            _dormFacilityRepository = dormFacilityRepository;
        }

        public async Task<IEnumerable<object>> GetAllAsync()
        {
            return await _dormFacilityRepository.GetAllWithDetailsAsync();
        }

        public async Task<DormFacility> CreateAsync(DormFacility dormFacility)
        {
            if (dormFacility.Quantity < 0)
            {
                throw new ArgumentException("Số lượng phải >= 0");
            }

            return await _dormFacilityRepository.CreateAsync(dormFacility);
        }

        public async Task<DormFacility> UpdateAsync(int id, DormFacility dormFacility)
        {
            var existing = await _dormFacilityRepository.GetByIdAsync(id);
            if (existing == null)
            {
                throw new ArgumentException("DormFacility not found");
            }

            if (dormFacility.Quantity < 0)
            {
                throw new ArgumentException("Số lượng phải >= 0");
            }

            existing.DormitoryId = dormFacility.DormitoryId;
            existing.FacilityId = dormFacility.FacilityId;
            existing.Quantity = dormFacility.Quantity;

            return await _dormFacilityRepository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(int id)
        {
            var df = await _dormFacilityRepository.GetByIdAsync(id);
            if (df == null)
            {
                throw new ArgumentException("DormFacility not found");
            }

            await _dormFacilityRepository.DeleteAsync(id);
        }
    }
} 