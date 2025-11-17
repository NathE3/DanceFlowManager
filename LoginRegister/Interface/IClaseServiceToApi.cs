using InfoManager.Models;


namespace InfoManager.Interface
{
    public interface IClaseServiceToApi
    {
        // Obtiene un Proyectos desde la API
        Task<IEnumerable<ClaseDTO>> GetClases();

        // Agrega un Proyecto a la API
        Task PostClase(ClaseDTO proyecto);

        // Modifica un Proyecto ya existente
        Task PutClase(ClaseDTO proyecto);
    }
}
