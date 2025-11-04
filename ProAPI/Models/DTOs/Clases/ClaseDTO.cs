using RestAPI.Models.DTOs.Alumnos;


namespace RestAPI.Models.DTOs.Clases;

public class ClaseDTO : CreateClaseDTO
{
    public Guid Id_clase { get; set; } = Guid.NewGuid();
    public ICollection<AlumnoDTO> AlumnosInscritos { get; set; }
              = new List<AlumnoDTO>();


}
