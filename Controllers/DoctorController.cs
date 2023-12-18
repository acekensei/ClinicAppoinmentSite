using ClinicAppointmentClient.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace ClinicAppointmentClient.Controllers
{
    public class DoctorController : Controller
    {
        const string API_URL = "https://localhost:7182/api";

        Uri baseAddress = new Uri(API_URL);
        private readonly HttpClient _client;

        public DoctorController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var response = _client.GetAsync($"{_client.BaseAddress}/doctor").Result;
            string data = response.Content.ReadAsStringAsync().Result;
            var docList = JsonConvert.DeserializeObject<List<Doctor>>(data);

            return View(docList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Doctor values)
        {
            if (!ModelState.IsValid) return View(values);

            values.IsAvailable = true;

            // Serialize the object and sends it to thru the API to the DB
            var data = JsonConvert.SerializeObject(values);
            StringContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            var response = _client.PostAsync($"{_client.BaseAddress}/doctor", content).Result;

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
            var response = _client.GetAsync($"{_client.BaseAddress}/doctor/{Id}").Result;
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var doctor = JsonConvert.DeserializeObject<Doctor>(data);
            
                return View(doctor);
            }

            return View();
        }

        [HttpPost]
        public IActionResult Edit(Doctor doctor)
        {
            if (!ModelState.IsValid) return View(doctor);

            string data = JsonConvert.SerializeObject(doctor);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PutAsync($"{_client.BaseAddress}/doctor/{doctor.Id}", content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/doctor/{Id}").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var doctor = JsonConvert.DeserializeObject<Doctor>(data);
                
                return View(doctor);
            }
            return View();
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int Id)
        {
            HttpResponseMessage response = _client.DeleteAsync($"{_client.BaseAddress}/doctor/{Id}").Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View();
        }
    }
}
