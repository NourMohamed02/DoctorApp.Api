using DoctorApp.Context.entities;
using Microsoft.EntityFrameworkCore;

namespace DoctorApp.DB.Context
{
    public class DoctorAppDbContext : DbContext
    {
        public DoctorAppDbContext(DbContextOptions<DoctorAppDbContext> options)
            : base(options)
        {
        }

        public DbSet<users> Users { get; set; }

        public DbSet<doctor> Doctors { get; set; }

        public DbSet<patient> Patients { get; set; }

        public DbSet<Specialization> Specializations { get; set; }

        public DbSet<DoctorSpecialization> DoctorSpecializations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DoctorSpecialization>()
                .HasKey(ds => new { ds.DoctorId, ds.SpecializationId });
        }
    }
}