using RestAPI.Models.Entity;

namespace RestAPI.Repository.IRepository
{
    public interface IClasesRepository : IRepository<ClaseEntity>
    {
        Task<ICollection<ClaseEntity>> GetAllFromUserAsync(string id);
    }
}
