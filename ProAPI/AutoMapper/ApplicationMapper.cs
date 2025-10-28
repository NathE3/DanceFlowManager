
using AutoMapper;
using RestAPI.Models.DTOs;
using RestAPI.Models.DTOs;
using RestAPI.Models.DTOs.UserDto;



//using RestAPI.Models.DTOs.LibroDTO;
using RestAPI.Models.Entity;

namespace RestAPI.AutoMapper
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<AppUser, UserDto>().ReverseMap();
            CreateMap<ProyectoDTO, ProyectoEntity>().ReverseMap();
            CreateMap<CreateProyectoDTO, ProyectoEntity>().ReverseMap();
            CreateMap<CreateProyectoUserDTO, ProyectoEntity>().ReverseMap();

        }
    }
}
