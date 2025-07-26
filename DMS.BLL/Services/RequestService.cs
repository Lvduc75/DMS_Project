using DMS.BLL.Interfaces;
using DMS.DAL.Interfaces;
using DMS.Models.Entities;
using DMS.Models.DTOs;

namespace DMS.BLL.Services
{
    public class RequestService : IRequestService
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IUserRepository _userRepository;

        public RequestService(IRequestRepository requestRepository, IUserRepository userRepository)
        {
            _requestRepository = requestRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<RequestResponseDTO>> GetAllAsync()
        {
            return await _requestRepository.GetAllWithDetailsAsync();
        }

        public async Task<RequestResponseDTO?> GetByIdAsync(int id)
        {
            return await _requestRepository.GetByIdWithDetailsAsync(id);
        }

        public async Task<RequestResponseDTO> CreateAsync(RequestCreateDTO requestDto)
        {
            // Validate student exists
            var student = await _userRepository.GetByIdAsync(requestDto.StudentId);
            if (student == null)
            {
                throw new ArgumentException("Student not found");
            }

            var request = new Request
            {
                StudentId = requestDto.StudentId,
                Type = requestDto.Type,
                Description = requestDto.Description,
                Status = "Pending",
                CreatedAt = DateTime.Now
            };

            await _requestRepository.CreateAsync(request);

            return new RequestResponseDTO
            {
                Id = request.Id,
                StudentId = request.StudentId,
                StudentName = student.Name,
                ManagerId = request.ManagerId,
                ManagerName = null,
                Type = request.Type,
                Description = request.Description,
                Status = request.Status,
                CreatedAt = request.CreatedAt
            };
        }

        public async Task<RequestResponseDTO> UpdateAsync(int id, RequestUpdateDTO requestDto)
        {
            var request = await _requestRepository.GetByIdAsync(id);
            if (request == null)
            {
                throw new ArgumentException("Request not found");
            }

            // Validate manager if provided
            if (requestDto.ManagerId.HasValue)
            {
                var manager = await _userRepository.GetByIdAsync(requestDto.ManagerId.Value);
                if (manager == null)
                {
                    throw new ArgumentException("Manager not found");
                }
            }

            request.Type = requestDto.Type;
            request.Description = requestDto.Description;
            request.Status = requestDto.Status;
            request.ManagerId = requestDto.ManagerId;

            await _requestRepository.UpdateAsync(request);

            return await _requestRepository.GetByIdWithDetailsAsync(id) ?? 
                throw new InvalidOperationException("Failed to retrieve updated request");
        }

        public async Task<RequestResponseDTO> UpdateStatusAsync(int id, string status)
        {
            var request = await _requestRepository.GetByIdAsync(id);
            if (request == null)
            {
                throw new ArgumentException("Request not found");
            }

            request.Status = status;
            await _requestRepository.UpdateAsync(request);

            return await _requestRepository.GetByIdWithDetailsAsync(id) ?? 
                throw new InvalidOperationException("Failed to retrieve updated request");
        }

        public async Task DeleteAsync(int id)
        {
            var request = await _requestRepository.GetByIdAsync(id);
            if (request == null)
            {
                throw new ArgumentException("Request not found");
            }

            await _requestRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<RequestResponseDTO>> GetByUserIdAsync(int userId)
        {
            return await _requestRepository.GetByUserIdAsync(userId);
        }

        public async Task<IEnumerable<RequestResponseDTO>> GetByStatusAsync(string status)
        {
            return await _requestRepository.GetByStatusAsync(status);
        }
    }
} 