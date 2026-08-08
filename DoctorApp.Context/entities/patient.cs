using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApp.Context.entities
{
    public class patient 

    {
        public int Id { get; set; }

        public int UserId { get; set; }
       

        public string? BloodType { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
             = new List<Appointment>();

        [ForeignKey(nameof(UserId))]
        public users users  { get; set; }

        //Make Relation between  patient and users entities
    }
}
