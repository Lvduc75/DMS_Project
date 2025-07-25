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

        public User? GetUserByUsernameAndPassword(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Name == username);
            if (user == null) return null;
            if (BCrypt.Net.BCrypt.Verify(password, user.Password))
                return user;
            return null;
        }
    }
} 