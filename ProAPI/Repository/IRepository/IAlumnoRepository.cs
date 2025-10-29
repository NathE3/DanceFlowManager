using RestAPI.Models.DTOs.Alumnos;
using RestAPI.Models.Entity;

namespace RestAPI.Repository.IRepository
{
    public interface IAlumnoRepository
    {
        ICollection<AlumnoEntity> GetAlumnos();
        AppUser GetUser(string id);
        bool IsUniqueUser(string userName);
        Task<AlumnoLoginResponseDTO> Login(AlumnoLoginDTO alumnoLoginDTO);
        Task<AlumnoLoginResponseDTO> Register(AlumnoRegistrationDTO alumnoRegistrationDTO);
    }
}
