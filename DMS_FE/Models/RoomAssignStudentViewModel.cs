using System.Collections.Generic;

namespace DMS_FE.Models
{
    public class RoomAssignStudentViewModel
    {
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public int Capacity { get; set; }
        public int CurrentCount { get; set; }
        public List<StudentSimpleViewModel> Students { get; set; }
        public int StudentId { get; set; }
    }
    public class StudentSimpleViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
} 