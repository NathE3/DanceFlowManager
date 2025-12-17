using RestAPI.Models.DTOs.Alumnos;
using RestAPI.Models.DTOs.Clases;
using RestAPI.Models.Entity;

namespace RestAPI.Repository.IRepository
{
    public interface IClasesRepository 
    {
        Task<List<ClaseDTO>> GetAllAsync();
        Task<List<ClaseDTO>> GetAllFromProfesorAsync(string id);
        Task<ClaseDTO> GetClaseAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<bool> CreateAsync(CreateClaseDTO dto);
        Task<bool> UpdateAsync(ClaseDTO dto);
        Task<bool> DeleteAsync(Guid id);

        Task<bool> AnadirAlumno(Guid claseId, string idAlumno);
        Task<bool> EliminarAlumno(Guid idClase, string idAlumno);

        Task<List<AlumnoDTO>> GetAlumnosDeClase(Guid claseId);

        Task<List<ClaseDTO>> GetClasesDeAlumno(string alumnoId);

        Task<bool> Save();
    }
}
