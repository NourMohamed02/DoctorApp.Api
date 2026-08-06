using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorApp.Context.entities
{
    public class users
    {
        public int Id { get; set; }

        public string name { get; set; }

        public int? age { get; set; }

        public string email { get; set; }

        public string? phoneNumber { get; set; }

        public string? Address { get; set; }

        public string? gender { get; set; }

        public string password { get; set; }

        public int? DoctorId { get; set; }

        [ForeignKey(nameof(DoctorId))]
        public doctor doctor { get; set; }

        public int? PatientId { get; set; }

        [ForeignKey(nameof(PatientId))]
        public patient patient { get; set; }
    }
}