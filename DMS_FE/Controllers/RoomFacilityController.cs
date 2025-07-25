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
    public class RoomFacilityController : Controller
    {
        private readonly ApiHelper _apiHelper;
        public RoomFacilityController(ApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<IActionResult> Manage()
        {
            var response = await _apiHelper.GetAsync("/api/RoomFacility");
            var json = await response.Content.ReadAsStringAsync();
            var list = JsonSerializer.Deserialize<List<RoomFacilityViewModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var roomRes = await _apiHelper.GetAsync("/api/Room");
            var roomJson = await roomRes.Content.ReadAsStringAsync();
            var rooms = JsonSerializer.Deserialize<List<RoomSimpleViewModel>>(roomJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var facRes = await _apiHelper.GetAsync("/api/Facility");
            var facJson = await facRes.Content.ReadAsStringAsync();
            var facilities = JsonSerializer.Deserialize<List<FacilityViewModel>>(facJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var vm = new RoomFacilityViewModel
            {
                Rooms = rooms,
                Facilities = facilities
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoomFacilityViewModel model)
        {
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            await _apiHelper.PostAsync("/api/RoomFacility", content);
            return RedirectToAction("Manage");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _apiHelper.GetAsync("/api/RoomFacility");
            var json = await response.Content.ReadAsStringAsync();
            var list = JsonSerializer.Deserialize<List<RoomFacilityViewModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var item = list.FirstOrDefault(x => x.Id == id);
            if (item == null) return NotFound();

            var roomRes = await _apiHelper.GetAsync("/api/Room");
            var roomJson = await roomRes.Content.ReadAsStringAsync();
            var rooms = JsonSerializer.Deserialize<List<RoomSimpleViewModel>>(roomJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var facRes = await _apiHelper.GetAsync("/api/Facility");
            var facJson = await facRes.Content.ReadAsStringAsync();
            var facilities = JsonSerializer.Deserialize<List<FacilityViewModel>>(facJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            item.Rooms = rooms;
            item.Facilities = facilities;
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RoomFacilityViewModel model)
        {
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            await _apiHelper.PutAsync($"/api/RoomFacility/{model.Id}", content);
            return RedirectToAction("Manage");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _apiHelper.GetAsync("/api/RoomFacility");
            var json = await response.Content.ReadAsStringAsync();
            var list = JsonSerializer.Deserialize<List<RoomFacilityViewModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var item = list.FirstOrDefault(x => x.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int Id)
        {
            var response = await _apiHelper.DeleteAsync($"/api/RoomFacility/{Id}");
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