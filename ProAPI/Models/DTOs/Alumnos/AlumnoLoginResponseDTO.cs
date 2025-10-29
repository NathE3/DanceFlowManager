using RestAPI.Models.Entity;

namespace RestAPI.Models.DTOs.Alumnos
{
    public class AlumnoLoginResponseDTO
    {
        public AppUser User { get; set; }
        public string Token { get; set; }

    }
}
