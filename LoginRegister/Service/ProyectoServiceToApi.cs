using InfoManager.Helpers;
using InfoManager.Interface;
using InfoManager.Models;
using InfoManager.Services;



namespace InfoManager.Service
{

   public class ProyectoServiceToApi : IProyectoServiceToApi
    {
        private readonly IHttpJsonProvider<ProfesorDTO> _httpJsonProvider;
      

        public ProyectoServiceToApi(IHttpJsonProvider<ProfesorDTO>  httpJsonProvider) 
        {
            _httpJsonProvider = httpJsonProvider;
        }



         public async  Task<IEnumerable<ProfesorDTO>> GetProyectos()
         {
 
            IEnumerable<ProfesorDTO> proyectos = await _httpJsonProvider.GetAsync(Constants.PROYECTO_URL);

         return proyectos;
         }

        public async Task PostProyecto(ProfesorDTO proyecto)
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

        public async Task PutProyecto(ProfesorDTO proyecto)
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
        public async Task CambiarEstado(ProfesorDTO proyecto)
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