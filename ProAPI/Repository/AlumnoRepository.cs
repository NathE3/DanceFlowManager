using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestAPI.Models.Entity;
using RestAPI.Repository.IRepository;
using RestAPI.Data;

namespace RestAPI.Repository
{
    public class AlumnoRepository : IAlumnoRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
    

        public AlumnoRepository(ApplicationDbContext context, IConfiguration config,
            UserManager<AppUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<ICollection<AlumnoEntity>> GetAlumnos()
        {
            return await _context.Alumnos
                .Include(a => a.ClasesInscritas)
                .OrderBy(a => a.UserName)
                .ToListAsync();
        }

        public async Task<AlumnoEntity?> GetById(string id)
        {
            return await _context.Alumnos
                .Include(a => a.ClasesInscritas)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<AlumnoEntity?> UpdateAsync(string id, AlumnoEntity alumno)
        {
            var existing = await _context.Alumnos.FirstOrDefaultAsync(a => a.Id == id);
            if (existing == null)
                return null;

            existing.Name = alumno.Name;
            existing.Email = alumno.Email;
            existing.UserName = alumno.UserName;

            _context.Alumnos.Update(existing);
            await _context.SaveChangesAsync();

            return existing;
        }

        public async Task<bool> DeleteAsync(string username)
        {
            var alumno = await _context.Alumnos.FirstOrDefaultAsync(a => a.UserName == username);
            if (alumno == null)
                return false;

            _context.Alumnos.Remove(alumno);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
