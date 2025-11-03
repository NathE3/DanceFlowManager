using RestAPI.Models.DTOs.Alumnos;
using RestAPI.Models.DTOs.Profesores;
using RestAPI.Models.Entity;

namespace RestAPI.Repository.IRepository
{
    public interface IProfesorRepository
    {
        Task <List<ProfesorDTO?>> GetProfesores();
        Task<ProfesorDTO?> GetProfesor(string id);

        Task<ProfesorEntity?> UpdateProfesor(string id, ProfesorEntity profesor);

        Task<bool> DeleteProfesor(string username);


    }
}

