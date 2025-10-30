using Microsoft.AspNetCore.Mvc;
using RestAPI.Models.Entity;
using RestAPI.Repository.IRepository;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlumnoController : ControllerBase
    {
        private readonly IAlumnoRepository _alumnoRepository;

        public AlumnoController(IAlumnoRepository alumnoRepository)
        {
            _alumnoRepository = alumnoRepository;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<AlumnoEntity>>> GetAlumnos()
        {
            var alumnos = await _alumnoRepository.GetAlumnos();
            return Ok(alumnos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AlumnoEntity>> GetAlumno(string id)
        {
            var alumno = await _alumnoRepository.GetById(id);
            if (alumno == null)
                return NotFound();
            return Ok(alumno);
        }

        [HttpPost]
        public async Task<ActionResult<AlumnoEntity>> CreateAlumno([FromBody] AlumnoEntity alumno)
        {
            var createdAlumno = await _alumnoRepository.CreateAsync(alumno);
            return CreatedAtAction(nameof(GetAlumno), new { id = createdAlumno.Id }, createdAlumno);
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
