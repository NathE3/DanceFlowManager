using RestAPI.Models.DTOs.Clases;


namespace RestAPI.Models.DTOs.Alumnos

{
    public class AlumnoDTO
    {
        public string Id_alumno { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Email { get; set; }

        public DateTime FechaAlta { get; set; }
        public DateTime? FechaBaja { get; set; }

        public int Telefono { get; set; }

        public ICollection<ClaseDTO> ClasesInscritas { get; set; }
            = new List<ClaseDTO>();
    }
}
