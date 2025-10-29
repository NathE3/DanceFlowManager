
using AutoMapper;
using RestAPI.Models.DTOs.Clases;
using RestAPI.Models.DTOs.Alumnos;
using RestAPI.Models.Entity;
using RestAPI.Models.DTOs.Profesores;

namespace RestAPI.AutoMapper
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<AlumnoEntity, AlumnoDTO>().ReverseMap();
            CreateMap<ClaseDTO, ClaseEntity>().ReverseMap();
            CreateMap<CreateClaseDTO, ClaseEntity>().ReverseMap();
            CreateMap<ProfesorEntity, ProfesorDTO>().ReverseMap();

        }
    }
}
