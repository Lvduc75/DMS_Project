namespace DMS_FE.Models
{
    public class UtilityReading
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public DateOnly ReadingMonth { get; set; }
        public decimal Electric { get; set; }
        public decimal Water { get; set; }
        public DateTime ImportedAt { get; set; }
        
        // Navigation property
        public RoomViewModel? Room { get; set; }
    }
} 