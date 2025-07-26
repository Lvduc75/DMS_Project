using System;

namespace DMS.Models.DTOs
{
    public class RequestCreateDTO
    {
        public int StudentId { get; set; }
        public string Type { get; set; } = null!;
        public string Description { get; set; } = null!;
    }

    public class RequestUpdateDTO
    {
        public string Type { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Status { get; set; } = null!;
        public int? ManagerId { get; set; }
    }

    public class RequestResponseDTO
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = null!;
        public int? ManagerId { get; set; }
        public string? ManagerName { get; set; }
        public string Type { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
} 