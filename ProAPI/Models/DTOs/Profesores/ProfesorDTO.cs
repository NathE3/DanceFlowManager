using RestAPI.Models.DTOs.Clases;
using RestAPI.Models.DTOs.ProfesorDTO;

namespace RestAPI.Models.DTOs.Profesores
{
    public class ProfesorDTO
    {
        public string Id_profesor { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string Email { get; set; }

        public int? Telefono { get; set; }

        public string Estado { get; set; }

        public ICollection<ClaseDTO> ClasesDirigidas { get; set; }
            = new List<ClaseDTO>();

    }
}
