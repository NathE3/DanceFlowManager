using Microsoft.AspNetCore.Identity;

namespace RestAPI.Models.Entity
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public ICollection<ProyectoEntity> ProyectosProfesor { get; set; }

        public ICollection<ProyectoEntity> ProyectosAlumno { get; set; }
    }
}
