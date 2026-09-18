using MultiShop.Basket.Dtos;
using MultiShop.Basket.Settings;
using System.Text.Json;

namespace MultiShop.Basket.Services
{
    public class BasketService : IBasketServices
    {

        private readonly RedisService _redisService;

        public BasketService(RedisService redisService)
        {
            _redisService = redisService;
        }

        public async Task DeleteBasket(string userId)
        {
            await _redisService.Getdb().KeyDeleteAsync(userId);
        }

        public async Task<BasketTotalDto?> GetBasket(string userId)
        {
            var values = await _redisService.Getdb().StringGetAsync(userId);
            if (values.IsNullOrEmpty)
                return null;

            return JsonSerializer.Deserialize<BasketTotalDto>(values!);
        }

        public async Task<bool> SaveBasket(BasketTotalDto basket)
        {
            return await _redisService.Getdb().StringSetAsync(basket.UsreId, JsonSerializer.Serialize(basket));
        }
    }
}
