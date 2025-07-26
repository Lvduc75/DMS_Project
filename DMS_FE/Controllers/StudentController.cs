using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace DMS_FE.Controllers
{
    public class StudentController : Controller
    {
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
    }
} 