using DMS.BLL.Interfaces;
using DMS.DAL.Interfaces;
using DMS.Models.DTOs;
using DMS.Models.Entities;

namespace DMS.BLL.Services
{
    public class RoomFacilityService : IRoomFacilityService
    {
        private readonly IRoomFacilityRepository _roomFacilityRepository;

        public RoomFacilityService(IRoomFacilityRepository roomFacilityRepository)
        {
            _roomFacilityRepository = roomFacilityRepository;
        }

        public async Task<IEnumerable<RoomFacilityDTO>> GetAllAsync()
        {
            var roomFacilities = await _roomFacilityRepository.GetAllAsync();
            return roomFacilities.Select(rf => new RoomFacilityDTO
            {
                Id = rf.Id,
                RoomId = rf.RoomId,
                FacilityId = rf.FacilityId,
                Quantity = rf.Quantity,
                Status = rf.Status,
                RoomName = rf.Room?.Code,
                FacilityName = rf.Facility?.Name
            });
        }

        public async Task<RoomFacilityDTO?> GetByIdAsync(int id)
        {
            var roomFacility = await _roomFacilityRepository.GetByIdAsync(id);
            if (roomFacility == null) return null;

            return new RoomFacilityDTO
            {
                Id = roomFacility.Id,
                RoomId = roomFacility.RoomId,
                FacilityId = roomFacility.FacilityId,
                Quantity = roomFacility.Quantity,
                Status = roomFacility.Status,
                RoomName = roomFacility.Room?.Code,
                FacilityName = roomFacility.Facility?.Name
            };
        }

        public async Task<RoomFacilityDTO> CreateAsync(RoomFacilityDTO roomFacilityDto)
        {
            if (roomFacilityDto.RoomId <= 0)
                throw new ArgumentException("RoomId is required");

            if (roomFacilityDto.FacilityId <= 0)
                throw new ArgumentException("FacilityId is required");

            if (roomFacilityDto.Quantity <= 0)
                throw new ArgumentException("Quantity must be greater than 0");

            var roomFacility = new RoomFacility
            {
                RoomId = roomFacilityDto.RoomId,
                FacilityId = roomFacilityDto.FacilityId,
                Quantity = roomFacilityDto.Quantity,
                Status = roomFacilityDto.Status ?? "Active"
            };

            var created = await _roomFacilityRepository.CreateAsync(roomFacility);
            
            return new RoomFacilityDTO
            {
                Id = created.Id,
                RoomId = created.RoomId,
                FacilityId = created.FacilityId,
                Quantity = created.Quantity,
                Status = created.Status,
                RoomName = created.Room?.Code,
                FacilityName = created.Facility?.Name
            };
        }

        public async Task<RoomFacilityDTO> UpdateAsync(int id, RoomFacilityDTO roomFacilityDto)
        {
            var existing = await _roomFacilityRepository.GetByIdAsync(id);
            if (existing == null)
                throw new ArgumentException("RoomFacility not found");

            if (roomFacilityDto.RoomId <= 0)
                throw new ArgumentException("RoomId is required");

            if (roomFacilityDto.FacilityId <= 0)
                throw new ArgumentException("FacilityId is required");

            if (roomFacilityDto.Quantity <= 0)
                throw new ArgumentException("Quantity must be greater than 0");

            existing.RoomId = roomFacilityDto.RoomId;
            existing.FacilityId = roomFacilityDto.FacilityId;
            existing.Quantity = roomFacilityDto.Quantity;
            existing.Status = roomFacilityDto.Status ?? existing.Status;

            var updated = await _roomFacilityRepository.UpdateAsync(existing);
            
            return new RoomFacilityDTO
            {
                Id = updated.Id,
                RoomId = updated.RoomId,
                FacilityId = updated.FacilityId,
                Quantity = updated.Quantity,
                Status = updated.Status,
                RoomName = updated.Room?.Code,
                FacilityName = updated.Facility?.Name
            };
        }

        public async Task DeleteAsync(int id)
        {
            var roomFacility = await _roomFacilityRepository.GetByIdAsync(id);
            if (roomFacility == null)
                throw new ArgumentException("RoomFacility not found");

            await _roomFacilityRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<RoomFacilityDTO>> GetByRoomIdAsync(int roomId)
        {
            var roomFacilities = await _roomFacilityRepository.GetByRoomIdAsync(roomId);
            return roomFacilities.Select(rf => new RoomFacilityDTO
            {
                Id = rf.Id,
                RoomId = rf.RoomId,
                FacilityId = rf.FacilityId,
                Quantity = rf.Quantity,
                Status = rf.Status,
                RoomName = rf.Room?.Code,
                FacilityName = rf.Facility?.Name
            });
        }

        public async Task<IEnumerable<RoomFacilityDTO>> GetByFacilityIdAsync(int facilityId)
        {
            var roomFacilities = await _roomFacilityRepository.GetByFacilityIdAsync(facilityId);
            return roomFacilities.Select(rf => new RoomFacilityDTO
            {
                Id = rf.Id,
                RoomId = rf.RoomId,
                FacilityId = rf.FacilityId,
                Quantity = rf.Quantity,
                Status = rf.Status,
                RoomName = rf.Room?.Code,
                FacilityName = rf.Facility?.Name
            });
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _roomFacilityRepository.ExistsAsync(id);
        }

        public async Task<int> GetCountAsync()
        {
            return await _roomFacilityRepository.GetCountAsync();
        }
    }
} 