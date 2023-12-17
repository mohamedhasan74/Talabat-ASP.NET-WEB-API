using StackExchange.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.IRepository;

namespace Talabat.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
        }
        public async Task<bool> DeleteBasketAsync(string id)
        {
            var delBasket = await _database.KeyDeleteAsync(id);
            return delBasket;
        }

        public async Task<CustomerBasket?> GetBasketAsync(string id)
        {
            var basket = await _database.StringGetAsync(id);
            return basket.IsNull ? null : JsonSerializer.Deserialize<CustomerBasket>(basket);
        }

        public async Task<CustomerBasket?> UpdateOrCreateBasketAsync(CustomerBasket basket)
        {
            var basketJason = JsonSerializer.Serialize(basket);
            var updateOrCreatedBasket = await _database.StringSetAsync(basket.Id, basketJason, TimeSpan.FromDays(14));
            if (updateOrCreatedBasket is false) return null;
            return await GetBasketAsync(basket.Id);
        }
    }
}
