using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestAPI.Data;
using RestAPI.Models.DTOs.Alumnos;
using RestAPI.Models.DTOs.Profesores;
using RestAPI.Models.Entity;
using RestAPI.Repository.IRepository;
namespace RestAPI.Repository
{
    public class ProfesorRepository : IProfesorRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;


        public ProfesorRepository(ApplicationDbContext context, IConfiguration config,
            UserManager<AppUser> userManager, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProfesorDTO?> GetProfesor(string id)
        {
            var profesor = await _context.Profesores
                 .Include(a => a.ClasesCreadas)
                 .FirstOrDefaultAsync(a => a.Id == id);

            return await TransforDTOtoEntity(profesor);
        }

        public async Task<List<ProfesorDTO>> GetProfesores()
        {
            var profesores = await _context.Profesores
                           .Include(user => user.ClasesCreadas)
                           .OrderBy(user => user.UserName)
                           .ToListAsync();

            return  TransForListEntityToDTO(profesores);
        }

        public async Task<ProfesorEntity?> UpdateProfesor(string id, ProfesorEntity profesor)
        {
            var existing = await _context.Profesores.FirstOrDefaultAsync(a => a.Id == id);
            if (existing == null)
                return null;

            existing.Name = profesor.Name;
            existing.Email = profesor.Email;
            existing.UserName = profesor.UserName;
            existing.Estado = profesor.Estado;
            existing.Telefono = profesor.Telefono;


            _context.Profesores.Update(existing);
            await _context.SaveChangesAsync();

            return existing;
        }

        public async Task<bool> DeleteProfesor(string username)
        {
            var profesor = await _context.Profesores.FirstOrDefaultAsync(a => a.UserName == username);
            if (profesor == null)
                return false;

            _context.Profesores.Remove(profesor);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<ProfesorDTO> TransforDTOtoEntity(ProfesorEntity profesorEntity)
        {
            var alumno = _mapper.Map<ProfesorDTO>(profesorEntity);
            return alumno;
        }

        private List<ProfesorDTO> TransForListEntityToDTO(List<ProfesorEntity> profesorEntity)
        {
            return _mapper.Map<List<ProfesorDTO>>(profesorEntity);
        }



    }
}

