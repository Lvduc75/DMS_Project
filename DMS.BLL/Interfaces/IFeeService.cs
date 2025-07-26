using DMS.Models.Entities;
using DMS.Models.DTOs;

namespace DMS.BLL.Interfaces
{
    public interface IFeeService
    {
        Task<IEnumerable<FeeResponseDTO>> GetAllAsync(string? type, int? month, int? year);
        Task<FeeResponseDTO?> GetByIdAsync(int id);
        Task<FeeResponseDTO> CreateAsync(FeeCreateDTO feeDto);
        Task UpdateAsync(int id, FeeUpdateDTO feeDto);
        Task DeleteAsync(int id);
        Task<IEnumerable<FeeResponseDTO>> GetByStudentAsync(int studentId);
        Task<IEnumerable<FeeResponseDTO>> GetOverdueFeesAsync();
    }
} 