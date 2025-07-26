using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using DMS_FE.Models;
using DMS_FE.Helpers;
using System.Text.Json;

namespace DMS_FE.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApiHelper _apiHelper;

        public StudentController(ApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public IActionResult Index()
        {
            // Kiểm tra session
            var userRole = HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(userRole) || userRole != "Student")
            {
                return RedirectToAction("Index", "Home");
            }

            var userName = HttpContext.Session.GetString("UserName");
            ViewBag.UserName = userName;
            
            return View();
        }

        public IActionResult Home()
        {
            // Kiểm tra session
            var userRole = HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(userRole) || userRole != "Student")
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        public async Task<IActionResult> Dormitories()
        {
            // Kiểm tra session
            var userRole = HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(userRole) || userRole != "Student")
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var response = await _apiHelper.GetAsync("/api/Student/dormitories");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var dormitories = JsonSerializer.Deserialize<List<DormitorySimpleViewModel>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return View(dormitories);
                }
            }
            catch (Exception ex)
            {
                // Log error
                ViewBag.ErrorMessage = "Không thể tải dữ liệu ký túc xá: " + ex.Message;
            }

            return View(new List<DormitorySimpleViewModel>());
        }

        public async Task<IActionResult> Rooms(string dormitoryId = null)
        {
            // Kiểm tra session
            var userRole = HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(userRole) || userRole != "Student")
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                string endpoint = "/api/Student/rooms";
                if (!string.IsNullOrEmpty(dormitoryId))
                {
                    endpoint += $"?dormitoryId={dormitoryId}";
                }

                var response = await _apiHelper.GetAsync(endpoint);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var rooms = JsonSerializer.Deserialize<List<RoomViewModel>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return View(rooms);
                }
            }
            catch (Exception ex)
            {
                // Log error
                ViewBag.ErrorMessage = "Không thể tải dữ liệu phòng: " + ex.Message;
            }

            return View(new List<RoomViewModel>());
        }

        public async Task<IActionResult> Profile()
        {
            // Kiểm tra session
            var userRole = HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(userRole) || userRole != "Student")
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var response = await _apiHelper.GetAsync($"/api/Student/profile/{userId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var user = JsonSerializer.Deserialize<UserModel>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return View(user);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Không thể tải thông tin hồ sơ: " + ex.Message;
            }

            return View(new UserModel());
        }

        public async Task<IActionResult> RoomAssignment()
        {
            // Kiểm tra session
            var userRole = HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(userRole) || userRole != "Student")
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var response = await _apiHelper.GetAsync($"/api/Room/student/{userId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var room = JsonSerializer.Deserialize<RoomViewModel>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return View(room);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Không thể tải thông tin phòng: " + ex.Message;
            }

            return View(new RoomViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile()
        {
            // Kiểm tra session
            var userRole = HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(userRole) || userRole != "Student")
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var formData = new Dictionary<string, object>
                {
                    ["Name"] = Request.Form["Name"].ToString(),
                    ["Email"] = Request.Form["Email"].ToString(),
                    ["Phone"] = Request.Form["Phone"].ToString()
                };

                if (!string.IsNullOrEmpty(Request.Form["NewPassword"].ToString()))
                {
                    formData["NewPassword"] = Request.Form["NewPassword"].ToString();
                }

                var content = new StringContent(
                    JsonSerializer.Serialize(formData),
                    System.Text.Encoding.UTF8,
                    "application/json"
                );

                var response = await _apiHelper.PatchAsync($"/api/User/{userId}/profile", content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Cập nhật hồ sơ thành công!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi cập nhật hồ sơ.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Lỗi kết nối: " + ex.Message;
            }

            return RedirectToAction("Profile");
        }

        public async Task<IActionResult> Fees()
        {
            // Kiểm tra session
            var userRole = HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(userRole) || userRole != "Student")
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var response = await _apiHelper.GetAsync($"/api/Fee/GetByStudent/{userId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var fees = JsonSerializer.Deserialize<List<FeeModel>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return View(fees);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Không thể tải thông tin phí: " + ex.Message;
            }

            return View(new List<FeeModel>());
        }

        public async Task<IActionResult> UtilityReadings()
        {
            // Kiểm tra session
            var userRole = HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(userRole) || userRole != "Student")
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var response = await _apiHelper.GetAsync($"/api/Utility/GetByStudent/{userId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var utilities = JsonSerializer.Deserialize<List<UtilityReading>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return View(utilities);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Không thể tải thông tin tiện ích: " + ex.Message;
            }

            return View(new List<UtilityReading>());
        }

        public async Task<IActionResult> Requests()
        {
            // Kiểm tra session
            var userRole = HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(userRole) || userRole != "Student")
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var response = await _apiHelper.GetAsync($"/api/request/user/{userId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var requests = JsonSerializer.Deserialize<List<RequestModel>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return View(requests);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Không thể tải thông tin yêu cầu: " + ex.Message;
            }

            return View(new List<RequestModel>());
        }

        public IActionResult CreateRequest()
        {
            // Kiểm tra session
            var userRole = HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(userRole) || userRole != "Student")
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            // Redirect to Request controller with student ID
            return RedirectToAction("Create", "Request", new { studentId = int.Parse(userId) });
        }

        public IActionResult News()
        {
            // Kiểm tra session
            var userRole = HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(userRole) || userRole != "Student")
            {
                return RedirectToAction("Login", "Account");
            }

            // TODO: Implement news functionality
            return View();
        }

        public IActionResult Facilities()
        {
            // Kiểm tra session
            var userRole = HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(userRole) || userRole != "Student")
            {
                return RedirectToAction("Login", "Account");
            }

            // TODO: Implement facilities functionality
            return View();
        }
    }
} 