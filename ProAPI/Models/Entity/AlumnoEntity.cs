namespace RestAPI.Models.Entity
{
    public class AlumnoEntity : AppUser
    {
        public ICollection<ClaseEntity> ClasesInscritas { get; set; } = new List<ClaseEntity>();
    }
}
