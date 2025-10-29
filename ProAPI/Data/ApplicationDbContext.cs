using System.Reflection.Metadata;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestAPI.Models.Entity;

namespace RestAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ProfesorEntity -> ClasesCreadas (uno a muchos)
            modelBuilder.Entity<ClaseEntity>()
                .HasOne(c => c.Profesor)
                .WithMany(p => p.ClasesCreadas)
                .HasForeignKey(c => c.IdProfesor)
                .IsRequired();

            // AlumnoEntity -> ClasesInscritas (muchos a muchos)
            modelBuilder.Entity<ClaseEntity>()
                .HasMany(c => c.AlumnosInscritos)
                .WithMany(a => a.ClasesInscritas)
                .UsingEntity(j => j.ToTable("AlumnoClases"));
        }

        public DbSet<ProfesorEntity> Profesores { get; set; }
        public DbSet<AlumnoEntity> Alumnos { get; set; }
        public DbSet<ClaseEntity> Clases { get; set; }
    }
}
