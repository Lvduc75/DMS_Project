using DMS.BLL.Interfaces;
using DMS.DAL.Interfaces;
using DMS.Models.Entities;

namespace DMS.BLL.Services
{
    public class FacilityService : IFacilityService
    {
        private readonly IFacilityRepository _facilityRepository;

        public FacilityService(IFacilityRepository facilityRepository)
        {
            _facilityRepository = facilityRepository;
        }

        public async Task<IEnumerable<object>> GetAllAsync()
        {
            return await _facilityRepository.GetAllWithDetailsAsync();
        }

        public async Task<object?> GetByIdAsync(int id)
        {
            return await _facilityRepository.GetByIdWithDetailsAsync(id);
        }

        public async Task<Facility> CreateAsync(Facility facility)
        {
            if (string.IsNullOrWhiteSpace(facility.Name))
            {
                throw new ArgumentException("Tên tiện ích không được để trống");
            }

            if (facility.UnitPrice < 0)
            {
                throw new ArgumentException("Đơn giá phải >= 0");
            }

            return await _facilityRepository.CreateAsync(facility);
        }

        public async Task<Facility> UpdateAsync(int id, Facility facility)
        {
            var existing = await _facilityRepository.GetByIdAsync(id);
            if (existing == null)
            {
                throw new ArgumentException("Facility not found");
            }

            if (string.IsNullOrWhiteSpace(facility.Name))
            {
                throw new ArgumentException("Tên tiện ích không được để trống");
            }

            if (facility.UnitPrice < 0)
            {
                throw new ArgumentException("Đơn giá phải >= 0");
            }

            existing.Name = facility.Name;
            existing.UnitPrice = facility.UnitPrice;

            return await _facilityRepository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(int id)
        {
            var facility = await _facilityRepository.GetByIdAsync(id);
            if (facility == null)
            {
                throw new ArgumentException("Facility not found");
            }

            await _facilityRepository.DeleteAsync(id);
        }
    }
} 