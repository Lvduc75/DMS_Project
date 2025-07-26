using DMS.BLL.Interfaces;
using DMS.DAL.Interfaces;
using DMS.Models.Entities;

namespace DMS.BLL.Services
{
    public class DormitoryService : IDormitoryService
    {
        private readonly IDormitoryRepository _dormitoryRepository;

        public DormitoryService(IDormitoryRepository dormitoryRepository)
        {
            _dormitoryRepository = dormitoryRepository;
        }

        public async Task<IEnumerable<object>> GetAllAsync()
        {
            return await _dormitoryRepository.GetAllWithDetailsAsync();
        }

        public async Task<object?> GetByIdAsync(int id)
        {
            return await _dormitoryRepository.GetByIdWithDetailsAsync(id);
        }

        public async Task<Dormitory> CreateAsync(Dormitory dormitory)
        {
            if (string.IsNullOrWhiteSpace(dormitory.Name))
            {
                throw new ArgumentException("Tên ký túc xá không được để trống");
            }

            return await _dormitoryRepository.CreateAsync(dormitory);
        }

        public async Task<Dormitory> UpdateAsync(int id, Dormitory dormitory)
        {
            var existing = await _dormitoryRepository.GetByIdAsync(id);
            if (existing == null)
            {
                throw new ArgumentException("Dormitory not found");
            }

            if (string.IsNullOrWhiteSpace(dormitory.Name))
            {
                throw new ArgumentException("Tên ký túc xá không được để trống");
            }

            existing.Name = dormitory.Name;
            return await _dormitoryRepository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(int id)
        {
            var dorm = await _dormitoryRepository.GetByIdAsync(id);
            if (dorm == null)
            {
                throw new ArgumentException("Dormitory not found");
            }

            var hasRooms = await _dormitoryRepository.HasRoomsAsync(id);
            var hasFacilities = await _dormitoryRepository.HasFacilitiesAsync(id);

            if (hasRooms || hasFacilities)
            {
                throw new InvalidOperationException("Dom này đang được sử dụng");
            }

            await _dormitoryRepository.DeleteAsync(id);
        }
    }
} 