using DMS.Models.DTOs;

namespace DMS.BLL.Interfaces
{
    public interface IAuthService
    {
        string? Authenticate(LoginDTO loginDto);
    }
} 