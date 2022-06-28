using AutoMapper;
using BackendGestionaleBar.Authentication.Entities;
using BackendGestionaleBar.Shared.Requests;

namespace BackendGestionaleBar.BusinessLayer.MapperProfiles;

internal class UserMapperProfile : Profile
{
	public UserMapperProfile()
	{
		CreateMap<RegisterUserRequest, ApplicationUser>();
	}
}