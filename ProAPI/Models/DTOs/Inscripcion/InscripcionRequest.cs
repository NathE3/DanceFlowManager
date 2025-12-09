using RestAPI.Models.DTOs.Alumnos;

namespace RestAPI.Models.DTOs.Inscripcion
{
    public class InscripcionRequest
    {
        public Guid Id { get; set; }         
        public AlumnoDTO Alumno { get; set; } 
    }

}
