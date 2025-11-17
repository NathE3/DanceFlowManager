using AutoMapper;
using Microsoft.AspNetCore.Components.Forms.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RestAPI.Data;
using RestAPI.Models.DTOs.Alumnos;
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

        public async Task<ClaseDTO> GetClaseAsync(Guid id)
        {
            if (_cache.TryGetValue(ClaseEntityCacheKey, out List<ClaseDTO> ClasesCached))
            {
                var Clases = ClasesCached.FirstOrDefault(c => c.Id == id);
                if (Clases != null)
                    return Clases;
            }
            var ClaseEntity = await _context.Clases.FirstOrDefaultAsync(c => c.Id == id);
            var ClasesMapped = TransforEntitytoDTO(ClaseEntity);
            return await ClasesMapped;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Clases.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> CreateAsync(CreateClaseDTO ClaseDTO)
        {
            var clase = await TransforCreateDTOtoDTO(ClaseDTO);
            if (clase == null) return false;
            _context.Clases.Add(clase);
            return await Save();
        }

        public async Task<bool> UpdateAsync(ClaseDTO ClaseDTO)
        {
            var clase = await MapClaseDTOtoEntity(ClaseDTO);
            _context.Update(clase);
            return await Save();
        }

        public async Task<bool> AnadirAlumno(Guid Id, AlumnoDTO AlumnoDTO)
        {
            var claseDTO = await GetClaseAsync(Id);
            var clase = await MapClaseDTOtoEntity(claseDTO);
            var alumno = await TransforDTOAlumnotoEntity(AlumnoDTO);

            if (clase == null || alumno == null)
                return false;

            var existeAlumno = clase.AlumnosInscritos.Any(a => a.Id == alumno.Id);
            if (!existeAlumno)
            {
                clase.AlumnosInscritos.Add(alumno);
                _context.Update(clase);
                return await Save();
            }

            return false; 
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
            _context.Update(clase);
            return await Save();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var Clase = await GetClaseAsync(id);
            var ClaseMapped = await MapClaseDTOtoEntity(Clase);
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

        private async Task<ClaseEntity> MapClaseDTOtoEntity(ClaseDTO claseDTO)
        {
            var clase = _mapper.Map<ClaseEntity>(claseDTO);
            return clase;
        }

        private async Task<AlumnoEntity> TransforDTOAlumnotoEntity(AlumnoDTO alumnoDTO)
        {
            var alumno = _mapper.Map<AlumnoEntity>(alumnoDTO);
            return alumno;
        }

        private async Task<ClaseEntity> TransforCreateDTOtoDTO(CreateClaseDTO claseDTO)
        {
            var clase =  _mapper.Map<ClaseEntity>(claseDTO);
            return  clase;
        }

    }
}
