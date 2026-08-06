using DoctorApp.Api.RequestBody;
using DoctorApp.Context.entities;
using DoctorApp.DB.Context;
using Microsoft.AspNetCore.Mvc;

namespace DoctorApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly DoctorAppDbContext _context;

        public PatientController(DoctorAppDbContext context)
        {
            _context = context;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterPatient([FromBody] RegisterPatientRequestBody requestBody)
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

            // Create Patient
            var newPatient = new patient()
            {
                UserId = newUser.Id
            };

            _context.Patients.Add(newPatient);
            await _context.SaveChangesAsync();

            // Link User to Patient
            newUser.PatientId = newPatient.Id;

            _context.Users.Update(newUser);
            await _context.SaveChangesAsync();

            return Ok("Patient Registered Successfully");
        }
    }
}