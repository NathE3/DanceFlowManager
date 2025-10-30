using RestAPI.Models.DTOs.Register;

namespace RestAPI.Handler
{
    public interface IRegisterHandler
    {
        Task<UserRegisterResponse> RegisterAsync(UserRegisterRequest userRequest);

    }
}
