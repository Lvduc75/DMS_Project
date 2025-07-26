using Microsoft.AspNetCore.Mvc;
using DMS_FE.Helpers;
using DMS_FE.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text;
using Newtonsoft.Json;

namespace DMS_FE.Controllers
{
    public class UserController : Controller
    {
        private readonly ApiHelper _apiHelper;
        public UserController(ApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        // Danh sách user
        public async Task<IActionResult> Index()
        {
            var users = new List<UserModel>();
            var response = await _apiHelper.GetAsync("/api/user");
            if (response.IsSuccessStatusCode)
            {
                users = await response.Content.ReadFromJsonAsync<List<UserModel>>();
            }
            return View(users);
        }

        // Tạo user (GET)
        [HttpGet]
        public IActionResult Create()
        {
            return View(new UserModel());
        }

        // Tạo user (POST)
        [HttpPost]
        public async Task<IActionResult> Create(UserModel model)
        {
            if (!ModelState.IsValid) return View(model);
            
            // Tạo DTO cho API mới
            var createUserDto = new
            {
                name = model.Name,
                email = model.Email,
                password = model.Password,
                role = model.Role ?? "Student",
                phone = model.Phone
            };
            
            var content = new StringContent(JsonConvert.SerializeObject(createUserDto), Encoding.UTF8, "application/json");
            var response = await _apiHelper.PostAsync("/api/Auth/create-user", content);
            
            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Tạo người dùng thành công!";
                return RedirectToAction("Index");
            }
            
            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, error);
            return View(model);
        }

        // Sửa user (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _apiHelper.GetAsync($"/api/user/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();
            var user = await response.Content.ReadFromJsonAsync<UserModel>();
            return View(user);
        }

        // Sửa user (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(UserModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await _apiHelper.PutAsync($"/api/user/{model.Id}", content);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");
            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, error);
            return View(model);
        }

        // Xóa user (GET)
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _apiHelper.GetAsync($"/api/user/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();
            var user = await response.Content.ReadFromJsonAsync<UserModel>();
            return View(user);
        }

        // Xóa user (POST)
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _apiHelper.DeleteAsync($"/api/user/{id}");
            return RedirectToAction("Index");
        }
    }
} 