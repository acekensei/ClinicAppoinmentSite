using Microsoft.Data.SqlClient.DataClassification;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicAppointmentAPI.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }
        public string PatientName { get; set; }
        public string PhoneNumber { get; set; }
        public int? DoctorId { get; set; }
        public DateTime Date { get; set; }
        public int? TimeSlotId { get; set; }
        public bool IsLate { get; set; }
        public bool NoShow { get; set; }

        // Relationships
        [ForeignKey("DoctorId")] 
        public virtual Doctor? Doctor { get; set; }
        [ForeignKey("TimeSlotId")]
        public virtual TimeSlot? TimeSlot { get; set; }
    }
}
