using RestAPI.Models.DTOs.Alumnos;
using RestAPI.Models.DTOs.ProfesorDTO;
using RestAPI.Models.DTOs.Profesores;
using RestAPI.Models.Entity;

namespace RestAPI.Repository.IRepository
{
    public interface IProfesorRepository
    {
        ICollection<AlumnoEntity> GetProfesores();
        AppUser GetProfesor(string id);
        bool IsUniqueProfesor(string userName);
        Task<ProfesorLoginResponseDTO> Login(ProfesorLoginDTO profesorLoginDTO);
        Task<ProfesorLoginResponseDTO> Register(ProfesorRegistrationDTO profesorRegistrationDTO);
    }
}

