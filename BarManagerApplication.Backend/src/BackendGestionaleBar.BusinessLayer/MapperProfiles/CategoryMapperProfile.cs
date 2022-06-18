using AutoMapper;
using BackendGestionaleBar.Shared.Models;
using BackendGestionaleBar.Shared.Models.Requests;
using Entities = BackendGestionaleBar.DataAccessLayer.Entities;

namespace BackendGestionaleBar.BusinessLayer.MapperProfiles;

internal class CategoryMapperProfile : Profile
{
    public CategoryMapperProfile()
    {
        CreateMap<Entities.Category, Category>();
        CreateMap<SaveCategoryRequest, Entities.Category>();
    }
}