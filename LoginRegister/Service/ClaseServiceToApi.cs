using InfoManager.Helpers;
using InfoManager.Interface;
using InfoManager.Models;
using InfoManager.Services;



namespace InfoManager.Service
{

   public class ClaseServiceToApi : IClaseServiceToApi
    {
        private readonly IHttpJsonProvider<ClaseDTO> _httpJsonProvider;
      

        public ClaseServiceToApi(IHttpJsonProvider<ClaseDTO>  httpJsonProvider) 
        {
            _httpJsonProvider = httpJsonProvider;
        }



         public async  Task<IEnumerable<ClaseDTO>> GetClases()
         {
 
            IEnumerable<ClaseDTO> clases = await _httpJsonProvider.GetAsync(Constants.CLASE_URL);

         return clases;
         }

        public async Task<IEnumerable<ClaseDTO>> GetClase(string id)
        {

            IEnumerable<ClaseDTO> clases = await _httpJsonProvider.GetAsync(Constants.CLASE_URL + "/" + id);

            return clases;
        }

        public async Task PostClase(ClaseDTO clase)
            {
                try
                {
                    if (clase == null) return;
                    var response = await _httpJsonProvider.PostAsync(Constants.CLASE_URL, clase);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
        }

        public async Task PutClase(ClaseDTO clase)
        {
            try
            {
                if (clase == null) return;
                var response = await _httpJsonProvider.PutAsync(Constants.CLASE_URL + "/" + clase.Id_clase, clase);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task DeleteClase(string id)
        {
            try
            {
                if (id == null) return;
                var response = await _httpJsonProvider.DeleteAsync(Constants.CLASE_URL + "/" + id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
   
}