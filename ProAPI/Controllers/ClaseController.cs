using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPI.Data;
using RestAPI.Models.DTOs.Alumnos;
using RestAPI.Models.DTOs.Clases;
using RestAPI.Models.DTOs.Inscripcion;
using RestAPI.Models.Entity;
using RestAPI.Repository.IRepository;
using System.Security.Claims;

namespace RestAPI.Controllers
{
        
        [Route("DanceFlowApi/[controller]")]
        [ApiController]
    public  class ClaseController : ControllerBase
        {
            private readonly IClasesRepository _claseRepository;
            private readonly IMapper _mapper;
            private readonly ApplicationDbContext _context;



        public ClaseController(IClasesRepository repository, IMapper mapper, ApplicationDbContext context)
            {
                _claseRepository = repository;
                _mapper = mapper;            
                _context = context; 
            }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ClaseDTO>>> GetAll()
        {
            try
            {
                var entities = await _claseRepository.GetAllAsync();
                return Ok(entities);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet("user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllFromUser()
        {
            try
            {
                var entities = await _claseRepository.GetAllFromProfesorAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
                return Ok(entities);
            }
            catch (Exception ex)
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}", Name = "[controller]_GetClaseEntity")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetClase(Guid id)
        {
            try
            {
                var clase = await _claseRepository.GetClaseAsync(id);
                if (clase == null) return NotFound();

                return Ok(clase);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateClaseDTO createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var entity = new CreateClaseDTO
                {
                    Nombre = createDto.Nombre,
                    Tipo = createDto.Tipo,
                    Descripcion = createDto.Descripcion,
                    FechaClase = createDto.FechaClase,
                    IdProfesor = createDto.IdProfesor
                };

                var dto = await _claseRepository.CreateAsync(entity);

                return StatusCode(StatusCodes.Status201Created, dto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] ClaseDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var entity = await _claseRepository.GetClaseAsync(id);
                if (entity == null) return NotFound();

                _mapper.Map(dto, entity);
                await _claseRepository.UpdateAsync(entity);

                return Ok();
            }
            catch (Exception ex)
            {            
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var entity = await _claseRepository.GetClaseAsync(id);
                if (entity == null) return NotFound();

                await _claseRepository.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {           
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("{claseId}/anadir-alumno/{alumnoId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<bool> AnadirAlumno(Guid claseId, string alumnoId)
        {
            var clase = await _context.Clases
                .Include(c => c.AlumnosInscritos)
                .FirstOrDefaultAsync(c => c.Id == claseId);

            if (clase == null)
                return false;

            var alumno = await _context.Alumnos
                .FirstOrDefaultAsync(a => a.Id == alumnoId);

            if (alumno == null)
                return false;

            if (!clase.AlumnosInscritos.Any(a => a.Id == alumno.Id))
            {
                clase.AlumnosInscritos.Add(alumno);
                return await _context.SaveChangesAsync() > 0;
            }

            return false;
        }



        [HttpDelete("{idClase}/eliminar-alumno/{idAlumno}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EliminarAlumno(Guid idClase, string idAlumno)
        {
            var resultado = await _claseRepository.EliminarAlumno(idClase, idAlumno);

            if (!resultado)
                return NotFound("No se pudo eliminar al alumno o no estaba inscrito.");

            return Ok(true);
        }




    }
}
