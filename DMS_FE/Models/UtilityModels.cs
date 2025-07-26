namespace DMS_FE.Models
{
    public class UtilityReadingModel
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public DateOnly ReadingMonth { get; set; }
        public decimal Electric { get; set; }
        public decimal Water { get; set; }
        public DateTime ImportedAt { get; set; }
        
        // Navigation properties
        public UtilityRoomModel? Room { get; set; }
    }

    public class UtilityRoomModel
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string Status { get; set; } = string.Empty;
        public int DormitoryId { get; set; }
        
        // Navigation properties
        public DormitoryModel? Dormitory { get; set; }
        public List<StudentRoomModel>? StudentRooms { get; set; }
    }

    public class StudentRoomModel
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int RoomId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        
        // Navigation properties
        public UserModel? Student { get; set; }
    }

    public class MonthlyReportModel
    {
        public string Month { get; set; } = string.Empty;
        public List<MonthlyReportItemModel> Reports { get; set; } = new List<MonthlyReportItemModel>();
    }

    public class MonthlyReportItemModel
    {
        public int RoomId { get; set; }
        public UtilityRoomModel? Room { get; set; }
        public decimal TotalElectric { get; set; }
        public decimal TotalWater { get; set; }
        public List<UserModel>? Students { get; set; }
    }
} 