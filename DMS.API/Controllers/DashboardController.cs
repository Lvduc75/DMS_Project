using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DMS.Models.DTOs;
using DMS.Models.Entities;

namespace DMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly DormManagementContext _context;

    public DashboardController(DormManagementContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<DashboardDTO>> GetDashboardStats()
    {
        try
        {
            // Lấy tổng số phòng
            var totalRooms = await _context.Rooms.CountAsync();

            // Lấy số lượng sinh viên (Role = "Student")
            var totalStudents = await _context.Users
                .Where(u => u.Role == "Student")
                .CountAsync();

            // Lấy số yêu cầu mới (trong 7 ngày gần đây)
            var sevenDaysAgo = DateTime.Now.AddDays(-7);
            var newRequests = await _context.Requests
                .Where(r => r.CreatedAt >= sevenDaysAgo)
                .CountAsync();

            // Lấy doanh thu tháng hiện tại (từ các fee đã thanh toán)
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            var monthlyRevenue = await _context.Fees
                .Where(f => f.CreatedAt.Month == currentMonth && 
                           f.CreatedAt.Year == currentYear &&
                           f.Status == "paid")
                .SumAsync(f => f.Amount);

            var dashboardStats = new DashboardDTO
            {
                TotalRooms = totalRooms,
                TotalStudents = totalStudents,
                NewRequests = newRequests,
                MonthlyRevenue = monthlyRevenue
            };

            return Ok(dashboardStats);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal server error", error = ex.Message });
        }
    }
} 