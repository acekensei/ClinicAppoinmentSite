using ClinicAppointmentAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicAppointmentAPI
{
        public class AppDbContext : DbContext
        {
            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
            {
            }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<TimeSlot> TimeSlots { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
    }
}
