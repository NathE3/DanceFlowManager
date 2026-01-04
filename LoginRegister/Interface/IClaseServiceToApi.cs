using InfoManager.Models;


namespace InfoManager.Interface
{
    public interface IClaseServiceToApi
    {
        Task<IEnumerable<ClaseDTO>> GetClases();
  
        Task PostClase(ClaseDTO proyecto);

        Task PutClase(ClaseDTO proyecto);
    }
}
