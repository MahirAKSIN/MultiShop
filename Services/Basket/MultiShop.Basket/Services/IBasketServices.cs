using MultiShop.Basket.Dtos;

namespace MultiShop.Basket.Services
{
    public interface IBasketServices
    {

        Task<BasketTotalDto?> GetBasket(string userId);
        Task<bool> SaveBasket(BasketTotalDto basket);
        Task DeleteBasket(string userId);

    }
}
