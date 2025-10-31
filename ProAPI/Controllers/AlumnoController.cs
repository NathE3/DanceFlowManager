using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Models.DTOs.Alumnos;
using RestAPI.Models.Entity;
using RestAPI.Repository;
using RestAPI.Repository.IRepository;
using System.Collections.Generic;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlumnoController : ControllerBase
    {
        private readonly IAlumnoRepository _alumnoRepository;
        private readonly IMapper _mapper;


        public AlumnoController(IAlumnoRepository alumnoRepository)
        {
            _alumnoRepository = alumnoRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlumnoDTO>>> GetAll()
        {
            try
            {

                var alumnos = await _alumnoRepository.GetAlumnos();
                if(alumnos == null)return NotFound();
                var alumnosMapped = _mapper.Map<List<AlumnoDTO>>(alumnos);
                return Ok(alumnosMapped);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AlumnoDTO>> GetAlumno(string id)
        {
            try
            {
                var alumno = await _alumnoRepository.GetById(id);
                if (alumno == null) return NotFound();
                var alumnoMapped = _mapper.Map<AlumnoDTO>(alumno);
                return Ok(alumnoMapped);

            }
            catch (Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AlumnoEntity>> UpdateAlumno(string id, [FromBody] AlumnoEntity alumno)
        {
            var updatedAlumno = await _alumnoRepository.UpdateAsync(id, alumno);
            if (updatedAlumno == null)
                return NotFound();
            return Ok(updatedAlumno);
        }

        [HttpDelete("{username}")]
        public async Task<ActionResult> DeleteAlumno(string username)
        {
            var result = await _alumnoRepository.DeleteAsync(username);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}
