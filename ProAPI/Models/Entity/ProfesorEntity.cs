namespace RestAPI.Models.Entity
{
    public class ProfesorEntity : AppUser
    {
        public ICollection<ClaseEntity> ClasesCreadas { get; set; } = new List<ClaseEntity>();
    }
}
