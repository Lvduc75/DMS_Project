namespace DMS_FE.Models
{
    public class RoomFacilityModel
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int FacilityId { get; set; }
        public int Quantity { get; set; }
        
        // Navigation properties
        public RoomModel? Room { get; set; }
        public FacilityModel? Facility { get; set; }
    }

    public class FacilityModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
    }
} 