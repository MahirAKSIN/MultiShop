using MultiShop.Discount.Dtos;

namespace MultiShop.Discount.Services
{
    public interface IDiscountService
    {
        Task<List<ResultCouponDto>> GetAllCouponAsync();
        Task CreateCouponDtoAsync(CreateCouponDto createCouponDto);
        Task UpdateCouponDtoAsync(UpdateCouponDto updateCouponDto);
        Task DeleteCouponDtoAsync(int id);
        Task<GetByIdCouponDto> GetByIdCouponDto(int id);
    }
}
