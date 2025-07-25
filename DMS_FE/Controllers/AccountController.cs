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
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiBaseUrl;
        public AccountController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _apiBaseUrl = configuration["ApiBaseUrl"] ?? "http://localhost:5000";
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new Models.LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(Models.LoginViewModel model)
        {
            var client = _httpClientFactory.CreateClient();
            var content = new StringContent(JsonConvert.SerializeObject(new { username = model.Username, password = model.Password }), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{_apiBaseUrl}/api/auth/login", content);
            if (response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
                HttpContext.Session.SetString("jwt", (string)result.token);
                TempData["LoginSuccess"] = true;
                return RedirectToAction("Index", "Home");
            }
            model.Error = "Invalid username or password";
            return View(model);
        }
    }
} 