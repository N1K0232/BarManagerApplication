using AutoMapper;
using BackendGestionaleBar.DataAccessLayer.Entities;
using BackendGestionaleBar.Shared.Models;

namespace BackendGestionaleBar.BusinessLayer.MapperProfile
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<ApplicationUser, User>();
        }
    }
}