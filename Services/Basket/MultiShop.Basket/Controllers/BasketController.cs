using Microsoft.AspNetCore.Mvc;
using MultiShop.Basket.Dtos;
using MultiShop.Basket.NewFolder;
using MultiShop.Basket.Services;

namespace MultiShop.Basket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketServices _basketServices;
        private readonly ILoginService _loginService;

        public BasketController(IBasketServices basketServices, ILoginService loginService)
        {
            _basketServices = basketServices;
            _loginService = loginService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyBasketDetail()
        {
            var values = await _basketServices.GetBasket(_loginService.GetUserId);
            return Ok(values);
        }

        [HttpPost]
        public async Task<IActionResult> SaveMyBasket([FromBody] BasketTotalDto basketTotalDto)
        {
            basketTotalDto.UsreId = _loginService.GetUserId;
            basketTotalDto.BasketItems ??= new List<BasketItemDto>();

            await _basketServices.SaveBasket(basketTotalDto);
            return Ok("Basket add succes");
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveMyBasket()
        {
            await _basketServices.DeleteBasket(_loginService.GetUserId);
            return Ok("Delete basket succes");
        }
    }
}
