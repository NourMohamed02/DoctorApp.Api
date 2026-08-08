using System;

namespace DoctorApp.Context.entities
{
    public class Appointment
    {
        public int Id { get; set; }

        public int DoctorId { get; set; }

        public doctor Doctor { get; set; }

        public int PatientId { get; set; }

        public patient Patient { get; set; }

        public DateTime AppointmentDate { get; set; }

        public string Status { get; set; }
    }
}