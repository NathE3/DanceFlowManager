using RestAPI.Models.DTOs;
using RestAPI.Models.Entity;

namespace RestAPI.Models.DTOs.UserDto
{
    public class UserDto
    {
       public string Id { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Email { get; set; }
        public string Rol { get; set; }
        public int Telefono { get; set; }
        public ICollection<ProyectoDTO> ProyectosProfesor { get; set; }
        public ICollection<ProyectoDTO> ProyectosAlumno { get; set; }



    }
}
