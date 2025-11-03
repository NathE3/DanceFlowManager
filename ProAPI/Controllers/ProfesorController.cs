using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Models.DTOs.Alumnos;
using RestAPI.Models.DTOs.Profesores;
using RestAPI.Models.Entity;
using RestAPI.Repository.IRepository;

namespace RestAPI.Controllers
{
    [Route("DanceFlowApi/[controller]")]
    [ApiController]
    public class ProfesorController : ControllerBase
    {
        private readonly IProfesorRepository _profesorRepository;

        public ProfesorController(IProfesorRepository profesorRepository)
        {
            _profesorRepository = profesorRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProfesorDTO>>> GetAll()
        {
            try
            {
                var profesores = await _profesorRepository.GetProfesores();
                if (profesores == null) return NotFound();
                return Ok(profesores);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProfesorDTO>> GetProfesor(string id)
        {
            try
            {
                var profesor = await _profesorRepository.GetProfesor(id);
                if (profesor == null) return NotFound();
                return Ok(profesor);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProfesorEntity>> UpdateProfesor(string id, [FromBody] ProfesorEntity profesor)
        {
            var updatedProfesor = await _profesorRepository.UpdateProfesor(id, profesor);
            if (updatedProfesor == null)
                return NotFound();
            return Ok();
        }

        [HttpDelete("{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult> DeleteAlumno(string username)
        {
            var result = await _profesorRepository.DeleteProfesor(username);
            if (!result)
                return NotFound();
            return Ok();
        }
    }
}
