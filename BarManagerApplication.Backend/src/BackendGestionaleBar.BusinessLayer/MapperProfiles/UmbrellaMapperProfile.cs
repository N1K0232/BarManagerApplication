using AutoMapper;
using BackendGestionaleBar.Shared.Models;
using Entities = BackendGestionaleBar.DataAccessLayer.Entities;

namespace BackendGestionaleBar.BusinessLayer.MapperProfiles;

internal class UmbrellaMapperProfile : Profile
{
	public UmbrellaMapperProfile()
	{
		CreateMap<Entities.Umbrella, Umbrella>();
	}
}