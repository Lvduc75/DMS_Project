namespace DMS.Models.DTOs
{
    public class LoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // Student hoặc Manager
    }

    public class RegisterDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } // Student hoặc Manager
    }

    public class CreateDummyUsersDTO
    {
        public int ManagerCount { get; set; } = 5;
        public int StudentCount { get; set; } = 15;
        public string Password { get; set; } = "123456";
        public bool ClearExisting { get; set; } = true;
    }

    public class CreateSingleUserDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // Student hoặc Manager
        public string? Phone { get; set; }
    }

    public class ProfileDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string Role { get; set; }
    }

    public class UpdateProfileDTO
    {
        public string Name { get; set; }
        public string? Phone { get; set; }
    }
} 