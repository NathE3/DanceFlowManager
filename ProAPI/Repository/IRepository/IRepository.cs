using RestAPI.Models.Entity;

namespace RestAPI.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<ICollection<TEntity>> GetAllAsync();
        Task<TEntity> GetAsync(string id);
        Task<bool> ExistsAsync(string id);
        Task<bool> CreateAsync(TEntity category);
        Task<bool> UpdateAsync(TEntity category);
        Task<bool> DeleteAsync(string id);
        Task<bool> Save();
        void ClearCache();
    }
}


