using DMS.DAL.Interfaces;
using DMS.Models.Entities;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;

namespace DMS.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DormManagementContext _context;
        public UserRepository(DormManagementContext context)
        {
            _context = context;
        }

        public User? GetUserByEmailAndPassword(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) return null;
            // Tạm thời so sánh trực tiếp, sau này sẽ hash
            if (user.Password == password)
                return user;
            return null;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Users.CountAsync();
        }

        public async Task CreateMultipleAsync(IEnumerable<User> users)
        {
            await _context.Users.AddRangeAsync(users);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAllAsync()
        {
            _context.Users.RemoveRange(_context.Users);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task CreateAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
} 