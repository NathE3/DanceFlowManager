using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RestAPI.Data;
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
        private readonly string secretKey;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly int TokenExpirationDays = 7;

        public UserRepository(ApplicationDbContext context, IConfiguration config,
            UserManager<AppUser> userManager, IMapper mapper)
        {
            _context = context;
            secretKey = config.GetValue<string>("ApiSettings:SecretKey");
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<UserLoginResponse> Login(UserLoginRequest userLogin)
        {

            if (userLogin.IsProfesor)
            {
                var user = _context.Profesores.FirstOrDefault(u => u.Email.ToLower() == userLogin.Email.ToLower());
                if (user == null || !await _userManager.CheckPasswordAsync(user, userLogin.Password))
                {
                    return new UserLoginResponse { Status = Status.ERROR };
                }
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(secretKey);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName)
                }),
                    Expires = DateTime.UtcNow.AddDays(TokenExpirationDays),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
                };

                var jwtToken = tokenHandler.CreateToken(tokenDescriptor);
                return new UserLoginResponse
                {
                    Token = tokenHandler.WriteToken(jwtToken),
                    User = user
                };

            }
            else
            {

                var user = _context.Alumnos.FirstOrDefault(u => u.Email.ToLower() == userLogin.Email.ToLower());
                if (user == null || !await _userManager.CheckPasswordAsync(user, userLogin.Password))
                {
                    return new UserLoginResponse { Token = "", User = null };
                }
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(secretKey);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName)
                }),
                    Expires = DateTime.UtcNow.AddDays(TokenExpirationDays),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
                };

                var jwtToken = tokenHandler.CreateToken(tokenDescriptor);
                return new UserLoginResponse
                {
                    Token = tokenHandler.WriteToken(jwtToken),
                    User = user
                };

            }
            return new UserLoginResponse();
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
                var userResponse = _context.Profesores.FirstOrDefault(u => u.Email.ToLower() == user.Email.ToLower());
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
                var userResponse = _context.Alumnos.FirstOrDefault(u => u.Email.ToLower() == user.Email.ToLower());
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
