using AutoMapper;
using BackendGestionaleBar.Shared.Models;
using ApplicationCategory = BackendGestionaleBar.DataAccessLayer.Entities.Category;

namespace BackendGestionaleBar.BusinessLayer.MapperProfile
{
    public class CategoryMapperProfile : Profile
    {
        public CategoryMapperProfile()
        {
            CreateMap<ApplicationCategory, Category>();
        }
    }
}