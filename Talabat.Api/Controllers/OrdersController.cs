using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Talabat.Api.Dtos;
using Talabat.Api.Dtos.Order;
using Talabat.Api.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Services.Contract;
using Order = Talabat.Core.Entities.Order_Aggregate.Order;


namespace Talabat.Api.Controllers
{

    public class OrdersController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        public OrdersController(IOrderService orderService,IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }
        [HttpPost] //post: api/orders
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseToNotFound_Badrequest_Unauthorized), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var addrerss = _mapper.Map<AddressDto, Address>(orderDto.ShippingAddress);
            var order = await _orderService.CreateOrderAsync(orderDto.BuyerEmail, orderDto.BasketId, orderDto.DeliveryMethodId, addrerss);
                if (order is null)

                return BadRequest(new ApiResponseToNotFound_Badrequest_Unauthorized(400));

            return order;
        }
        [HttpGet]  // Get: url/api/Orders?email=omarwagih815@gmail.com
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseToNotFound_Badrequest_Unauthorized), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Order>> GetOrdersToSpecificUser(string BuyerEmail)
        {
            var getOrderToSpecficUser = await _orderService.GetOrdersForUserAsync(BuyerEmail);
            return Ok(getOrderToSpecficUser);
        }
        [HttpGet("{id}")]  // Get:Url/api/Orders/id?email=omarwagih815@gmail.com
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseToNotFound_Badrequest_Unauthorized), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Order>>GetSpecificOrderToSpecificUser(int id , [FromQuery] string Email)
        {

            var order = await _orderService.GetOrderByIdUserAsync(id, Email);
            if (order is null)
                return NotFound(new ApiResponseToNotFound_Badrequest_Unauthorized(404));

            return Ok(order);
        }
    }
}





