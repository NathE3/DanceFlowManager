using RestAPI.Models.DTOs.Login;

namespace RestAPI.Handler.IHandler
{
    public interface IAuthHandler
    {
        Task<UserLoginResponse> AuthenticateAsync(UserLoginRequest userLoginRequest);
    }
}
