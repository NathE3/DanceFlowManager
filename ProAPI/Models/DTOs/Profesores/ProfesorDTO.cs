using RestAPI.Models.DTOs.Clases;

namespace RestAPI.Models.DTOs.Profesores
{
    public class ProfesorDTO
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Apellido { get; set; }

        public string Email { get; set; }

        public int? Telefono { get; set; }

        public string Estado { get; set; }

        public ICollection<ClaseDTO> ClasesDirigidas { get; set; }
            = new List<ClaseDTO>();

    }
}
