using ClinicAppointmentAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicAppointmentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeSlotController : ControllerBase
    {
        private readonly AppDbContext _context;
        public TimeSlotController(AppDbContext context) => _context = context;

        [HttpGet]
        public IActionResult GetAllTimeSlots()
        {
            var item = _context.TimeSlots.ToList();

            if (item == null)
            {
                return BadRequest();
            }
            return Ok(item);
        }

        [HttpGet("{Id}")]
        public IActionResult GetTimeSlotById(int Id)
        {
            var item = _context.TimeSlots.FirstOrDefault(x => x.Id == Id);

            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public IActionResult CreateTimeSlot([FromBody] TimeSlot values)
        {
            _context.TimeSlots.Add(values);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetTimeSlotById), new { Id = values.Id }, values);
        }

        [HttpPut("{Id}")]
        public IActionResult UpdateTimeSlot(int Id, [FromBody] TimeSlot values)
        {
            if (Id != values.Id)
            {
                return NotFound();
            }

            _context.TimeSlots.Update(values);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetTimeSlotById), new { Id = values.Id }, values);
        }

        [HttpDelete("{Id}")]
        public IActionResult DeleteTimeSlot(int Id)
        {
            var item = _context.TimeSlots.FirstOrDefault(x => x.Id == Id);
            if (item == null)
            {
                return BadRequest();
            }

            _context.TimeSlots.Remove(item);
            _context.SaveChanges();

            return Ok(item);
        }
    }
}
