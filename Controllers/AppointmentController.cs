using ClinicAppointmentAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicAppointmentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly AppDbContext _context;
        public AppointmentController(AppDbContext context) => _context = context;

        [HttpGet]
        public IActionResult GetAllAppointments()
        {
            var item = _context.Appointments
            .Include(b => b.Doctor)
            .Include(b => b.TimeSlot)
            .ToList();

            if (item == null)
            {
                return BadRequest();
            }
            return Ok(item);
        }

        [HttpGet("{Id}")]
        public IActionResult GetAppointmentById(int Id)
        {

            var item = _context.Appointments
           .Include(b => b.Doctor)
           .Include(b => b.TimeSlot)
           .FirstOrDefault(x => x.Id == Id);

            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public IActionResult CreateAppointment([FromBody] Appointment values)
        {
            _context.Appointments.Add(values);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetAppointmentById), new { Id = values.Id }, values);
        }

        [HttpPut("{Id}")]
        public IActionResult UpdateAppointment(int Id, [FromBody] Appointment values)
        {
            if (Id != values.Id)
            {
                return NotFound();
            }

            _context.Appointments.Update(values);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetAppointmentById), new { Id = values.Id }, values);
        }

        [HttpDelete("{Id}")]
        public IActionResult DeleteAppointment(int Id)
        {
            var item = _context.Appointments.FirstOrDefault(x => x.Id == Id);
            if (item == null)
            {
                return BadRequest();
            }

            _context.Appointments.Remove(item);
            _context.SaveChanges();

            return Ok(item);
        }
    }
}
