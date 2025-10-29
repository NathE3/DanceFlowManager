using RestAPI.Models.DTOs.Alumnos;
using RestAPI.Models.DTOs.Login;
using RestAPI.Models.Entity;

namespace RestAPI.Repository.IRepository
{
    public interface IAlumnoRepository
    {
        ICollection<AlumnoEntity> GetAlumnos();
        AppUser GetUser(string id);
        bool IsUniqueUser(string userName);
      
    }
}
