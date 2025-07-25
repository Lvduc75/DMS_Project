using System.Collections.Generic;

namespace DMS_FE.Models
{
    public class RoomFacilityViewModel
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public int FacilityId { get; set; }
        public string FacilityName { get; set; }
        public int Quantity { get; set; }
        public List<RoomSimpleViewModel> Rooms { get; set; }
        public List<FacilityViewModel> Facilities { get; set; }
    }

    public class RoomSimpleViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
    }
} 