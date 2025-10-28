using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RestAPI.Data;
using RestAPI.Models.Entity;
using RestAPI.Repository.IRepository;

namespace RestAPI.Repository
{
    public class ProyectoRepository : IProyectoRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly string ProyectoEntityCacheKey = "ProyectoEntityCacheKey"; //cambiadmelo lokos
        private readonly int CacheExpirationTime = 3600;
        public ProyectoRepository(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<bool> Save()
        {
            var result = await _context.SaveChangesAsync() >= 0;
            if (result)
            {
                ClearCache();
            }
            return result;
        }

        public void ClearCache()
        {
            _cache.Remove(ProyectoEntityCacheKey);
        }

        public async Task<ICollection<ProyectoEntity>> GetAllAsync()
        {
            if (_cache.TryGetValue(ProyectoEntityCacheKey, out ICollection<ProyectoEntity> ProyectosCached))
                return ProyectosCached;

            var ProyectosFromDb = await _context.Proyectos.OrderBy(c => c.Nombre).ToListAsync();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                  .SetAbsoluteExpiration(TimeSpan.FromSeconds(CacheExpirationTime));

            _cache.Set(ProyectoEntityCacheKey, ProyectosFromDb, cacheEntryOptions);
            return ProyectosFromDb;
        }

        public async Task<ICollection<ProyectoEntity>> GetAllFromUserAsync(string id)
        {
            /*if (_cache.TryGetValue(ProyectoEntityCacheKey, out ICollection<ProyectoEntity> ProyectosCached))
                return ProyectosCached;*/

            var ProyectosFromDb = await _context.Proyectos.OrderBy(c => c.Nombre).Where(e=>e.IdAlumno==id).ToListAsync();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                  .SetAbsoluteExpiration(TimeSpan.FromSeconds(CacheExpirationTime));

            _cache.Set(ProyectoEntityCacheKey, ProyectosFromDb, cacheEntryOptions);
            return ProyectosFromDb;
        }

        public async Task<ProyectoEntity> GetAsync(int id)
        {
            if (_cache.TryGetValue(ProyectoEntityCacheKey, out ICollection<ProyectoEntity> ProyectosCached))
            {
                var ProyectoEntity = ProyectosCached.FirstOrDefault(c => c.Id == id);
                if (ProyectoEntity != null)
                    return ProyectoEntity;
            }

            return await _context.Proyectos.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Proyectos.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> CreateAsync(ProyectoEntity ProyectoEntity)
        {
            ProyectoEntity.CreatedDate = DateTime.Now;
            _context.Proyectos.Add(ProyectoEntity);
            return await Save();
        }

        public async Task<bool> UpdateAsync(ProyectoEntity ProyectoEntity)
        {
            ProyectoEntity.CreatedDate = DateTime.Now;
            _context.Update(ProyectoEntity);
            return await Save();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var ProyectoEntity = await GetAsync(id);
            if (ProyectoEntity == null)
                return false;

            _context.Proyectos.Remove(ProyectoEntity);
            return await Save();
        }
    }
}
