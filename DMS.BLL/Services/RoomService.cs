using DMS.BLL.Interfaces;
using DMS.DAL.Interfaces;
using DMS.Models.Entities;

namespace DMS.BLL.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;

        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<IEnumerable<object>> GetAllAsync(int? dormitoryId)
        {
            return await _roomRepository.GetAllWithDetailsAsync(dormitoryId);
        }

        public async Task<object?> GetByIdAsync(int id)
        {
            return await _roomRepository.GetByIdWithDetailsAsync(id);
        }

        public async Task<object?> GetByStudentAsync(int studentId)
        {
            return await _roomRepository.GetByStudentWithDetailsAsync(studentId);
        }

        public async Task<Room> CreateAsync(Room room)
        {
            if (room == null || string.IsNullOrWhiteSpace(room.Code) || room.Capacity <= 0 || string.IsNullOrWhiteSpace(room.Status))
            {
                throw new ArgumentException("Thông tin phòng không hợp lệ");
            }

            return await _roomRepository.CreateAsync(room);
        }

        public async Task<Room> UpdateAsync(int id, Room room)
        {
            if (id != room.Id)
            {
                throw new ArgumentException("Id không khớp");
            }

            var existing = await _roomRepository.GetByIdAsync(id);
            if (existing == null)
            {
                throw new ArgumentException("Room not found");
            }

            existing.Code = room.Code;
            existing.Capacity = room.Capacity;
            existing.Status = room.Status;
            existing.DormitoryId = room.DormitoryId;

            return await _roomRepository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(int id)
        {
            var room = await _roomRepository.GetByIdAsync(id);
            if (room == null)
            {
                throw new ArgumentException("Room not found");
            }

            // Kiểm tra nếu phòng có sinh viên hoặc tiện ích thì không cho xóa
            var hasStudents = await _roomRepository.HasStudentsAsync(id);
            var hasFacilities = await _roomRepository.HasFacilitiesAsync(id);

            if (hasStudents || hasFacilities)
            {
                throw new InvalidOperationException("Phòng đang có sinh viên hoặc tiện ích, không thể xóa");
            }

            await _roomRepository.DeleteAsync(id);
        }
    }
} 