using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RestAPI.Data;
using RestAPI.Models.DTOs.Alumnos;
using RestAPI.Models.DTOs.Clases;
using RestAPI.Models.Entity;
using RestAPI.Repository.IRepository;

namespace RestAPI.Repository
{
    public class ClaseRepository : IClasesRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly IMapper _mapper;

        private const string ClaseEntityCacheKey = "ClaseEntityCacheKey";
        private const int CacheExpirationTime = 3600;

        public ClaseRepository(ApplicationDbContext context, IMemoryCache cache, IMapper mapper)
        {
            _context = context;
            _cache = cache;
            _mapper = mapper;
        }

        public async Task<bool> Save()
        {
            var result = await _context.SaveChangesAsync() >= 0;
            if (result) _cache.Remove(ClaseEntityCacheKey);
            return result;
        }

        public async Task<List<ClaseDTO>> GetAllAsync()
        {
            if (_cache.TryGetValue(ClaseEntityCacheKey, out List<ClaseEntity> cache))
                return _mapper.Map<List<ClaseDTO>>(cache);

            var data = await _context.Clases.OrderBy(c => c.Nombre).ToListAsync();
            var mapped = _mapper.Map<List<ClaseDTO>>(data);

            _cache.Set(ClaseEntityCacheKey, mapped,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(CacheExpirationTime)));

            return mapped;
        }

        public async Task<List<ClaseDTO>> GetAllFromProfesorAsync(string id)
        {
            var data = await _context.Clases
                .Where(e => e.IdProfesor == id)
                .OrderBy(c => c.Nombre)
                .ToListAsync();

            var mapped = _mapper.Map<List<ClaseDTO>>(data);

            _cache.Set(ClaseEntityCacheKey, mapped,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(CacheExpirationTime)));

            return mapped;
        }

        public async Task<ClaseDTO> GetClaseAsync(Guid id)
        {
            if (_cache.TryGetValue(ClaseEntityCacheKey, out List<ClaseDTO> cache))
            {
                var found = cache.FirstOrDefault(c => c.Id == id);
                if (found != null) return found;
            }

            var entity = await _context.Clases.FirstOrDefaultAsync(c => c.Id == id);
            return _mapper.Map<ClaseDTO>(entity);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Clases.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> CreateAsync(CreateClaseDTO dto)
        {
            var entity = _mapper.Map<ClaseEntity>(dto);
            _context.Clases.Add(entity);
            return await Save();
        }

        public async Task<bool> UpdateAsync(ClaseDTO dto)
        {
            var entity = _mapper.Map<ClaseEntity>(dto);
            _context.Update(entity);
            return await Save();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var dto = await GetClaseAsync(id);
            if (dto == null) return false;

            var entity = _mapper.Map<ClaseEntity>(dto);
            _context.Clases.Remove(entity);
            return await Save();
        }

        public async Task<bool> AnadirAlumno(Guid claseId, string idAlumno)
        {
            var clase = await _context.Clases
                .Include(c => c.AlumnosInscritos)
                .FirstOrDefaultAsync(c => c.Id == claseId);

            if (clase == null) return false;

            var alumno = await _context.Alumnos.FirstOrDefaultAsync(a => a.Id == idAlumno);
            if (alumno == null) return false;

            var exists = clase.AlumnosInscritos.Any(a => a.Id == idAlumno);
            if (exists) return false;

            clase.AlumnosInscritos.Add(alumno);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> EliminarAlumno(Guid idClase, string idAlumno)
        {
            var clase = await _context.Clases
                .Include(c => c.AlumnosInscritos)
                .FirstOrDefaultAsync(c => c.Id == idClase);

            if (clase == null) return false;

            var alumno = clase.AlumnosInscritos.FirstOrDefault(a => a.Id == idAlumno);
            if (alumno == null) return false;

            clase.AlumnosInscritos.Remove(alumno);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<AlumnoDTO>> GetAlumnosDeClase(Guid claseId)
        {
            var clase = await _context.Clases
                .Include(c => c.AlumnosInscritos)
                .FirstOrDefaultAsync(c => c.Id == claseId);

            if (clase == null) return new List<AlumnoDTO>();

            return _mapper.Map<List<AlumnoDTO>>(clase.AlumnosInscritos);
        }

        public async Task<List<ClaseDTO>> GetClasesDeAlumno(string alumnoId)
        {
            var clases = await _context.Clases
                .Include(c => c.AlumnosInscritos)
                .Where(c => c.AlumnosInscritos.Any(a => a.Id == alumnoId))
                .ToListAsync();

            return _mapper.Map<List<ClaseDTO>>(clases);
        }

    }
}
