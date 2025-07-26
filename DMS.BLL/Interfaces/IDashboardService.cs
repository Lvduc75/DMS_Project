using DMS.Models.DTOs;

namespace DMS.BLL.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardDTO> GetDashboardStatsAsync();
    }
} 