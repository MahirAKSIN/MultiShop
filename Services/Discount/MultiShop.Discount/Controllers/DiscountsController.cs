using Microsoft.AspNetCore.Mvc;
using MultiShop.Discount.Dtos;
using MultiShop.Discount.Services;

namespace MultiShop.Discount.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController : ControllerBase
    {
        private readonly IDiscountService _discountService;

        public DiscountsController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpGet]
        public async Task<IActionResult> DiscountCouponList()
        {
            var values = await _discountService.GetAllCouponAsync();
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdDiscoutCoupon(int id)
        {
            var values = await _discountService.GetByIdCouponDto(id);
            return Ok(values);

        }
        [HttpPost]
        public async Task<IActionResult> DiscountCreateCoupon(CreateCouponDto createCouponDto)
        {
            await _discountService.CreateCouponDtoAsync(createCouponDto);
            return Ok("Coupon Create Succes");
        }
        [HttpPut]
        public async Task<IActionResult> DiscountUodateCoupon(UpdateCouponDto updateCouponDto)
        {
            await _discountService.UpdateCouponDtoAsync(updateCouponDto);
            return Ok("Coupon Update Succes");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DiscountDeleteCoupon(int id)
        {
            await _discountService.DeleteCouponDtoAsync(id);
            return Ok("Coupon delete succes");

        }
    }
}
