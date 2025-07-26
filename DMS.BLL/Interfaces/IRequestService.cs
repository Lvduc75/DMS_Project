using DMS.Models.Entities;
using DMS.Models.DTOs;

namespace DMS.BLL.Interfaces
{
    public interface IRequestService
    {
        Task<IEnumerable<RequestResponseDTO>> GetAllAsync();
        Task<RequestResponseDTO?> GetByIdAsync(int id);
        Task<RequestResponseDTO> CreateAsync(RequestCreateDTO requestDto);
        Task<RequestResponseDTO> UpdateAsync(int id, RequestUpdateDTO requestDto);
        Task<RequestResponseDTO> UpdateStatusAsync(int id, string status);
        Task DeleteAsync(int id);
        Task<IEnumerable<RequestResponseDTO>> GetByUserIdAsync(int userId);
        Task<IEnumerable<RequestResponseDTO>> GetByStatusAsync(string status);
    }
} 