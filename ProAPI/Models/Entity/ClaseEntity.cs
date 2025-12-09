using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RestAPI.Models.Entity
{
    public class ClaseEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; }
        public DateTime CreatedDate { get; set; }

        [Required]
        [MaxLength(200)]
        public string Descripcion { get; set; }

        [Required]
        [MaxLength(50)]
        public string Tipo { get; set; }

        [ForeignKey("IdProfesor")]
        public string? IdProfesor { get; set; }
        public ProfesorEntity Profesor { get; set; }

        public ICollection<AlumnoEntity> AlumnosInscritos { get; set; } = new List<AlumnoEntity>();
    }

}

