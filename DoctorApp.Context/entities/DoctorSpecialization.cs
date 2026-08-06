using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorApp.Context.entities
{
    public class DoctorSpecialization
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }

        [ForeignKey(nameof(DoctorId))]
        public doctor Doctor { get; set; }

        public int SpecializationId { get; set; }

        [ForeignKey(nameof(SpecializationId))]
        public Specialization Specialization { get; set; }
    }
}