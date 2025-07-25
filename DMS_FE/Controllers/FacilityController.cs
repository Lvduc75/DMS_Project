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
    public class FacilityController : Controller
    {
        private readonly ApiHelper _apiHelper;
        public FacilityController(ApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<IActionResult> Manage()
        {
            var response = await _apiHelper.GetAsync("/api/Facility");
            var json = await response.Content.ReadAsStringAsync();
            var facilities = JsonSerializer.Deserialize<List<FacilityViewModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(facilities);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(FacilityViewModel model)
        {
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            await _apiHelper.PostAsync("/api/Facility", content);
            return RedirectToAction("Manage");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _apiHelper.GetAsync("/api/Facility");
            var json = await response.Content.ReadAsStringAsync();
            var facilities = JsonSerializer.Deserialize<List<FacilityViewModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var facility = facilities.FirstOrDefault(f => f.Id == id);
            if (facility == null) return NotFound();
            return View(facility);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(FacilityViewModel model)
        {
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            await _apiHelper.PutAsync($"/api/Facility/{model.Id}", content);
            return RedirectToAction("Manage");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _apiHelper.GetAsync("/api/Facility");
            var json = await response.Content.ReadAsStringAsync();
            var facilities = JsonSerializer.Deserialize<List<FacilityViewModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var facility = facilities.FirstOrDefault(f => f.Id == id);
            if (facility == null) return NotFound();
            return View(facility);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int Id)
        {
            var response = await _apiHelper.DeleteAsync($"/api/Facility/{Id}");
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