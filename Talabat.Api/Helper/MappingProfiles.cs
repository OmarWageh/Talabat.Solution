using AutoMapper;
using Talabat.Api.Dtos;
using Talabat.Api.Dtos.Basket;
using Talabat.Api.Dtos.Order;
using Talabat.Api.Dtos.Product;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Api.Helper
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>().ForMember(d => d.productBrand, o => o.MapFrom(s => s.productBrand.Name)).
                ForMember(d => d.category, o => o.MapFrom(s => s.category.Name));
            CreateMap<CustomerBasket, CustomerBasketDto>().ReverseMap();
            CreateMap<BasketItem, BasketItemDto>().ReverseMap();
            CreateMap<AddressDto, Address>().ReverseMap();
            CreateMap<Product, CreateProductDto>().ReverseMap();
            CreateMap<Order, OrderToReturnDto>().ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName)).ForMember(d => d.DeliveryMethodCost, o => o.MapFrom(s => s.DeliveryMethod.ShortName));
            CreateMap<OrderItem, OrderItemDto>().ForMember(d => d.ProductName, o => o.MapFrom(s => s.product.ProductName)).ForMember(d => d.ProductId, o => o.MapFrom(s => s.product.ProductId)).ForMember(d => d.pictureUrl, o => o.MapFrom(s => s.product.PictureUrl));
            CreateMap<OrderItem, OrderItemDto>().ForMember(d => d.pictureUrl, o => o.MapFrom<OrderItemPictureUrlResolver>());
        }
    }
}
