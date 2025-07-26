namespace DMS_FE.Models
{
    public class RoomModel
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string Status { get; set; } = string.Empty;
        public int DormitoryId { get; set; }
        
        // Navigation properties
        public DormitoryModel? Dormitory { get; set; }
    }
} 