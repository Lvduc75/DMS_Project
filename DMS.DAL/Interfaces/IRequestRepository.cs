using DMS.Models.Entities;
using DMS.Models.DTOs;

namespace DMS.DAL.Interfaces
{
    public interface IRequestRepository
    {
        Task<IEnumerable<Request>> GetAllAsync();
        Task<IEnumerable<RequestResponseDTO>> GetAllWithDetailsAsync();
        Task<Request?> GetByIdAsync(int id);
        Task<RequestResponseDTO?> GetByIdWithDetailsAsync(int id);
        Task<Request> CreateAsync(Request request);
        Task<Request> UpdateAsync(Request request);
        Task DeleteAsync(int id);
        Task<IEnumerable<RequestResponseDTO>> GetByUserIdAsync(int userId);
        Task<IEnumerable<RequestResponseDTO>> GetByStatusAsync(string status);
        Task<bool> UserExistsAsync(int userId);
    }
} 