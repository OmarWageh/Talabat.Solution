using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Services.Contract
{
    public  interface IOrderService
    {
        public Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliverMethodId, Address address);
        public Task<IReadOnlyList<Order>> GetOrdersForUserAsync (string buyerEmail);
        public Task<Order> GetOrderByIdUserAsync(int orderId ,string  buyerEmail);
    }
}
 