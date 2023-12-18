using System.ComponentModel.DataAnnotations;

namespace ClinicAppointmentAPI.Models
{
    public class Doctor
    {
        [Key]
        public int Id { get; set; }
        public string DoctorName { get; set; }
        public bool IsAvailable { get; set; }
    }
}
