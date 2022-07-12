using AutoMapper;
using BackendGestionaleBar.Shared.Models;
using Views = BackendGestionaleBar.DataAccessLayer.Views;

namespace BackendGestionaleBar.BusinessLayer.MapperProfiles;

internal class MenuMapperProfile : Profile
{
    public MenuMapperProfile()
    {
        CreateMap<Views.Menu, Menu>();
    }
}