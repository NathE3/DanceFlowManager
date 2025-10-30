using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RestAPI.Data;
using RestAPI.Models.DTOs.Commons;
using RestAPI.Models.DTOs.Login;
using RestAPI.Models.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RestAPI.Handler
{
    public class AuthHandler : IAuthHandler
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly string _secretKey;
        private readonly int _tokenExpirationDays = 7;

        public AuthHandler(ApplicationDbContext context, IConfiguration config, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _secretKey = config.GetValue<string>("ApiSettings:SecretKey");
        }

        public async Task<UserLoginResponse> AuthenticateAsync(UserLoginRequest request)
        {
            AppUser? user = request.IsProfesor
                ? _context.Profesores.FirstOrDefault(u => u.Email.ToLower() == request.Email.ToLower())
                : _context.Alumnos.FirstOrDefault(u => u.Email.ToLower() == request.Email.ToLower());

            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return new UserLoginResponse { Status = Status.ERROR };
            }

            var token = GenerarJwtToken(user);

            return new UserLoginResponse
            {
                Status = Status.Completed_OK,
                Token = token,
                User = user
            };
        }

        private string GenerarJwtToken(AppUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName)
                }),
                Expires = DateTime.UtcNow.AddDays(_tokenExpirationDays),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var jwtToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(jwtToken);
        }
    }
}
    

