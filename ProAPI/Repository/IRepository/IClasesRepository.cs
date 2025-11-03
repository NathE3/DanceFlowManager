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
        Task<ClaseDTO> GetAsync(string id);
        Task<bool> ExistsAsync(string id);
        Task<bool> CreateAsync(CreateClaseDTO claseDTO);
        Task<bool> UpdateAsync(ClaseDTO claseDTO);
        Task<bool> DeleteAsync(string id);

    }
}
