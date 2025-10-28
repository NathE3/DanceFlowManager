using System.Reflection.Metadata;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestAPI.Models.Entity;

namespace RestAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>()
            .HasMany(e => e.ProyectosProfesor)
            .WithOne(e => e.Profesor)
            .HasForeignKey(e => e.IdProfesor)
            .IsRequired(false);

            modelBuilder.Entity<AppUser>()
            .HasMany(e => e.ProyectosAlumno)
            .WithOne(e => e.Alumno)
            .HasForeignKey(e => e.IdAlumno)
            .IsRequired();
        }
        //Add models here
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<ProyectoEntity> Proyectos { get; set; }

    }
}
