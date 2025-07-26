using DMS.BLL.Interfaces;
using DMS.DAL.Interfaces;
using DMS.Models.DTOs;
using DMS.Models.Entities;

namespace DMS.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;

        public UserService(IUserRepository userRepository, IAuthService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
        }

        public async Task<object> LoginAsync(LoginDTO loginDto)
        {
            if (string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
            {
                throw new ArgumentException("Email và mật khẩu không được để trống");
            }

            // Tìm user theo email
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);
            if (user == null)
            {
                throw new ArgumentException("Email không tồn tại");
            }

            // Kiểm tra mật khẩu (tạm thời so sánh trực tiếp, sau này sẽ hash)
            if (user.Password != loginDto.Password)
            {
                throw new ArgumentException("Mật khẩu không đúng");
            }

            // Kiểm tra role
            if (loginDto.Role != user.Role)
            {
                throw new ArgumentException($"Tài khoản này không có quyền đăng nhập với vai trò {loginDto.Role}");
            }

            // Tạo JWT token
            var token = _authService.Authenticate(loginDto);
            if (token == null)
            {
                throw new InvalidOperationException("Không thể tạo token");
            }

            // Trả về thông tin user (không bao gồm password) và token
            var userInfo = new
            {
                user.Id,
                user.Name,
                user.Email,
                user.Phone,
                user.Role
            };

            return new { token, user = userInfo };
        }

        public async Task<IEnumerable<object>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(u => new
            {
                u.Id,
                u.Name,
                u.Email,
                u.Phone,
                u.Role
            });
        }

        public async Task<object> CreateSampleUsersAsync()
        {
            // Kiểm tra xem đã có user mẫu chưa
            var existingCount = await _userRepository.GetCountAsync();
            if (existingCount > 0)
            {
                throw new InvalidOperationException("Đã có user trong hệ thống");
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

            await _userRepository.CreateMultipleAsync(sampleUsers);

            return new { message = "Đã tạo thành công 4 user mẫu", count = sampleUsers.Count };
        }

        public async Task<object> CreateDummyUsersAsync(CreateDummyUsersDTO request)
        {
            // Validate input
            if (request.ManagerCount < 0 || request.StudentCount < 0)
            {
                throw new ArgumentException("Số lượng Manager và Student phải >= 0");
            }

            if (string.IsNullOrEmpty(request.Password))
            {
                throw new ArgumentException("Password không được để trống");
            }

            // Xóa tất cả user hiện tại nếu được yêu cầu
            if (request.ClearExisting)
            {
                await _userRepository.DeleteAllAsync();
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

            await _userRepository.CreateMultipleAsync(dummyUsers);

            return new
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
        }

        public async Task<object> CreateSingleUserAsync(CreateSingleUserDTO request)
        {
            // Validate input
            if (string.IsNullOrEmpty(request.Name))
            {
                throw new ArgumentException("Tên không được để trống");
            }

            if (string.IsNullOrEmpty(request.Email))
            {
                throw new ArgumentException("Email không được để trống");
            }

            if (string.IsNullOrEmpty(request.Password))
            {
                throw new ArgumentException("Password không được để trống");
            }

            if (string.IsNullOrEmpty(request.Role))
            {
                throw new ArgumentException("Role không được để trống");
            }

            if (request.Role != "Manager" && request.Role != "Student")
            {
                throw new ArgumentException("Role phải là 'Manager' hoặc 'Student'");
            }

            // Kiểm tra email đã tồn tại chưa
            if (await _userRepository.EmailExistsAsync(request.Email))
            {
                throw new ArgumentException($"Email '{request.Email}' đã tồn tại");
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

            await _userRepository.CreateAsync(newUser);

            return new
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
        }

        public async Task<ProfileDTO?> GetProfileAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return null;
            }

            return new ProfileDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                Role = user.Role
            };
        }

        public async Task<ProfileDTO?> UpdateProfileAsync(int id, UpdateProfileDTO request)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return null;
            }

            // Validate input
            if (string.IsNullOrEmpty(request.Name))
            {
                throw new ArgumentException("Tên không được để trống");
            }

            // Update user information
            user.Name = request.Name;
            user.Phone = request.Phone;

            await _userRepository.UpdateAsync(user);

            return new ProfileDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                Role = user.Role
            };
        }
    }
} 