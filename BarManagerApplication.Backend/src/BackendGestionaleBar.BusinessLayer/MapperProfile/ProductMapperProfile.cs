using AutoMapper;
using BackendGestionaleBar.Shared.Models;
using ApplicationProduct = BackendGestionaleBar.DataAccessLayer.Entities.Product;

namespace BackendGestionaleBar.BusinessLayer.MapperConfigurations
{
    public class ProductMapperProfile : Profile
    {
        public ProductMapperProfile()
        {
            CreateMap<ApplicationProduct, Product>();
        }
    }
}