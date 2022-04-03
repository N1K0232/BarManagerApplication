using AutoMapper;
using BackendGestionaleBar.Shared.Models;
using BackendGestionaleBar.Shared.Models.Requests;
using ApplicationProduct = BackendGestionaleBar.DataAccessLayer.Entities.Product;

namespace BackendGestionaleBar.BusinessLayer.MapperConfigurations
{
    public class ProductMapperProfile : Profile
    {
        public ProductMapperProfile()
        {
            CreateMap<ApplicationProduct, Product>();

            CreateMap<SaveProductRequest, ApplicationProduct>();
        }
    }
}