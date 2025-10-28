using RestAPI.Models.Entity;

namespace RestAPI.Repository.IRepository
{
    public interface IProyectoRepository : IRepository<ProyectoEntity>
    {
        Task<ICollection<ProyectoEntity>> GetAllFromUserAsync(string id);
    }
}
