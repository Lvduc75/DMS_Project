using DMS.Models.Entities;

namespace DMS.DAL.Interfaces
{
    public interface IUserRepository
    {
        User? GetUserByEmailAndPassword(string email, string password);
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllAsync();
        Task<int> GetCountAsync();
        Task CreateMultipleAsync(IEnumerable<User> users);
        Task DeleteAllAsync();
        Task<bool> EmailExistsAsync(string email);
        Task CreateAsync(User user);
        Task<User?> GetByIdAsync(int id);
        Task UpdateAsync(User user);
    }
} 