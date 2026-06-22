using Microsoft.EntityFrameworkCore.Storage;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using IDatabase = StackExchange.Redis.IDatabase;


namespace Talabat.Repository.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;

        public BasketRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task<bool> DeleteBasketAsync(string Basketid)
        {
            return await  _database.KeyDeleteAsync(Basketid);
        }

        public async Task<CustomerBasket?> GetBasketAsync(string Basketid)
        {
            var basket=await _database.StringGetAsync(Basketid);
            if (basket.IsNullOrEmpty)
                return null;
            return JsonSerializer.Deserialize<CustomerBasket>(basket);
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
           var createdOrUpdated= await _database.StringSetAsync(basket.Id,JsonSerializer.Serialize(basket),TimeSpan.FromDays(30));
            if (createdOrUpdated is false )
                return  null;
            return await GetBasketAsync(basket.Id);
        }
    }
}
