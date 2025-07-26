namespace DMS_FE.Models
{
    public class RequestModel
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int? ManagerId { get; set; }
        public string Type { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        
        // Navigation properties
        public UserModel? Manager { get; set; }
        public UserModel Student { get; set; }
    }
} 