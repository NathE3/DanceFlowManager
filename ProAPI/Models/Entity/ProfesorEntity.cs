namespace RestAPI.Models.Entity
{
    public class ProfesorEntity : AppUser
    {
        public string Estado { get; set; }
        public string Apellido { get; set; }
        public ICollection<ClaseEntity> ClasesCreadas { get; set; } = new List<ClaseEntity>();
    }
}
