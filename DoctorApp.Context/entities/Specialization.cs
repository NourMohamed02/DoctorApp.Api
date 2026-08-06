using System.Collections.Generic;

namespace DoctorApp.Context.entities
{
    public class Specialization
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<DoctorSpecialization> DoctorSpecializations { get; set; } = new List<DoctorSpecialization>();
    }
}