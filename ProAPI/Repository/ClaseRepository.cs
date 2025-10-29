using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RestAPI.Data;
using RestAPI.Models.Entity;
using RestAPI.Repository.IRepository;

namespace RestAPI.Repository
{
    public class ClaseRepository : IClasesRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly string ClaseEntityCacheKey = "ClaseEntityCacheKey";
        private readonly int CacheExpirationTime = 3600;
        public ClaseRepository(ApplicationDbContext context, IMemoryCache cache)
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
            _cache.Remove(ClaseEntityCacheKey);
        }

        public async Task<ICollection<ClaseEntity>> GetAllAsync()
        {
            if (_cache.TryGetValue(ClaseEntityCacheKey, out ICollection<ClaseEntity> ClaseCached))
                return ClaseCached;

            var ClaseFromDb = await _context.Clases.OrderBy(c => c.Nombre).ToListAsync();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                  .SetAbsoluteExpiration(TimeSpan.FromSeconds(CacheExpirationTime));

            _cache.Set(ClaseEntityCacheKey, ClaseFromDb, cacheEntryOptions);
            return ClaseFromDb;
        }

        public async Task<ICollection<ClaseEntity>> GetAllFromUserAsync(string id)
        {
            var ClaseFromDb = await _context.Clases.OrderBy(c => c.Nombre).Where(e => e.IdProfesor == id).ToListAsync();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                  .SetAbsoluteExpiration(TimeSpan.FromSeconds(CacheExpirationTime));

            _cache.Set(ClaseEntityCacheKey, ClaseFromDb, cacheEntryOptions);
            return ClaseFromDb;
        }

        public async Task<ClaseEntity> GetAsync(int id)
        {
            if (_cache.TryGetValue(ClaseEntityCacheKey, out ICollection<ClaseEntity> ProyectosCached))
            {
                var ProyectoEntity = ProyectosCached.FirstOrDefault(c => c.Id == id);
                if (ProyectoEntity != null)
                    return ProyectoEntity;
            }

            return await _context.Clases.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Clases.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> CreateAsync(ClaseEntity ClaseEntity)
        {
            ClaseEntity.CreatedDate = DateTime.Now;
            _context.Clases.Add(ClaseEntity);
            return await Save();
        }

        public async Task<bool> UpdateAsync(ClaseEntity ClaseEntity)
        {
            ClaseEntity.CreatedDate = DateTime.Now;
            _context.Update(ClaseEntity);
            return await Save();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var ProyectoEntity = await GetAsync(id);
            if (ProyectoEntity == null)
                return false;

            _context.Clases.Remove(ProyectoEntity);
            return await Save();
        }
    }
}
