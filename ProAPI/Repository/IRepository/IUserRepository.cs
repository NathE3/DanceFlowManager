using RestAPI.Models.DTOs.Login;
using RestAPI.Models.DTOs.Register;

namespace RestAPI.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<UserLoginResponse> Login(UserLoginRequest userLogin);
        Task<UserRegisterResponse> Register(UserRegisterRequest userRegistrationDTO);
    }
}
