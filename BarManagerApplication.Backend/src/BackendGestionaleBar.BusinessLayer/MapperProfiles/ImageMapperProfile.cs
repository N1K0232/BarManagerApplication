using AutoMapper;
using BackendGestionaleBar.Shared.Models;
using Entities = BackendGestionaleBar.DataAccessLayer.Entities;

namespace BackendGestionaleBar.BusinessLayer.MapperProfiles;

internal class ImageMapperProfile : Profile
{
	public ImageMapperProfile()
	{
		CreateMap<Entities.Image, Image>();
	}
}