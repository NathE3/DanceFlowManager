using InfoManager.Models;


namespace InfoManager.Interface
{
    public interface IProyectoServiceToApi
    {
        // Obtiene un Proyectos desde la API
        Task<IEnumerable<ProyectoDTO>> GetProyectos();

        // Agrega un Proyecto a la API
        Task PostProyecto(ProyectoDTO proyecto);

        // Modifica un Proyecto ya existente
        Task PutProyecto(ProyectoDTO proyecto);
        Task CambiarEstado(ProyectoDTO proyecto);
    }
}
