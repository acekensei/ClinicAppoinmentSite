using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicAppointmentClient.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public string PhoneNumber { get; set; }
        public int? DoctorId { get; set; }
        public DateTime Date { get; set; }
        public int? TimeSlotId { get; set; }
        public bool IsLate { get; set; }
        public bool NoShow { get; set; }

        // Navigation
        public Doctor Doctor { get; set; }
        public TimeSlot TimeSlot { get; set; }
    }
}
