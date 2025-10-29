using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using RestAPI.Models.Entity;
using RestAPI.Repository.IRepository;
using RestAPI.Data;
using RestAPI.Models.DTOs.Alumnos;

namespace RestAPI.Repository
{
    public class AlumnoRepository : IAlumnoRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly string secretKey;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly int TokenExpirationDays = 7;

        public AlumnoRepository(ApplicationDbContext context, IConfiguration config,
            UserManager<AppUser> userManager, IMapper mapper)
        {
            _context = context;
            secretKey = config.GetValue<string>("ApiSettings:SecretKey");
            _userManager = userManager;
            _mapper = mapper;
        }

        public AppUser GetUser(string id)
        {
            return _context.Alumnos.FirstOrDefault(user => user.Id == id);
        }

        public ICollection<AlumnoEntity> GetAlumnos()
        {
            return _context.Alumnos
                           .Include(user => user.ClasesInscritas)
                           .OrderBy(user => user.UserName)
                           .ToList();
        }

        public bool IsUniqueUser(string userName)
        {
            return !_context.Alumnos.Any(user => user.UserName == userName);
        }

        public async Task<AlumnoLoginResponseDTO> Login(AlumnoLoginDTO userLoginDto)
        {
            var user = _context.Alumnos
                               .FirstOrDefault(u => u.Email.ToLower() == userLoginDto.Email.ToLower());

            if (user == null || !await _userManager.CheckPasswordAsync(user, userLoginDto.Password))
            {
                return new AlumnoLoginResponseDTO { Token = "", User = null };
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

            return new AlumnoLoginResponseDTO
            {
                Token = tokenHandler.WriteToken(jwtToken),
                User = user
            };
        }

        public async Task<AlumnoLoginResponseDTO?> Register(AlumnoRegistrationDTO userRegistrationDto)
        {
            var user = new AlumnoEntity
            {
                UserName = userRegistrationDto.UserName,
                Name = userRegistrationDto.Name,
                Email = userRegistrationDto.Email,
                NormalizedEmail = userRegistrationDto.Email.ToUpper(),
                NormalizedUserName = userRegistrationDto.UserName.ToUpper()
            };

            var result = await _userManager.CreateAsync(user, userRegistrationDto.Password);

            if (!result.Succeeded)
            {
                return null;
            }

            return new AlumnoLoginResponseDTO
            {
                User = user
            };
        }
    }
}
