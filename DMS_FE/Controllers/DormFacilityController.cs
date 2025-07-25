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
    public class DormFacilityController : Controller
    {
        private readonly ApiHelper _apiHelper;
        public DormFacilityController(ApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<IActionResult> Manage()
        {
            var response = await _apiHelper.GetAsync("/api/DormFacility");
            var json = await response.Content.ReadAsStringAsync();
            var list = JsonSerializer.Deserialize<List<DormFacilityViewModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var dormRes = await _apiHelper.GetAsync("/api/Dormitory");
            var dormJson = await dormRes.Content.ReadAsStringAsync();
            var dorms = System.Text.Json.JsonSerializer.Deserialize<List<DormitorySimpleViewModel>>(dormJson, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var facRes = await _apiHelper.GetAsync("/api/Facility");
            var facJson = await facRes.Content.ReadAsStringAsync();
            var facilities = System.Text.Json.JsonSerializer.Deserialize<List<FacilityViewModel>>(facJson, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var vm = new DormFacilityViewModel
            {
                Dormitories = dorms,
                Facilities = facilities
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DormFacilityViewModel model)
        {
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            await _apiHelper.PostAsync("/api/DormFacility", content);
            return RedirectToAction("Manage");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _apiHelper.GetAsync("/api/DormFacility");
            var json = await response.Content.ReadAsStringAsync();
            var list = JsonSerializer.Deserialize<List<DormFacilityViewModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var item = list.FirstOrDefault(x => x.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DormFacilityViewModel model)
        {
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            await _apiHelper.PutAsync($"/api/DormFacility/{model.Id}", content);
            return RedirectToAction("Manage");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _apiHelper.GetAsync("/api/DormFacility");
            var json = await response.Content.ReadAsStringAsync();
            var list = JsonSerializer.Deserialize<List<DormFacilityViewModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var item = list.FirstOrDefault(x => x.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int Id)
        {
            var response = await _apiHelper.DeleteAsync($"/api/DormFacility/{Id}");
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