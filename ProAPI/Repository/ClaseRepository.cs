using AutoMapper;
using Microsoft.AspNetCore.Components.Forms.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RestAPI.Data;
using RestAPI.Models.DTOs.Clases;
using RestAPI.Models.DTOs.Profesores;
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
        private readonly IMapper _mapper;

        public ClaseRepository(ApplicationDbContext context, IMemoryCache cache, IMapper mapper)
        {
            _context = context;
            _cache = cache;
            _mapper = mapper;
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

        public async Task<List<ClaseDTO>> GetAllAsync()
        {
            if (_cache.TryGetValue(ClaseEntityCacheKey, out List<ClaseEntity> ClaseCached))
                return TransForListEntityToDTO(ClaseCached);

            var ClaseFromDb = await _context.Clases.OrderBy(c => c.Nombre).ToListAsync();
            var ClasesMapped = TransForListEntityToDTO(ClaseFromDb);
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                  .SetAbsoluteExpiration(TimeSpan.FromSeconds(CacheExpirationTime));

            _cache.Set(ClaseEntityCacheKey, ClasesMapped, cacheEntryOptions);
            return ClasesMapped;
        }

        public async Task<List<ClaseDTO>> GetAllFromProfesorAsync(string id)
        {
            var ClaseFromDb = await _context.Clases.OrderBy(c => c.Nombre).Where(e => e.IdProfesor == id).ToListAsync();
            var ClasesMapped = TransForListEntityToDTO(ClaseFromDb);
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                  .SetAbsoluteExpiration(TimeSpan.FromSeconds(CacheExpirationTime));

            _cache.Set(ClaseEntityCacheKey, ClasesMapped, cacheEntryOptions);
            return ClasesMapped;
        }

        public async Task<ClaseDTO> GetAsync(string id)
        {
            if (_cache.TryGetValue(ClaseEntityCacheKey, out List<ClaseDTO> ClasesCached))
            {
                var Clases = ClasesCached.FirstOrDefault(c => c.Id_clase == id);
                if (Clases != null)
                    return Clases;
            }
            var ClaseEntity = await _context.Clases.FirstOrDefaultAsync(c => c.Id == id);
            var ClasesMapped = TransforEntitytoDTO(ClaseEntity);
            return await ClasesMapped;
        }

        public async Task<bool> ExistsAsync(string id)
        {
            return await _context.Clases.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> CreateAsync(ClaseDTO ClaseDTO)
        {
            var clase = await TransforDTOtoEntity(ClaseDTO);
            _context.Clases.Add(clase);
            return await Save();
        }

        public async Task<bool> UpdateAsync(ClaseEntity ClaseEntity)
        {
            _context.Update(ClaseEntity);
            return await Save();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var Clase = await GetAsync(id);
            var ClaseMapped = await TransforDTOtoEntity(Clase);
            if (Clase == null)
                return false;

            _context.Clases.Remove(ClaseMapped);
            return await Save();
        }

        private async Task<ClaseDTO> TransforEntitytoDTO(ClaseEntity claseEntity)
        {
            var clase = _mapper.Map<ClaseDTO>(claseEntity);
            return clase;
        }

        private List<ClaseDTO> TransForListEntityToDTO(List<ClaseEntity> claseEntity)
        {
            return _mapper.Map<List<ClaseDTO>>(claseEntity);
        }

        private async Task<ClaseEntity> TransforDTOtoEntity(ClaseDTO claseDTO)
        {
            var clase = _mapper.Map<ClaseEntity>(claseDTO);
            return clase;
        }

    }
}
