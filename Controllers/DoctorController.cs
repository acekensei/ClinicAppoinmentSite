using ClinicAppointmentAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAppointmentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly AppDbContext _context;
        public DoctorController(AppDbContext context) => _context = context;

        [HttpGet]
        public IActionResult GetAllDoctors()
        {
            var item = _context.Doctors.ToList();

            if (item == null)
            {
                return BadRequest();
            }
            return Ok(item);
        }

        [HttpGet("{Id}")]
        public IActionResult GetDoctorById(int Id)
        {
            var item = _context.Doctors.FirstOrDefault(x => x.Id == Id);

            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public IActionResult CreateDoctor([FromBody] Doctor values)
        {
            _context.Doctors.Add(values);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetDoctorById), new { Id = values.Id }, values);
        }

        [HttpPut("{Id}")]
        public IActionResult UpdateDoctor(int Id, [FromBody] Doctor values)
        {
            if (Id != values.Id)
            {
                return NotFound();
            }

            _context.Doctors.Update(values);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetDoctorById), new { Id = values.Id }, values);
        }

        [HttpDelete("{Id}")]
        public IActionResult DeleteDoctor(int Id)
        {
            var item = _context.Doctors.FirstOrDefault(x => x.Id == Id);
            if (item == null)
            {
                return BadRequest();
            }

            _context.Doctors.Remove(item);
            _context.SaveChanges();

            return Ok(item);
        }
    }
}
