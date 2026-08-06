using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApp.Context.entities
{
    public class doctor 
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public string? liscenceNumber { get; set; }

        public Decimal? HourRate { get; set; }
        //Make Relation between  doctor and users entities
        public ICollection<DoctorSpecialization> DoctorSpecializations { get; set; }
        = new List<DoctorSpecialization>();

        [ForeignKey(nameof(UserId))]
        public users users { get; set; }



    }
}
