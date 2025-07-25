using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DMS_FE.Helpers;
using DMS_FE.Models;

namespace DMS_FE.Controllers
{
    public class DormitoryController : Controller
    {
        private readonly ApiHelper _apiHelper;
        public DormitoryController(ApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<IActionResult> Manage()
        {
            var response = await _apiHelper.GetAsync("/api/Dormitory");
            var json = await response.Content.ReadAsStringAsync();
            var dormitories = JsonSerializer.Deserialize<List<DormitoryModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(dormitories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string Name)
        {
            var dorm = new DormitoryModel { Name = Name };
            var content = new StringContent(JsonSerializer.Serialize(dorm), Encoding.UTF8, "application/json");
            await _apiHelper.PostAsync("/api/Dormitory", content);
            return RedirectToAction("Manage");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _apiHelper.GetAsync("/api/Dormitory");
            var json = await response.Content.ReadAsStringAsync();
            var dormitories = JsonSerializer.Deserialize<List<DormitoryModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var dorm = dormitories.FirstOrDefault(d => d.Id == id);
            if (dorm == null) return NotFound();
            return View(dorm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int Id, string Name)
        {
            var dorm = new DormitoryModel { Id = Id, Name = Name };
            var content = new StringContent(JsonSerializer.Serialize(dorm), Encoding.UTF8, "application/json");
            await _apiHelper.PutAsync($"/api/Dormitory/{Id}", content);
            return RedirectToAction("Manage");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _apiHelper.GetAsync("/api/Dormitory");
            var json = await response.Content.ReadAsStringAsync();
            var dormitories = JsonSerializer.Deserialize<List<DormitoryModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var dorm = dormitories.FirstOrDefault(d => d.Id == id);
            if (dorm == null) return NotFound();
            return View(dorm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int Id)
        {
            var response = await _apiHelper.DeleteAsync($"/api/Dormitory/{Id}");
            if (!response.IsSuccessStatusCode)
            {
                var msg = await response.Content.ReadAsStringAsync();
                TempData["DeleteError"] = msg;
                return RedirectToAction("Delete", new { id = Id });
            }
            return RedirectToAction("Manage");
        }
    }
} 