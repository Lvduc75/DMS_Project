using DMS.DAL.Interfaces;
using DMS.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DMS.DAL.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly DormManagementContext _context;

        public DashboardRepository(DormManagementContext context)
        {
            _context = context;
        }

        public async Task<int> GetTotalRoomsAsync()
        {
            return await _context.Rooms.CountAsync();
        }

        public async Task<int> GetTotalStudentsAsync()
        {
            return await _context.Users
                .Where(u => u.Role == "Student")
                .CountAsync();
        }

        public async Task<int> GetNewRequestsAsync(DateTime fromDate)
        {
            return await _context.Requests
                .Where(r => r.CreatedAt >= fromDate)
                .CountAsync();
        }

        public async Task<decimal> GetMonthlyRevenueAsync(int month, int year)
        {
            return await _context.Fees
                .Where(f => f.CreatedAt.Month == month && 
                           f.CreatedAt.Year == year &&
                           f.Status == "paid")
                .SumAsync(f => f.Amount);
        }
    }
} 