using AutoMapper;
using Talabat.Api.Dtos.Order;
using Talabat.Api.Dtos.Product;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Api.Helper
{
    public class OrderItemPictureUrlResolver:IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.product.PictureUrl))

                return $"{_configuration["ApiBaseUrl"]}/{source.product.PictureUrl}";
            return string.Empty;
        }
        }
    }
