using System.ComponentModel.DataAnnotations;

namespace RestAPI.Models.DTOs;

public class CreateProyectoUserDTO 
{
    
    public string Nombre { get; set; }

    
    public string Descripcion { get; set; }

    public string Tipo { get; set; }

    public string IdAlumno { get; set; }

    public string? IdProfesor { get; set; }

    
    public String Estado { get; set; }
}
