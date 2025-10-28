using System.Text.Json.Serialization;


namespace InfoManager.Models
{
    public class ProyectoDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; }

        [JsonPropertyName("tipo")]
        public string Tipo { get; set; }

        [JsonPropertyName("idAlumno")]
        public string IdAlumno { get; set; }

        [JsonPropertyName("idProfesor")]
        public string IdProfesor { get; set; }

        [JsonPropertyName("estado")]
        public string Estado { get; set; }
        [JsonPropertyName("createdDate")]
        public string CreatedDate { get; set; }

    }
    }

  
  





