using InfoManager.Models;


namespace InfoManager.Interface
{
    public interface IProyectoServiceToApi
    {
        // Obtiene un Proyectos desde la API
        Task<IEnumerable<ProfesorDTO>> GetProyectos();

        // Agrega un Proyecto a la API
        Task PostProyecto(ProfesorDTO proyecto);

        // Modifica un Proyecto ya existente
        Task PutProyecto(ProfesorDTO proyecto);
        Task CambiarEstado(ProfesorDTO proyecto);
    }
}
