using DoctorApp.Api.RequestBody;
using DoctorApp.Context.entities;
using DoctorApp.DB.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoctorApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly DoctorAppDbContext _context;

        public DoctorController(DoctorAppDbContext context)
        {
            _context = context;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterDoctor([FromBody] RegisterDoctorRequestBody requestBody)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Create User
            var newUser = new users()
            {
                name = requestBody.name,
                email = requestBody.email,
                password = requestBody.password
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            // Create Doctor
            var newDoctor = new doctor()
            {
                UserId = newUser.Id
            };

            _context.Doctors.Add(newDoctor);
            await _context.SaveChangesAsync();

            // Link User to Doctor
            newUser.DoctorId = newDoctor.Id;

            _context.Users.Update(newUser);
            await _context.SaveChangesAsync();

            return Ok("Doctor Registered Successfully");
        }
        [HttpGet("BySpecialization/{specializationId}")]
        public async Task<IActionResult> GetDoctorsBySpecialization(int specializationId)
        {
            var doctors = await _context.DoctorSpecializations
                .Where(ds => ds.SpecializationId == specializationId)
                .Include(ds => ds.Doctor)
                .ThenInclude(d => d.users)
                .Select(ds => new
                {
                    DoctorId = ds.Doctor.Id,
                    Name = ds.Doctor.users.name,
                    HourRate = ds.Doctor.HourRate
                })
                .ToListAsync();

            return Ok(doctors);
        }

    }
}