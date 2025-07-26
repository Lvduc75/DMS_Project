using DMS.Models.Entities;
using DMS.Models.DTOs;

namespace DMS.DAL.Interfaces
{
    public interface IFeeRepository
    {
        Task<IEnumerable<Fee>> GetAllAsync();
        Task<IEnumerable<FeeResponseDTO>> GetAllWithFiltersAsync(string? type, int? month, int? year);
        Task<Fee?> GetByIdAsync(int id);
        Task<FeeResponseDTO?> GetByIdWithDetailsAsync(int id);
        Task<Fee> CreateAsync(Fee fee);
        Task<Fee> UpdateAsync(Fee fee);
        Task DeleteAsync(int id);
        Task<IEnumerable<FeeResponseDTO>> GetByStudentAsync(int studentId);
        Task<IEnumerable<FeeResponseDTO>> GetOverdueFeesAsync();
        Task<bool> StudentExistsAsync(int studentId);
    }
} 