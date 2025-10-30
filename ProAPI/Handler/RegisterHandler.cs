using global::RestAPI.Data;
using global::RestAPI.Models.DTOs.Commons;
using global::RestAPI.Models.DTOs.Register;
using global::RestAPI.Models.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace RestAPI.Handler
{
    public class RegisterHandler : IRegisterHandler
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public RegisterHandler(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<UserRegisterResponse> RegisterAsync(UserRegisterRequest userRegisterRequest)
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

                var userResponse = await _context.Profesores
                    .FirstOrDefaultAsync(usuario => usuario.Email.ToLower() == user.Email.ToLower());

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

                var userResponse = await _context.Alumnos
                    .FirstOrDefaultAsync(usuario => usuario.Email.ToLower() == user.Email.ToLower());

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
        }
    }
}

