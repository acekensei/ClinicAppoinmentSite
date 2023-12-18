using ClinicAppointmentClient.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace ClinicAppointmentClient.Controllers
{
    public class TimeSlotController : Controller
    {
        const string API_URL = "https://localhost:7182/api";

        Uri baseAddress = new Uri(API_URL);
        private readonly HttpClient _client;

        public TimeSlotController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var response = _client.GetAsync($"{_client.BaseAddress}/timeslot").Result;
            string data = response.Content.ReadAsStringAsync().Result;
            var slotList = JsonConvert.DeserializeObject<List<TimeSlot>>(data);

            return View(slotList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(TimeSlot values)
        {
            if (!ModelState.IsValid) return View(values);

            // Serialize the object and sends it to thru the API to the DB
            var data = JsonConvert.SerializeObject(values);
            StringContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            var response = _client.PostAsync($"{_client.BaseAddress}/timeslot", content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(values);
            }
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var response = _client.GetAsync($"{_client.BaseAddress}/timeslot/{Id}").Result;
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var timeslot = JsonConvert.DeserializeObject<TimeSlot>(data);

                return View(timeslot);
            }

            return View();
        }

        [HttpPost]
        public IActionResult Edit(TimeSlot timeslot)
        {
            if (!ModelState.IsValid) return View(timeslot);

            string data = JsonConvert.SerializeObject(timeslot);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PutAsync($"{_client.BaseAddress}/timeslot/{timeslot.Id}", content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/timeslot/{Id}").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var timeslot = JsonConvert.DeserializeObject<TimeSlot>(data);

                return View(timeslot);
            }
            return View();
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int Id)
        {
            HttpResponseMessage response = _client.DeleteAsync($"{_client.BaseAddress}/timeslot/{Id}").Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View();
        }
    }
}
