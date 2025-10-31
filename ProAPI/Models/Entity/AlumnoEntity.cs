namespace RestAPI.Models.Entity
{
    public class AlumnoEntity : AppUser
    {
        public string Apellidos { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime? FechaBaja { get; set; }
        public int Telefono { get; set; }
        public ICollection<ClaseEntity> ClasesInscritas { get; set; } = new List<ClaseEntity>();
    }
}
