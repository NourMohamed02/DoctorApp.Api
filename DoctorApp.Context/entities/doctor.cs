using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorApp.Context.entities
{
    public class doctor
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string? liscenceNumber { get; set; }

        public decimal? HourRate { get; set; }

        public ICollection<DoctorSpecialization> DoctorSpecializations { get; set; }
            = new List<DoctorSpecialization>();

        public ICollection<Appointment> Appointments { get; set; }
            = new List<Appointment>();

        [ForeignKey(nameof(UserId))]
        public users users { get; set; }
    }
}