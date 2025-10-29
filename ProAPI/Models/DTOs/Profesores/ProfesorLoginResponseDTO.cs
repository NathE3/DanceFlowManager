using RestAPI.Models.Entity;

namespace RestAPI.Models.DTOs.ProfesorDTO
{
    public class ProfesorLoginResponseDTO
    {
        public AppUser User { get; set; }
        public string Token { get; set; }
    }
}
