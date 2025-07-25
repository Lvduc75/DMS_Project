using DMS.BLL.Interfaces;
using DMS.Models.DTOs;
using DMS.Models.Entities;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using BCrypt.Net;

namespace DMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO loginDto)
        {
            var token = _authService.Authenticate(loginDto);
            if (token == null)
                return Unauthorized(new { message = "Invalid credentials" });

            return Ok(new { token });
        }

        //[HttpPost("dummy-user")]
        //public IActionResult CreateDummyUser([FromServices] DormManagementContext context)
        //{
        //    // Dummy data
        //    var username = "admin";
        //    var password = "123456";
        //    var email = "admin@example.com";
        //    var role = "Admin";

        //    // Hash password
        //    byte[] salt = new byte[128 / 8];
        //    using (var rng = RandomNumberGenerator.Create())
        //    {
        //        rng.GetBytes(salt);
        //    }
        //    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
        //        password: password,
        //        salt: salt,
        //        prf: KeyDerivationPrf.HMACSHA256,
        //        iterationCount: 10000,
        //        numBytesRequested: 256 / 8));
        //    var user = new User
        //    {
        //        Name = username,
        //        Email = email,
        //        Role = role,
        //        Password = $"{Convert.ToBase64String(salt)}.{hashed}"
        //    };
        //    if (context.Users.Any(u => u.Name == username))
        //        return Conflict("User already exists");
        //    context.Users.Add(user);
        //    context.SaveChanges();
        //    return Ok(new { user.Name, user.Email, user.Role });
        //}

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDTO dto, [FromServices] DormManagementContext context)
        {
            if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password) || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Role))
                return BadRequest("Missing required fields");
            if (dto.Role != "Student" && dto.Role != "Manager")
                return BadRequest("Role must be 'Student' or 'Manager'");
            if (context.Users.Any(u => u.Name == dto.Username))
                return Conflict("User already exists");
            var hashed = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            var user = new User
            {
                Name = dto.Username,
                Email = dto.Email,
                Role = dto.Role,
                Password = hashed
            };
            context.Users.Add(user);
            context.SaveChanges();
            return Ok(new { user.Name, user.Email, user.Role });
        }
    }
} 