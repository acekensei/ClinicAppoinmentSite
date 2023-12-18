using System.ComponentModel.DataAnnotations;

namespace ClinicAppointmentAPI.Models
{
    public class TimeSlot
    {
        [Key]
        public int Id { get; set; }
        public string Slot { get; set; }
    }
}
