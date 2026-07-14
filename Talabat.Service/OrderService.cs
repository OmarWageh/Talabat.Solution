using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Repository.Repository;
using Talabat.Repository.Repository.OrderRepository;

namespace Talabat.Service
{
    //Business
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitofWork;
        private readonly IOrderRepository _orderRepository;

        public OrderService(IBasketRepository basketRepo, IUnitOfWork unitOfWork,IOrderRepository orderRepository)
        {
            _basketRepo = basketRepo;
            _unitofWork = unitOfWork;
            _orderRepository = orderRepository;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliverMethodId, Address ShippingAddress)
        {
            // 1- Get Basket From Baskets Repostory
            var basket =await  _basketRepo.GetBasketAsync(basketId);
            // 2- Get Selected Items at Basket From Products Repo 
            var orderItems = new List<OrderItem>();

            if(basket?.Items?.Count>0)
            {
                var ProductRepository =_unitofWork.Repository<Product>();
                foreach (var item in basket.Items)
                {
                    var product = await ProductRepository.GetAsync(item.Id);
                    var productItemOrdered = new ProductItemOrdered(item.Id, product.Name, product.PictureUrl);
                    var orderItem = new OrderItem(productItemOrdered, product.Price, item.Quantity);
                    orderItems.Add(orderItem);
                }
            }
            // 3-  Calculate SubTotal
            var SubTotal = orderItems.Sum(orderItem => orderItem.Price * orderItem.Quantity);
            //4- Get Delivery Method From DeliveryMethods Repo
            var deliveryMethod = await _unitofWork.Repository<DeliveryMethod>().GetAsync(deliverMethodId);
            // 5- Creat Order 
              var order = new Order(buyerEmail,ShippingAddress,deliveryMethod,orderItems,SubTotal);
               await _unitofWork.Repository<Order>().AddAsync(order);
            // 6- Save To DataBase 
            var result = await _unitofWork.CompleteAsync();
            if (result <= 0)
                return null;
            return order;
        }
        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var orderRepo =await  _orderRepository.GetOrdersForUserAsync(buyerEmail);
            return orderRepo;
        }
        public async Task<Order?> GetOrderByIdUserAsync(int orderId, string buyerEmail)
        {
            var orderRepo = await _orderRepository.GetSpecificOrderForSpecificUser(orderId, buyerEmail);
            if (orderRepo is null)
            {
                throw new Exception("The order is Empty");
            }
            return orderRepo;
        }

        
    }
}
