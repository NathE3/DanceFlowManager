using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InfoManager.Models
{
    public class AlumnoDTO
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("apellidos")]
        public string Apellidos { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("fechaAlta")]
        public DateTime FechaAlta { get; set; }

        [JsonPropertyName("fechaBaja")]
        public DateTime? FechaBaja { get; set; }

        [JsonPropertyName("telefono")]
        public int Telefono { get; set; }

        [JsonPropertyName("clasesInscritas")]
        public List<ClaseDTO> ClasesInscritas { get; set; } = new();
    }

}
