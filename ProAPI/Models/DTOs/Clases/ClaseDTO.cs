using RestAPI.Models.DTOs.Alumnos;


namespace RestAPI.Models.DTOs.Clases;

public class ClaseDTO : CreateClaseDTO
{
    public Guid Id { get; set; }
    public ICollection<AlumnoDTO> AlumnosInscritos { get; set; }
              = new List<AlumnoDTO>();


}
