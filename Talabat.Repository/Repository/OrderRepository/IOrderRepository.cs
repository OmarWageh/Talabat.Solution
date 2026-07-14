
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Entities.Order_Aggregate;
namespace Talabat.Repository.Repository.OrderRepository
{
    public interface IOrderRepository:IGenericRepository<Order>
    {
        public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail);
        public Task<Order?> GetSpecificOrderForSpecificUser(int id, string buyerEmail);

    }
}
