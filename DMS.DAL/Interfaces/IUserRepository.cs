using DMS.Models.Entities;

namespace DMS.DAL.Interfaces
{
    public interface IUserRepository
    {
        User? GetUserByEmailAndPassword(string email, string password);
    }
} 