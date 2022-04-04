using AutoMapper;
using BackendGestionaleBar.Shared.Models;
using BackendGestionaleBar.Shared.Models.Requests;
using ApplicationCategory = BackendGestionaleBar.DataAccessLayer.Entities.Category;

namespace BackendGestionaleBar.BusinessLayer.MapperProfile
{
    public class CategoryMapperProfile : Profile
    {
        public CategoryMapperProfile()
        {
            CreateMap<ApplicationCategory, Category>();

            CreateMap<SaveCategoryRequest, ApplicationCategory>();
        }
    }
}