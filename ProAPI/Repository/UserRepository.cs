using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RestAPI.Data;
using RestAPI.Handler;
using RestAPI.Models.DTOs.Alumnos;
using RestAPI.Models.DTOs.Commons;
using RestAPI.Models.DTOs.Login;
using RestAPI.Models.DTOs.Register;
using RestAPI.Models.Entity;
using RestAPI.Repository.IRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RestAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly AuthHandler _authHandler;
        private readonly IMapper _mapper;
        public UserRepository(ApplicationDbContext context, IConfiguration config,
            UserManager<AppUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<UserLoginResponse> Login(UserLoginRequest userLogin)
        {           
            return await _authHandler.AuthenticateAsync(userLogin);
        }

        public async Task<UserRegisterResponse> Register(UserRegisterRequest userRegisterRequest)
        {

            if (userRegisterRequest.IsProfesor)
            {
                var user = new ProfesorEntity
                {
                    UserName = userRegisterRequest.UserName,
                    Name = userRegisterRequest.Name,
                    Email = userRegisterRequest.Email,
                    NormalizedEmail = userRegisterRequest.Email.ToUpper(),
                    NormalizedUserName = userRegisterRequest.UserName.ToUpper(),
                    Estado = userRegisterRequest.Estado,
                };
                var userResponse = _context.Profesores.FirstOrDefault(usuario => usuario.Email.ToLower() == user.Email.ToLower());
                if (userResponse != null) {
                    return new UserRegisterResponse { Status = Status.ERROR };
                }
                var result = await _userManager.CreateAsync(user, userRegisterRequest.Password);

            }
            else
            {
                var user = new AlumnoEntity
                {

                    UserName = userRegisterRequest.UserName,
                    Name = userRegisterRequest.Name,
                    Email = userRegisterRequest.Email,
                    NormalizedEmail = userRegisterRequest.Email.ToUpper(),
                    NormalizedUserName = userRegisterRequest.UserName.ToUpper()
                };
                var userResponse = _context.Alumnos.FirstOrDefault(usuario => usuario.Email.ToLower() == user.Email.ToLower());
                if (userResponse != null)
                {
                    return new UserRegisterResponse { Status = Status.ERROR };
                }
                var result = await _userManager.CreateAsync(user, userRegisterRequest.Password);

                if (!result.Succeeded)
                {
                    return new UserRegisterResponse
                    {
                        Status = Status.ERROR
                    };
                }

                return new UserRegisterResponse
                {
                    Status = Status.Completed_OK
                };
            }

            return new UserRegisterResponse();
       
        }



    }
}
