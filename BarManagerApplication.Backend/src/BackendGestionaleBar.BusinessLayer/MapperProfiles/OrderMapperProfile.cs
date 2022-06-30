using AutoMapper;
using BackendGestionaleBar.Shared.Models;
using Entities = BackendGestionaleBar.DataAccessLayer.Entities;

namespace BackendGestionaleBar.BusinessLayer.MapperProfiles;

internal class OrderMapperProfile : Profile
{
	public OrderMapperProfile()
	{
		CreateMap<Entities.Order, Order>();
	}
}