using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Text.Json;
using DMS_FE.Models;

namespace DMS_FE.Controllers
{
    public class DashboardController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(HttpClient httpClient, IConfiguration configuration, ILogger<DashboardController> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            // Kiểm tra session
            var userRole = HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(userRole) || userRole != "Manager")
            {
                return RedirectToAction("Index", "Home");
            }

            var userName = HttpContext.Session.GetString("UserName");
            ViewBag.UserName = userName;

            try
            {
                // Lấy API base URL từ configuration
                var apiBaseUrl = _configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5000";
                var apiUrl = $"{apiBaseUrl}/api/Dashboard";
                
                _logger.LogInformation($"Calling API: {apiUrl}");
                
                // Gọi API để lấy dashboard stats
                var response = await _httpClient.GetAsync(apiUrl);
                
                _logger.LogInformation($"API Response Status: {response.StatusCode}");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation($"API Response Content: {content}");
                    
                    var dashboardData = JsonSerializer.Deserialize<DashboardViewModel>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    _logger.LogInformation($"Parsed Data - Rooms: {dashboardData?.TotalRooms}, Students: {dashboardData?.TotalStudents}, Requests: {dashboardData?.NewRequests}, Revenue: {dashboardData?.MonthlyRevenue}");

                    return View(dashboardData);
                }
                else
                {
                    _logger.LogWarning($"API call failed with status: {response.StatusCode}");
                    // Nếu API call thất bại, sử dụng data mặc định
                    var defaultData = new DashboardViewModel
                    {
                        TotalRooms = 0,
                        TotalStudents = 0,
                        NewRequests = 0,
                        MonthlyRevenue = 0
                    };
                    return View(defaultData);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred: {ex.Message}");
                // Log error nếu cần
                var defaultData = new DashboardViewModel
                {
                    TotalRooms = 0,
                    TotalStudents = 0,
                    NewRequests = 0,
                    MonthlyRevenue = 0
                };
                return View(defaultData);
            }
        }
    }
} 