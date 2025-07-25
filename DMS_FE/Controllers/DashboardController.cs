using Microsoft.AspNetCore.Mvc;

namespace DMS_FE.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
} 