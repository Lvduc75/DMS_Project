using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using DMS_FE.Models;
using DMS_FE.Helpers;

namespace DMS_FE.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApiHelper _apiHelper;

        public AccountController(ApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        [HttpGet]
        public IActionResult Login()
        {
            // Redirect về trang chủ vì login form ở đó
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password) || string.IsNullOrEmpty(model.Role))
            {
                TempData["Error"] = "Vui lòng nhập đầy đủ thông tin";
                return RedirectToAction("Index", "Home");
            }

            try
            {
                var loginData = new
                {
                    Email = model.Email,
                    Password = model.Password,
                    Role = model.Role
                };

                var content = new StringContent(
                    JsonConvert.SerializeObject(loginData),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _apiHelper.PostAsync("/api/Auth/login", content);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var userInfo = JsonConvert.DeserializeObject<UserInfoViewModel>(responseContent);
                    
                    // Lưu thông tin user vào session
                    HttpContext.Session.SetString("UserId", userInfo.Id.ToString());
                    HttpContext.Session.SetString("UserName", userInfo.Name);
                    HttpContext.Session.SetString("UserEmail", userInfo.Email);
                    HttpContext.Session.SetString("UserRole", userInfo.Role);

                    // Chuyển hướng theo role
                    if (userInfo.Role == "Manager")
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else if (userInfo.Role == "Student")
                    {
                        return RedirectToAction("Index", "Student");
                    }
                    else
                    {
                        TempData["Error"] = "Vai trò không hợp lệ";
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    TempData["Error"] = errorContent.Replace("\"", "");
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi kết nối: " + ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
} 