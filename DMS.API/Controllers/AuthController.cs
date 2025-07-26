using Microsoft.AspNetCore.Mvc;
using DMS.Models.Entities;
using DMS.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DormManagementContext _context;

        public AuthController(DormManagementContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO loginDto)
        {
            if (string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
            {
                return BadRequest("Email và mật khẩu không được để trống");
            }

            // Tìm user theo email
            var user = _context.Users.FirstOrDefault(u => u.Email == loginDto.Email);
            if (user == null)
            {
                return BadRequest("Email không tồn tại");
            }

            // Kiểm tra mật khẩu (tạm thời so sánh trực tiếp, sau này sẽ hash)
            if (user.Password != loginDto.Password)
            {
                return BadRequest("Mật khẩu không đúng");
            }

            // Kiểm tra role
            if (loginDto.Role != user.Role)
            {
                return BadRequest($"Tài khoản này không có quyền đăng nhập với vai trò {loginDto.Role}");
            }

            // Trả về thông tin user (không bao gồm password)
            var userInfo = new
            {
                user.Id,
                user.Name,
                user.Email,
                user.Phone,
                user.Role
            };

            return Ok(userInfo);
        }

        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            var users = _context.Users.Select(u => new
            {
                u.Id,
                u.Name,
                u.Email,
                u.Phone,
                u.Role
            }).ToList();

            return Ok(users);
        }

        [HttpPost("create-sample-users")]
        public IActionResult CreateSampleUsers()
        {
            try
            {
                // Kiểm tra xem đã có user mẫu chưa
                if (_context.Users.Any())
                {
                    return BadRequest("Đã có user trong hệ thống");
                }

                var sampleUsers = new List<User>
                {
                    new User
                    {
                        Name = "Manager Admin",
                        Email = "manager@studorm.com",
                        Password = "123456",
                        Role = "Manager",
                        Phone = "0123456789"
                    },
                    new User
                    {
                        Name = "Student Test",
                        Email = "student@studorm.com",
                        Password = "123456",
                        Role = "Student",
                        Phone = "0987654321"
                    },
                    new User
                    {
                        Name = "Manager 2",
                        Email = "manager2@studorm.com",
                        Password = "123456",
                        Role = "Manager",
                        Phone = "0111222333"
                    },
                    new User
                    {
                        Name = "Student 2",
                        Email = "student2@studorm.com",
                        Password = "123456",
                        Role = "Student",
                        Phone = "0999888777"
                    }
                };

                _context.Users.AddRange(sampleUsers);
                _context.SaveChanges();

                return Ok(new { message = "Đã tạo thành công 4 user mẫu", count = sampleUsers.Count });
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi: {ex.Message}");
            }
        }

        [HttpPost("create-dummy-users")]
        public IActionResult CreateDummyUsers([FromBody] CreateDummyUsersDTO request)
        {
            try
            {
                // Validate input
                if (request.ManagerCount < 0 || request.StudentCount < 0)
                {
                    return BadRequest("Số lượng Manager và Student phải >= 0");
                }

                if (string.IsNullOrEmpty(request.Password))
                {
                    return BadRequest("Password không được để trống");
                }

                // Xóa tất cả user hiện tại nếu được yêu cầu
                if (request.ClearExisting)
                {
                    _context.Users.RemoveRange(_context.Users);
                    _context.SaveChanges();
                }

                var dummyUsers = new List<User>();

                // Tạo Managers
                for (int i = 1; i <= request.ManagerCount; i++)
                {
                    dummyUsers.Add(new User
                    {
                        Name = $"Manager {i}",
                        Email = $"manager{i}@studorm.com",
                        Password = request.Password,
                        Role = "Manager",
                        Phone = $"01234567{i:D2}"
                    });
                }

                // Tạo Students
                for (int i = 1; i <= request.StudentCount; i++)
                {
                    dummyUsers.Add(new User
                    {
                        Name = $"Student {i}",
                        Email = $"student{i}@studorm.com",
                        Password = request.Password,
                        Role = "Student",
                        Phone = $"09876543{i:D2}"
                    });
                }

                _context.Users.AddRange(dummyUsers);
                _context.SaveChanges();

                var result = new
                {
                    message = "Đã tạo thành công dummy data cho User",
                    request = new
                    {
                        request.ManagerCount,
                        request.StudentCount,
                        request.Password,
                        request.ClearExisting
                    },
                    totalCount = dummyUsers.Count,
                    managerCount = dummyUsers.Count(u => u.Role == "Manager"),
                    studentCount = dummyUsers.Count(u => u.Role == "Student"),
                    users = dummyUsers.Select(u => new
                    {
                        u.Id,
                        u.Name,
                        u.Email,
                        u.Phone,
                        u.Role
                    }).ToList()
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi: {ex.Message}");
            }
        }

        [HttpPost("create-user")]
        public IActionResult CreateSingleUser([FromBody] CreateSingleUserDTO request)
        {
            try
            {
                // Validate input
                if (string.IsNullOrEmpty(request.Name))
                {
                    return BadRequest("Tên không được để trống");
                }

                if (string.IsNullOrEmpty(request.Email))
                {
                    return BadRequest("Email không được để trống");
                }

                if (string.IsNullOrEmpty(request.Password))
                {
                    return BadRequest("Password không được để trống");
                }

                if (string.IsNullOrEmpty(request.Role))
                {
                    return BadRequest("Role không được để trống");
                }

                if (request.Role != "Manager" && request.Role != "Student")
                {
                    return BadRequest("Role phải là 'Manager' hoặc 'Student'");
                }

                // Kiểm tra email đã tồn tại chưa
                if (_context.Users.Any(u => u.Email == request.Email))
                {
                    return BadRequest($"Email '{request.Email}' đã tồn tại");
                }

                // Tạo user mới
                var newUser = new User
                {
                    Name = request.Name,
                    Email = request.Email,
                    Password = request.Password,
                    Role = request.Role,
                    Phone = request.Phone ?? ""
                };

                _context.Users.Add(newUser);
                _context.SaveChanges();

                var result = new
                {
                    message = "Đã tạo thành công user",
                    user = new
                    {
                        newUser.Id,
                        newUser.Name,
                        newUser.Email,
                        newUser.Phone,
                        newUser.Role
                    }
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi: {ex.Message}");
            }
        }

        [HttpGet("profile/{id}")]
        public IActionResult GetProfile(int id)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == id);
                if (user == null)
                {
                    return NotFound("Không tìm thấy user");
                }

                var profile = new ProfileDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Phone = user.Phone,
                    Role = user.Role
                };

                return Ok(profile);
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi: {ex.Message}");
            }
        }

        [HttpPut("profile/{id}")]
        public IActionResult UpdateProfile(int id, [FromBody] UpdateProfileDTO request)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == id);
                if (user == null)
                {
                    return NotFound("Không tìm thấy user");
                }

                // Validate input
                if (string.IsNullOrEmpty(request.Name))
                {
                    return BadRequest("Tên không được để trống");
                }

                // Update user information
                user.Name = request.Name;
                user.Phone = request.Phone;

                _context.SaveChanges();

                var result = new ProfileDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Phone = user.Phone,
                    Role = user.Role
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi: {ex.Message}");
            }
        }
    }
} 