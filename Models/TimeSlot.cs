using System.ComponentModel.DataAnnotations;

namespace ClinicAppointmentClient.Models
{
    public class TimeSlot
    {
        public int Id { get; set; }
        public string Slot { get; set; }
    }
}
