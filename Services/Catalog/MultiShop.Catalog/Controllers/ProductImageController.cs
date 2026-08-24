using Microsoft.AspNetCore.Mvc;
using MultiShop.Catalog.Dtos.ProductImageDtos;
using MultiShop.Catalog.Services.CategoryServices;

namespace MultiShop.Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImageController : ControllerBase
    {
        private readonly IProductImageServices _productImageServices;
        public ProductImageController(IProductImageServices productImageServices)
        {
            _productImageServices = productImageServices;
        }
        [HttpGet]
        public async Task<IActionResult> ProductImageList()
        {
            var values = await _productImageServices.GetAllProductImageAsync();
            return Ok(values);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByProductImageId(string id)
        {
            var values = await _productImageServices.GetByProductImageIdto(id);
            return Ok(values);
        }
        [HttpPost]
        public async Task<IActionResult> CreateProductImage(CreateProductImageDto createProductImageDto)
        {
            await _productImageServices.CreateProductImageAsync(createProductImageDto);
            return Ok("ProductImage create succes");
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteProductImage(string id)
        {
            await _productImageServices.DeleteProductImageAsync(id);
            return Ok("ProductImage deleted succes");
        }
        [HttpPut]
        public async Task<IActionResult> UpdateProductImage(UpdateProductImageDto updateProductImageDto)
        {
            await _productImageServices.UpdateProductImageAsync(updateProductImageDto);
            return Ok("Update ProductImage succes");

        }
    }
}
