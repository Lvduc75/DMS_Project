namespace DMS_FE.Models
{
    public class RequestManageViewModel
    {
        public List<RequestModel> Requests { get; set; } = new List<RequestModel>();
        public string? FilterStatus { get; set; }
        public int TotalRequests { get; set; }
        public int PendingRequests { get; set; }
        public int ApprovedRequests { get; set; }
        public int RejectedRequests { get; set; }
    }
} 