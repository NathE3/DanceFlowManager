using RestAPI.Models.DTOs.Alumnos;
using RestAPI.Models.DTOs.Login;
using RestAPI.Models.Entity;

namespace RestAPI.Repository.IRepository
{
    public interface IAlumnoRepository
    {
        Task<List<AlumnoDTO>> GetAlumnos();
        Task<AlumnoDTO?> GetById(string id);

        Task<AlumnoEntity?> UpdateAsync(string id, AlumnoEntity alumno);

        Task<bool> DeleteAsync(string username);
    }
}
