using DMS.Models.DTOs;

namespace DMS.DAL.Interfaces
{
    public interface IDashboardRepository
    {
        Task<int> GetTotalRoomsAsync();
        Task<int> GetTotalStudentsAsync();
        Task<int> GetNewRequestsAsync(DateTime fromDate);
        Task<decimal> GetMonthlyRevenueAsync(int month, int year);
    }
} 