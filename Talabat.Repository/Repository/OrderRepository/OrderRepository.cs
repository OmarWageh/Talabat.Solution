using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository.Data;

namespace Talabat.Repository.Repository.OrderRepository
{
    public class OrderRepository:GenericRepository<Order>,IOrderRepository

    {
        private readonly AppDbContext _appDbContext;

        public OrderRepository(AppDbContext appDbContext):base (appDbContext)
        {
          _appDbContext = appDbContext;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var GetOrdersBybuyerEmail = await _appDbContext.Orders.Where(o => o.BuyerEmail == buyerEmail).Include(o => o.DeliveryMethod).Include(o => o.OrderItems).OrderByDescending(o => o.OrderDate).ToListAsync();
            return GetOrdersBybuyerEmail;
        }
        public async Task<Order?> GetSpecificOrderForSpecificUser(int id, string buyerEmail)
        {
            var order = await _appDbContext.Orders

.Include(o => o.DeliveryMethod)
.Include(o => o.OrderItems)
.FirstOrDefaultAsync(o => o.BuyerEmail == buyerEmail && o.Id == id);

            return order;

        }

    }
}
