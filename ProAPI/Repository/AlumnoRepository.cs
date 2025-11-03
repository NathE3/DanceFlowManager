using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPI.Data;
using RestAPI.Models.DTOs.Alumnos;
using RestAPI.Models.Entity;
using RestAPI.Repository.IRepository;

namespace RestAPI.Repository
{
    public class AlumnoRepository : IAlumnoRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;



        public AlumnoRepository(ApplicationDbContext context, IConfiguration config,
             IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<AlumnoDTO>> GetAlumnos()
        {
            var alumnosEntity = await _context.Alumnos
                .Include(a => a.ClasesInscritas)
                .OrderBy(a => a.UserName)
                .ToListAsync();

            return TransForListEntityToDTO(alumnosEntity);
        }


        public async Task<AlumnoDTO?> GetById(string id)
        {
            var alumno = await _context.Alumnos
                .Include(a => a.ClasesInscritas)
                .FirstOrDefaultAsync(a => a.Id == id);

            return await TransforDTOtoEntity(alumno);

        }

        public async Task<AlumnoEntity?> UpdateAsync(string id, AlumnoEntity alumno)
        {
            var existing = await _context.Alumnos.FirstOrDefaultAsync(a => a.Id == id);
            if (existing == null)
                return null;

            existing.Name = alumno.Name;
            existing.Email = alumno.Email;
            existing.UserName = alumno.UserName;
            existing.Telefono = alumno.Telefono;

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

        private async Task<AlumnoDTO> TransforDTOtoEntity(AlumnoEntity alumnoEntity)
        {
            var alumno = _mapper.Map<AlumnoDTO>(alumnoEntity);
            return alumno;
        }

        private List<AlumnoDTO> TransForListEntityToDTO(List<AlumnoEntity> alumnosEntity)
        {
            return _mapper.Map<List<AlumnoDTO>>(alumnosEntity);
        }


    }
}
