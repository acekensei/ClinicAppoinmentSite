using ClinicAppointmentClient.Models;
using ClinicAppointmentClient.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;

namespace ClinicAppointmentClient.Controllers
{
    public class AppointmentController : Controller
    {
        const string API_URL = "https://localhost:7182/api";

        Uri baseAddress = new Uri(API_URL);
        private readonly HttpClient _client;

        public AppointmentController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult Index(string term, int currentPage = 1)
        {
            var response = _client.GetAsync($"{_client.BaseAddress}/appointment").Result;
            string data = response.Content.ReadAsStringAsync().Result;
            var billList = JsonConvert.DeserializeObject<List<Appointment>>(data);

            var totalRecords = billList.Count(); // Set the total amount of records
            var pageSize = 5; // Set the page size
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            var obj = new AppointmentVM
            {
                Data = billList.AsQueryable(),
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalPages = totalPages,
                term = term,
            };

            if (String.IsNullOrEmpty(term))
            {
                obj.Data = billList.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList().AsQueryable();
                return View(obj);
            }
            else
            {
                term = term.ToLower();
                var searchedItems = obj.Data.Where(s => s.PatientName.ToLower().Contains(term)).ToList();

                var totalRecords2 = searchedItems.Count(); // Set the total amount of records
                obj.TotalPages = (int)Math.Ceiling(totalRecords2 / (double)obj.PageSize);
                obj.CurrentPage = currentPage;

                // Apply pagination to the filtered results
                searchedItems = searchedItems.Skip((currentPage - 1) * obj.PageSize).Take(obj.PageSize).ToList();

                obj.Data = searchedItems.AsQueryable();

                return View(obj);
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            // Pull API data for washers
            var doctorResponse = _client.GetAsync($"{_client.BaseAddress}/doctor").Result;
            var doctorData = doctorResponse.Content.ReadAsStringAsync().Result;
            List<Doctor> doctorList = JsonConvert.DeserializeObject<List<Doctor>>(doctorData)!;

            // Pull API data for vehicle
            var timeslotResponse = _client.GetAsync($"{_client.BaseAddress}/timeslot").Result;
            var timeslotData = timeslotResponse.Content.ReadAsStringAsync().Result;
            List<TimeSlot> timeslotList = JsonConvert.DeserializeObject<List<TimeSlot>>(timeslotData)!;

            // To prevent issues, if a washer is not avail then 
            // they will not be displayed in the dropdown list
            var doctors = doctorList.AsQueryable();
            var availDoctors = doctors.Where(s => s.IsAvailable == true).ToList();


            // Bind properties to the dropdown list
            var viewModel = new AppointmentVM
            {
                DoctorLists = availDoctors.Select(doc => new SelectListItem
                {
                    Text = doc.DoctorName,
                    Value = doc.Id.ToString()
                }).ToList(),
                TimeSlotLists = timeslotList.Select(ts => new SelectListItem
                {
                    Text = ts.Slot,
                    Value = ts.Id.ToString()
                }).ToList(),
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(AppointmentVM vm)
        {
            //if (!ModelState.IsValid) return View(vm); //commented out because it wouldnt let my code run
            //ran perfectly without it

            // Save the selected item from dropdown 
            // And textbox entry to the bill object
            var appt = new Appointment
            {
                PatientName = vm.PatientName,
                PhoneNumber = vm.PhoneNumber,
                DoctorId = (int)vm.SelectedDoctorId,
                Date = vm.Date,
                TimeSlotId = (int)vm.SelectedTimeSlotId,
                IsLate = vm.IsLate,
                NoShow = vm.NoShow,
            };

            // check if this doctor is already booked on that date at that time 
            //var availResponse = _client.GetAsync($"{_client.BaseAddress/})

            // Pull data from api for washer and set washer to not available if they are booked
            var docGetResponse = _client.GetAsync($"{_client.BaseAddress}/doctor/{appt.DoctorId}").Result;
            var docGetData = docGetResponse.Content.ReadAsStringAsync().Result;
            var selectedDoc = JsonConvert.DeserializeObject<Doctor>(docGetData);

            selectedDoc.IsAvailable = false;

            string docPutData = JsonConvert.SerializeObject(selectedDoc);
            StringContent docContent = new StringContent(docPutData, Encoding.UTF8, "application/json");
            HttpResponseMessage docPutResponse = _client.PutAsync($"{_client.BaseAddress}/washer/{appt.DoctorId}", docContent).Result;



            // Serialize the object and sends it to thru the API to the DB
            var data = JsonConvert.SerializeObject(appt);
            StringContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            var response = _client.PostAsync($"{_client.BaseAddress}/appointment", content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(vm);
            }
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            //Get appt record using Id and deserialize it in a variable
            var response = _client.GetAsync($"{_client.BaseAddress}/appointment/{Id}").Result;
            var content = response.Content.ReadAsStringAsync().Result;
            var appt = JsonConvert.DeserializeObject<Appointment>(content);

            // Pull API data for doctors
            var doctorResponse = _client.GetAsync($"{_client.BaseAddress}/doctor").Result;
            var doctorData = doctorResponse.Content.ReadAsStringAsync().Result;
            List<Doctor> docList = JsonConvert.DeserializeObject<List<Doctor>>(doctorData)!;

            // Pull API data for timeslot
            var timeslotResponse = _client.GetAsync($"{_client.BaseAddress}/timeslot").Result;
            var timeslotData = timeslotResponse.Content.ReadAsStringAsync().Result;
            List<TimeSlot> timeslotList = JsonConvert.DeserializeObject<List<TimeSlot>>(timeslotData)!;

            // To prevent issues, if a washer is not avail then 
            // they will not be displayed in the dropdown list
            var doctors = docList.AsQueryable();
            var availDoctors = doctors.Where(s => s.IsAvailable == true).ToList();

            var viewModel = new AppointmentVM
            {
                PatientName = appt.PatientName,
                PhoneNumber = appt.PhoneNumber,
                SelectedDoctorId = (int)appt.DoctorId,
                Date = appt.Date,
                SelectedTimeSlotId = (int)appt.TimeSlotId,
                IsLate = appt.IsLate,
                NoShow = appt.NoShow,

                DoctorLists = availDoctors.Select(doc => new SelectListItem
                {
                    Text = doc.DoctorName,
                    Value = doc.Id.ToString()
                }).ToList(),
                TimeSlotLists = timeslotList.Select(ts => new SelectListItem
                {
                    Text = ts.Slot,
                    Value = ts.Id.ToString()
                }).ToList(),
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(AppointmentVM vm)
        {
            //if (!ModelState.IsValid) return View(vm);

            var appt = new Appointment
            {
                Id = vm.Id,
                PatientName = vm.PatientName,
                PhoneNumber = vm.PhoneNumber,
                DoctorId = (int)vm.SelectedDoctorId,
                Date = vm.Date,
                TimeSlotId = (int)vm.SelectedTimeSlotId,
                IsLate = vm.IsLate,
                NoShow = vm.NoShow,
            };

            var data = JsonConvert.SerializeObject(appt);
            StringContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            var response = _client.PutAsync($"{_client.BaseAddress}/appointment/{appt.Id}", content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(vm);
            }
        }

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            try
            {
                Appointment appt = new Appointment();

                HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/appointment/{Id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    appt = JsonConvert.DeserializeObject<Appointment>(data);
                }
                return View(appt);
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int Id)
        {
            try
            {
                HttpResponseMessage response = _client.DeleteAsync($"{_client.BaseAddress}/appointment/{Id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                return View();
            }

            return View();
        }

        [HttpPost]
        public IActionResult MarkLate(int apptId)
        {
            // Get the appointemnt by its ID
            var aResponse = _client.GetAsync($"{_client.BaseAddress}/appointment/{apptId}").Result;
            var aData = aResponse.Content.ReadAsStringAsync().Result;
            var appt = JsonConvert.DeserializeObject<Appointment>(aData);

            // Mark the appointemnt as complete
            appt.IsLate = true;

            // Update the appointemnt in the API
            var aData2 = JsonConvert.SerializeObject(appt);
            StringContent aContent = new StringContent(aData2, System.Text.Encoding.UTF8, "application/json");
            var aResponse2 = _client.PutAsync($"{_client.BaseAddress}/appointment/{apptId}", aContent).Result;

            if (!aResponse2.IsSuccessStatusCode)
            {
                return View();
            }

            return RedirectToAction("Index"); // Redirect to the appointment list
        }

        [HttpPost]
        public IActionResult MarkNoShow(int apptId)
        {
            // Get the appointemnt by its ID
            var aResponse = _client.GetAsync($"{_client.BaseAddress}/appointment/{apptId}").Result;
            var aData = aResponse.Content.ReadAsStringAsync().Result;
            var appt = JsonConvert.DeserializeObject<Appointment>(aData);

            // Mark the appointemnt as complete
            appt.NoShow = true;

            // Update the appointemnt in the API
            var aData2 = JsonConvert.SerializeObject(appt);
            StringContent aContent = new StringContent(aData2, System.Text.Encoding.UTF8, "application/json");
            var aResponse2 = _client.PutAsync($"{_client.BaseAddress}/appointment/{apptId}", aContent).Result;

            if (!aResponse2.IsSuccessStatusCode)
            {
                return View();
            }

            return RedirectToAction("Index"); // Redirect to the appointment list
        }
    }
}
