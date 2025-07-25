using DMS.Models.Entities;

namespace DMS.DAL.Interfaces
{
    public interface IUserRepository
    {
        User? GetUserByUsernameAndPassword(string username, string password);
    }
} 