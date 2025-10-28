using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Controllers.RestAPI.Controllers;
using RestAPI.Models.DTOs;
using RestAPI.Models.Entity;
using RestAPI.Repository;
using RestAPI.Repository.IRepository;
using System.Security.Claims;

namespace RestAPI.Controllers
{
        
        [Route("api/[controller]")]
        [ApiController]
    public  class ProyectoController : ControllerBase
        {
            private readonly IProyectoRepository _proyectoRepository;
            private readonly IMapper _mapper;
            //protected readonly ILogger _logger;

        public ProyectoController(IProyectoRepository repository, IMapper mapper /*ILogger logger*/)
            {
                _proyectoRepository = repository;
                _mapper = mapper;
                //_logger = logger;
            }

        [HttpGet]
        //[Authorize(Roles = "profesor,alumno")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            try
            {

                var entities = _mapper.Map<List<ProyectoDTO>>(await _proyectoRepository.GetAllAsync());
                return Ok(entities);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error fetching data");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet("user")]
        [Authorize(Roles = "alumno")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllFromUser()
        {
            try
            {

                var entities = _mapper.Map<List<ProyectoDTO>>(await _proyectoRepository.GetAllFromUserAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)));
                return Ok(entities);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error fetching data");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "profesor,alumno")]
        [HttpGet("{id:int}", Name = "[controller]_GeProyectoEntity")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var entity = await _proyectoRepository.GetAsync(id);
                if (entity == null) return NotFound();

                return Ok(_mapper.Map<ProyectoDTO>(entity));
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error fetching data");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "profesor,alumno")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateProyectoDTO createDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
               
                var entity = _mapper.Map<ProyectoEntity>(new CreateProyectoUserDTO
                {
                    Nombre = createDto.Nombre,
                    Tipo = createDto.Tipo,
                    Descripcion = createDto.Descripcion,
                    Estado = createDto.Estado,
                    IdProfesor = createDto.IdProfesor,
                    IdAlumno = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    
                    
                });
                await _proyectoRepository.CreateAsync(entity);

                var dto = _mapper.Map<ProyectoDTO>(entity);
                return CreatedAtRoute($"{ControllerContext.ActionDescriptor.ControllerName}_GeProyectoEntity", new { id = entity.GetHashCode() }, dto);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error creating data");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "profesor,alumno")]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] ProyectoDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var entity = await _proyectoRepository.GetAsync(id);
                if (entity == null) return NotFound();

                _mapper.Map(dto, entity);
                await _proyectoRepository.UpdateAsync(entity);

                return Ok(_mapper.Map<ProyectoDTO>(entity));
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error updating data");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "profesor")]
        [HttpPut("State/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateState(int id, [FromBody] ProyectoDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                if(dto.Estado== "Aprobado")
                {
                    dto.IdProfesor = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    //dto.ProfesorNombre=User.FindFirstValue(ClaimTypes.Name);
                }


                var entity = await _proyectoRepository.GetAsync(id);
                if (entity == null) return NotFound();

                _mapper.Map(dto, entity);
                await _proyectoRepository.UpdateAsync(entity);

                return Ok(_mapper.Map<ProyectoDTO>(entity));
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error updating data");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "profesor,alumno")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var entity = await _proyectoRepository.GetAsync(id);
                if (entity == null) return NotFound();

                await _proyectoRepository.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error deleting data");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
