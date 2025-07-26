namespace DMS_FE.Models
{
    public class DormitorySimpleViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // Các fields bổ sung cho view (không có trong Entity nhưng cần thiết cho UI)
        public int TotalRooms { get; set; }
        public int AvailableRooms { get; set; }
        public int OccupiedRooms { get; set; }
        public List<string> Facilities { get; set; } = new List<string>();
    }
} 