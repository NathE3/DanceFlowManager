using RestAPI.Models.DTOs.Alumnos;
using System.Text.Json.Serialization;


namespace RestAPI.Models.DTOs.Clases;

public class ClaseDTO : CreateClaseDTO
{
    public Guid Id { get; set; }

    [JsonIgnore]
    public ICollection<AlumnoDTO> AlumnosInscritos { get; set; }
              = new List<AlumnoDTO>();


}
