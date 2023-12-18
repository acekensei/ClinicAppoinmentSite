using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ClinicAppointmentClient.Models.ViewModels
{
    public class AppointmentVM
    {
        public IQueryable<Appointment> Data { get; set; }

        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string term { get; set; }


        public int Id { get; set; }
        public string PatientName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Date { get; set; }
        public bool IsLate { get; set; }
        public bool NoShow { get; set; }

        // properties for dropdownlists
        public List<SelectListItem> DoctorLists { get; set; }
        public List<SelectListItem> TimeSlotLists { get; set; }

        // properties for selected 
        public int SelectedDoctorId { get; set; }
        public int SelectedTimeSlotId { get; set; }
    }
}
