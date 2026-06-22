using AutoMapper;
using Talabat.Api.Dtos.Basket;
using Talabat.Api.Dtos.Product;
using Talabat.Core.Entities;

namespace Talabat.Api.Helper
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>().ForMember(d => d.productBrand, o => o.MapFrom(s => s.productBrand.Name)).
                ForMember(d => d.category, o => o.MapFrom(s => s.category.Name));
            CreateMap<CustomerBasket, CustomerBasketDto>();
            CreateMap<BasketItem, BasketItemDto>();
        }
    }
}
