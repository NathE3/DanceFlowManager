using System.Text.Json.Serialization;


namespace InfoManager.Models
{
    public class ClaseDTO
    {
        [JsonPropertyName("Id")]
        public string Id_clase { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; }

        [JsonPropertyName("tipo")]
        public string Tipo { get; set; }

        [JsonPropertyName("idProfesor")]
        public string IdProfesor { get; set; }

        [JsonPropertyName("fechaClase")]
        public DateTime FechaClase { get; set; }


        [JsonPropertyName("alumnosInscritos")]
        public List<AlumnoDTO> AlumnosInscritos { get; set; } = new();

        [System.Text.Json.Serialization.JsonIgnore] 
        public int AlumnosContados { get; set; }


    }
}

  
  





