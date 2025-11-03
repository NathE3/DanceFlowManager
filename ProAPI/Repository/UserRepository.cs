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
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IAuthHandler _authHandler;
        private readonly IRegisterHandler _registerHandler;
        private readonly IMapper _mapper;
        public UserRepository(ApplicationDbContext context, IConfiguration config,
            UserManager<AppUser> userManager, IMapper mapper, IRegisterHandler registerHandler)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _registerHandler = registerHandler;

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
