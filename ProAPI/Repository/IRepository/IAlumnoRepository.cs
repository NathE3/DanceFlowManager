using RestAPI.Models.DTOs.Alumnos;
using RestAPI.Models.DTOs.Login;
using RestAPI.Models.Entity;

namespace RestAPI.Repository.IRepository
{
    public interface IAlumnoRepository
    {
        Task<ICollection<AlumnoEntity>> GetAlumnos();
        Task<AlumnoEntity?> GetById(string id);

        Task<AlumnoEntity?> UpdateAsync(string id, AlumnoEntity alumno);

        Task<bool> DeleteAsync(string username);
    }
}
