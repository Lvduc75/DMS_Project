using DMS.Models.DTOs;

namespace DMS.BLL.Interfaces
{
    public interface IUserService
    {
        Task<object> LoginAsync(LoginDTO loginDto);
        Task<IEnumerable<object>> GetAllUsersAsync();
        Task<object> CreateSampleUsersAsync();
        Task<object> CreateDummyUsersAsync(CreateDummyUsersDTO request);
        Task<object> CreateSingleUserAsync(CreateSingleUserDTO request);
        Task<ProfileDTO?> GetProfileAsync(int id);
        Task<ProfileDTO?> UpdateProfileAsync(int id, UpdateProfileDTO request);
    }
} 