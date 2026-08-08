using System;

namespace DoctorApp.Api.RequestBody
{
    public class BookAppointmentRequestBody
    {
        public int DoctorId { get; set; }

        public int PatientId { get; set; }

        public DateTime AppointmentDate { get; set; }
    }
}