using RestAPI.Models.DTOs.Clases;
using RestAPI.Models.Entity;

namespace RestAPI.Repository.IRepository
{
    public interface IClasesRepository : IRepository<ClaseEntity>
    {
        Task CreateAsync(CreateClaseDTO entity);
        Task<List<ClaseDTO>> GetAllFromProfesorAsync(string id);
    }
}
