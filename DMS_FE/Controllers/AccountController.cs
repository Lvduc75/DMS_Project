using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace DMS_FE.Models
{
    public class LoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Error { get; set; }
    }
}

namespace DMS_FE.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // TODO: Xử lý xác thực thật, tạm thời luôn thành công nếu có username
            if (!string.IsNullOrEmpty(username))
            {
                // Đăng nhập thành công, chuyển về Dashboard
                return RedirectToAction("Index", "Dashboard");
            }
            // Đăng nhập thất bại, quay lại login
            ViewBag.Error = "Sai tài khoản hoặc mật khẩu";
            return View();
        }
    }
} 