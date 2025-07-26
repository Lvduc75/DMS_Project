using DMS.BLL.Interfaces;
using DMS.DAL.Interfaces;
using DMS.Models.DTOs;

namespace DMS.BLL.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<DashboardDTO> GetDashboardStatsAsync()
        {
            // Lấy tổng số phòng
            var totalRooms = await _dashboardRepository.GetTotalRoomsAsync();

            // Lấy số lượng sinh viên (Role = "Student")
            var totalStudents = await _dashboardRepository.GetTotalStudentsAsync();

            // Lấy số yêu cầu mới (trong 7 ngày gần đây)
            var sevenDaysAgo = DateTime.Now.AddDays(-7);
            var newRequests = await _dashboardRepository.GetNewRequestsAsync(sevenDaysAgo);

            // Lấy doanh thu tháng hiện tại (từ các fee đã thanh toán)
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            var monthlyRevenue = await _dashboardRepository.GetMonthlyRevenueAsync(currentMonth, currentYear);

            return new DashboardDTO
            {
                TotalRooms = totalRooms,
                TotalStudents = totalStudents,
                NewRequests = newRequests,
                MonthlyRevenue = monthlyRevenue
            };
        }
    }
} 