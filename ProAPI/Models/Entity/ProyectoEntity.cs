using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RestAPI.Models.Entity
{
    public class ProyectoEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; }
        public DateTime CreatedDate { get; set; }

        [Required]
        [MaxLength(200)]
        public string Descripcion { get; set; }

        [Required]
        [MaxLength(50)]
        //Documental, Innovación, Gestión
        public string Tipo { get; set; }

        [ForeignKey("IdAlumno")]
        public string IdAlumno { get; set; }
        public string? IdProfesor { get; set; }

        [Required]
        [MaxLength(50)]
        //Aprovado, Pendiente, Rechazado, Cerrado
        public String Estado { get; set; }

        public AppUser Alumno { get; set; }

        public AppUser? Profesor { get; set; }


    }
}
