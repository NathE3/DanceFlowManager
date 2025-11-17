using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Models.DTOs.Alumnos;
using RestAPI.Models.DTOs.Clases;
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
            

        public ClaseController(IClasesRepository repository, IMapper mapper)
            {
                _claseRepository = repository;
                _mapper = mapper;                
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

        [HttpPost("{id}/anadir-alumno")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AnadirAlumno(Guid Id, AlumnoDTO Alumno)
        {
            try
            {
                var entity = await _claseRepository.AnadirAlumno(Id, Alumno);
                if (entity == false) return NotFound();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpDelete("{idClase}/eliminar-alumno/{idAlumno}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EliminarAlumno(Guid idClase, string idAlumno)
        {
            try
            {
                var result = await _claseRepository.EliminarAlumno(idClase, idAlumno);
                if (!result) return NotFound();

                return Ok(true);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



    }
}
