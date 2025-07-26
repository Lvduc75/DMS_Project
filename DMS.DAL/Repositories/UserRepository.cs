using DMS.DAL.Interfaces;
using DMS.Models.Entities;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using BCrypt.Net;

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
    }
} 