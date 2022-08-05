using AutoMapper;
using BackendGestionaleBar.Authentication.Entities;
using BackendGestionaleBar.Shared.Models;
using BackendGestionaleBar.Shared.Requests;

namespace BackendGestionaleBar.BusinessLayer.MapperProfiles;

internal class UserMapperProfile : Profile
{
	public UserMapperProfile()
	{
		CreateMap<AuthenticationUser, User>();
		CreateMap<RegisterUserRequest, AuthenticationUser>();
	}
}