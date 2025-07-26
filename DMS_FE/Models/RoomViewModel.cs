namespace DMS_FE.Models
{
    public class RoomViewModel
    {
        // Fields từ Entity Room
        public int Id { get; set; }
        public string Code { get; set; }
        public int Capacity { get; set; }
        public string Status { get; set; }
        public int DormitoryId { get; set; }
        
        // Fields bổ sung cho view
        public string RoomNumber { get; set; } // Alias cho Code
        public int CurrentOccupants { get; set; }
        public int Occupied { get; set; } // Số người đang ở
        public string RoomType { get; set; }
        public int Floor { get; set; }
        public decimal Price { get; set; }
        public string DormitoryName { get; set; }
        public List<string> Facilities { get; set; } = new List<string>();
        public List<StudentInfo> Students { get; set; } = new List<StudentInfo>();
    }

    public class StudentInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StudentId { get; set; }
        public string Email { get; set; }
    }
} 