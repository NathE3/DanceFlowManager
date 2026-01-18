using AutoMapper;
using Microsoft.AspNetCore.Identity;
using RestAPI.Data;
using RestAPI.Handler;
using RestAPI.Models.DTOs.Commons;
using RestAPI.Models.DTOs.Login;
using RestAPI.Models.DTOs.Register;
using RestAPI.Models.Entity;
using RestAPI.Repository.IRepository;

namespace RestAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IAuthHandler _authHandler;
        private readonly IRegisterHandler _registerHandler;
        public UserRepository(IRegisterHandler registerHandler, IAuthHandler authHandler)
        {
            _registerHandler = registerHandler;
            _authHandler = authHandler;
        }

        public async Task<UserLoginResponse> Login(UserLoginRequest userLogin)
        {
            return await _authHandler.AuthenticateAsync(userLogin);
        }

        public async Task<UserRegisterResponse> Register(UserRegisterRequest userRegisterRequest)
        {

            return await _registerHandler.RegisterAsync(userRegisterRequest);

        }



    }
}
