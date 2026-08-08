using DoctorApp.Api.RequestBody;
using DoctorApp.Context.entities;
using DoctorApp.DB.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoctorApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly DoctorAppDbContext _context;

        public AppointmentController(DoctorAppDbContext context)
        {
            _context = context;
        }

        [HttpPost("Book")]
        public async Task<IActionResult> BookAppointment(
            [FromBody] BookAppointmentRequestBody requestBody)
        {
            // Check if doctor exists
            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.Id == requestBody.DoctorId);

            if (doctor == null)
            {
                return NotFound("Doctor not found");
            }

            // Check if patient exists
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.Id == requestBody.PatientId);

            if (patient == null)
            {
                return NotFound("Patient not found");
            }

            // Create appointment
            var appointment = new Appointment()
            {
                DoctorId = requestBody.DoctorId,
                PatientId = requestBody.PatientId,
                AppointmentDate = requestBody.AppointmentDate,
                Status = "Pending"
            };

            // Add appointment to database
            _context.Appointments.Add(appointment);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Appointment booked successfully",
                AppointmentId = appointment.Id,
                DoctorId = appointment.DoctorId,
                PatientId = appointment.PatientId,
                AppointmentDate = appointment.AppointmentDate,
                Status = appointment.Status
            });
        }
        [HttpGet("Available/{doctorId}")]
        public async Task<IActionResult> GetAvailableSlots(
      int doctorId,
      [FromQuery] DateTime date)
        {
            // 1. Check that the doctor exists
            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.Id == doctorId);

            if (doctor == null)
            {
                return NotFound("Doctor not found");
            }

            // 2. Get the selected date only
            var selectedDate = date.Date;

            // 3. Get appointments for this doctor on this date
            var bookedSlots = await _context.Appointments
                .Where(a =>
                    a.DoctorId == doctorId &&
                    a.AppointmentDate.Date == selectedDate &&
                    a.Status != "Cancelled")
                .Select(a => a.AppointmentDate)
                .ToListAsync();

            // 4. Create the doctor's available working hours
            var allSlots = new List<DateTime>
    {
        selectedDate.AddHours(10),
        selectedDate.AddHours(11),
        selectedDate.AddHours(12),
        selectedDate.AddHours(13),
        selectedDate.AddHours(14),
        selectedDate.AddHours(15)
    };

            // 5. Remove booked slots
            var availableSlots = allSlots
                .Where(slot => !bookedSlots.Any(booked =>
                    booked == slot))
                .ToList();

            // 6. Return the available slots
            return Ok(availableSlots);
        }
        [HttpGet("Patient/{patientId}")]
        public async Task<IActionResult> GetPatientAppointments(int patientId)
        {
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.Id == patientId);

            if (patient == null)
            {
                return NotFound("Patient not found");
            }

            var appointments = await _context.Appointments
                .Where(a => a.PatientId == patientId)
                .Include(a => a.Doctor)
                    .ThenInclude(d => d.users)
                .Select(a => new
                {
                    AppointmentId = a.Id,
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor.users.name,
                    AppointmentDate = a.AppointmentDate,
                    Status = a.Status
                })
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();

            return Ok(appointments);
        }
        [HttpPut("Cancel/{appointmentId}")]
        public async Task<IActionResult> CancelAppointment(int appointmentId)
        {
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == appointmentId);

            if (appointment == null)
            {
                return NotFound("Appointment not found");
            }

            if (appointment.Status == "Cancelled")
            {
                return BadRequest("Appointment is already cancelled");
            }

            if (appointment.Status == "Completed")
            {
                return BadRequest("Completed appointment cannot be cancelled");
            }

            appointment.Status = "Cancelled";

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Appointment cancelled successfully",
                appointmentId = appointment.Id,
                status = appointment.Status
            });
        }
        [HttpGet("Doctor/{doctorId}")]
        public async Task<IActionResult> GetDoctorAppointments(int doctorId)
        {
            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.Id == doctorId);

            if (doctor == null)
            {
                return NotFound("Doctor not found");
            }

            var appointments = await _context.Appointments
                .Where(a => a.DoctorId == doctorId)
                .Include(a => a.Patient)
                    .ThenInclude(p => p.users)
                .Select(a => new
                {
                    AppointmentId = a.Id,
                    PatientId = a.PatientId,
                    PatientName = a.Patient.users.name,
                    PatientEmail = a.Patient.users.email,
                    AppointmentDate = a.AppointmentDate,
                    Status = a.Status
                })
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();

            return Ok(appointments);
        }
        [HttpPut("Confirm/{appointmentId}")]
        public async Task<IActionResult> ConfirmAppointment(int appointmentId)
        {
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == appointmentId);

            if (appointment == null)
            {
                return NotFound("Appointment not found");
            }

            if (appointment.Status == "Cancelled")
            {
                return BadRequest("Cancelled appointment cannot be confirmed");
            }

            if (appointment.Status == "Completed")
            {
                return BadRequest("Completed appointment cannot be confirmed");
            }

            appointment.Status = "Confirmed";

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Appointment confirmed successfully",
                appointmentId = appointment.Id,
                status = appointment.Status
            });
        }
    }
}