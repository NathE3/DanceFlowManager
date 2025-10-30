using RestAPI.Models.DTOs.Login;

namespace RestAPI.Handler
{
    public interface IAuthHandler
    {
        Task<UserLoginResponse> AuthenticateAsync(UserLoginRequest userLoginRequest);
    }
}
