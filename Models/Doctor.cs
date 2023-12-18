using System.ComponentModel.DataAnnotations;

namespace ClinicAppointmentClient.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public string DoctorName { get; set; }
        public bool IsAvailable { get; set; }
    }
}
