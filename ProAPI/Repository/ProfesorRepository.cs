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
using RestAPI.Models.DTOs.Profesores;
using RestAPI.Models.DTOs.Login;
namespace RestAPI.Repository
{
    public class ProfesorRepository : IProfesorRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly string secretKey;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly int TokenExpirationDays = 7;

        public ProfesorRepository(ApplicationDbContext context, IConfiguration config,
            UserManager<AppUser> userManager, IMapper mapper)
        {
            _context = context;
            secretKey = config.GetValue<string>("ApiSettings:SecretKey");
            _userManager = userManager;
            _mapper = mapper;
        }

        public AppUser GetProfesor(string id)
        {
            ProfesorEntity profe = _context.Profesores.FirstOrDefault(user => user.Id == id);
            return profe;
        }

        public ICollection<ProfesorEntity> GetProfesores()
        {
           

            return _context.Profesores
                           .Include(user => user.ClasesCreadas)
                           .OrderBy(user => user.UserName)
                           .ToList();
        }

        public bool IsUniqueUser(string userName)
        {
            return !_context.Alumnos.Any(user => user.UserName == userName);
        }

    }
}

