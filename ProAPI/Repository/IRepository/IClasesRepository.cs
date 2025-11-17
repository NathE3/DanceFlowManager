using RestAPI.Models.DTOs.Alumnos;
using RestAPI.Models.DTOs.Clases;
using RestAPI.Models.Entity;

namespace RestAPI.Repository.IRepository
{
    public interface IClasesRepository 
    {
        Task<bool> Save();
        void ClearCache();
        Task<List<ClaseDTO>> GetAllAsync();
        Task<List<ClaseDTO>> GetAllFromProfesorAsync(string id);
        Task<ClaseDTO> GetClaseAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<bool> CreateAsync(CreateClaseDTO claseDTO);
        Task<bool> UpdateAsync(ClaseDTO claseDTO);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> EliminarAlumno(Guid Id, string AlumnoDTO);
        Task<bool> AnadirAlumno(Guid Id, AlumnoDTO AlumnoDTO);
    }
}
