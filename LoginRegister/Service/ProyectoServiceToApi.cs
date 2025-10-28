using InfoManager.Helpers;
using InfoManager.Interface;
using InfoManager.Models;
using InfoManager.Services;



namespace InfoManager.Service
{

   public class ProyectoServiceToApi : IProyectoServiceToApi
    {
        private readonly IHttpJsonProvider<ProyectoDTO> _httpJsonProvider;
      

        public ProyectoServiceToApi(IHttpJsonProvider<ProyectoDTO>  httpJsonProvider) 
        {
            _httpJsonProvider = httpJsonProvider;
        }



         public async  Task<IEnumerable<ProyectoDTO>> GetProyectos()
         {
 
            IEnumerable<ProyectoDTO> proyectos = await _httpJsonProvider.GetAsync(Constants.PROYECTO_URL);

         return proyectos;
         }

        public async Task PostProyecto(ProyectoDTO proyecto)
            {
                try
                {
                    if (proyecto == null) return;
                    var response = await _httpJsonProvider.PostAsync(Constants.PROYECTO_URL, proyecto);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
        }

        public async Task PutProyecto(ProyectoDTO proyecto)
        {
            try
            {
                if (proyecto == null) return;
                var response = await _httpJsonProvider.PutAsync(Constants.PROYECTO_URL + "/" + proyecto.Id, proyecto);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task CambiarEstado(ProyectoDTO proyecto)
        {
            try
            {
                if (proyecto == null) return;
                var response = await _httpJsonProvider.PutAsync(Constants.PROYECTO_STATE_URL + "/" + proyecto.Id, proyecto);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
   
}