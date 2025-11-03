
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Models.DTOs;
using RestAPI.Models.DTOs.Alumnos;
using RestAPI.Models.DTOs.Commons;
using RestAPI.Models.DTOs.Login;
using RestAPI.Models.DTOs.Register;
using RestAPI.Repository.IRepository;
using System.Net;

namespace RestAPI.Controllers
{
    [Route("DanceFlowApi/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        protected ResponseApi _reponseApi;
        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _reponseApi = new ResponseApi();
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<UserRegisterResponse> Register(UserRegisterRequest userRegisterRequest)
        {
            if (!ModelState.IsValid)
            {
                return new UserRegisterResponse { Status = Status.ERROR };
            }
            var newUser = await _userRepository.Register(userRegisterRequest);

            return newUser;
        }


        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<UserLoginResponse> Login(UserLoginRequest userLogin)
        {
            var responseLogin = await _userRepository.Login(userLogin);

            return responseLogin;
        }
    }
}
