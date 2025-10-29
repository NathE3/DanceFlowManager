using RestAPI.Models.DTOs.Commons;
using RestAPI.Models.Entity;

namespace RestAPI.Models.DTOs.Login
{
    public class UserLoginResponse : BaseResponse
    {
        public AppUser User { get; set; }
        public string Token { get; set; }
    }
}
