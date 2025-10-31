using RestAPI.Models.DTOs.Alumnos;
using RestAPI.Models.DTOs.Profesores;
using RestAPI.Models.Entity;

namespace RestAPI.Repository.IRepository
{
    public interface IProfesorRepository
    {
        ICollection<ProfesorEntity> GetProfesores();
        AppUser GetProfesor(string id);
       // bool IsUniqueProfesor(string userName);
    }
}

