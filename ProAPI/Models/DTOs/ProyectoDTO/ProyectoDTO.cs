namespace RestAPI.Models.DTOs;

public class ProyectoDTO : CreateProyectoDTO
{
    public int Id {get; set;}

    public string IdAlumno { get; set; }
    public DateTime CreatedDate {get; set;}

    //public string AlumnoNombre { get; set; }
    //public string? ProfesorNombre { get; set; }



}
