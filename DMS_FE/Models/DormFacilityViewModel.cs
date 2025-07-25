using System.Collections.Generic;

namespace DMS_FE.Models
{
    public class DormFacilityViewModel
    {
        public int Id { get; set; }
        public int DormitoryId { get; set; }
        public string DormitoryName { get; set; }
        public int FacilityId { get; set; }
        public string FacilityName { get; set; }
        public int Quantity { get; set; }
        public List<DormitorySimpleViewModel> Dormitories { get; set; }
        public List<FacilityViewModel> Facilities { get; set; }
    }
} 