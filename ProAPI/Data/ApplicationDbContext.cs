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

            modelBuilder.Entity<ClaseEntity>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<ClaseEntity>()
                .HasOne(c => c.Profesor)
                .WithMany(p => p.ClasesCreadas)
                .HasForeignKey(c => c.IdProfesor)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClaseEntity>()
                .HasMany(c => c.AlumnosInscritos)
                .WithMany(a => a.ClasesInscritas)
                .UsingEntity<Dictionary<string, object>>(
                    "AlumnoClases",
                    j => j
                        .HasOne<AlumnoEntity>()
                        .WithMany()
                        .HasForeignKey("AlumnosInscritosId")
                        .OnDelete(DeleteBehavior.Cascade),

                    j => j
                        .HasOne<ClaseEntity>()
                        .WithMany()
                        .HasForeignKey("ClasesInscritasId")
                        .OnDelete(DeleteBehavior.NoAction)
                );
        }
        public DbSet<ProfesorEntity> Profesores { get; set; }
        public DbSet<AlumnoEntity> Alumnos { get; set; }
        public DbSet<ClaseEntity> Clases { get; set; }
    }
}
