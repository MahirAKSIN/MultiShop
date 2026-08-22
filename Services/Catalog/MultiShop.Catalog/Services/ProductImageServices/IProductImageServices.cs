using MultiShop.Catalog.Dtos.CategoryDtos;
using MultiShop.Catalog.Dtos.ProductDtos;
using MultiShop.Catalog.Dtos.ProductImageDtos;

namespace MultiShop.Catalog.Services.CategoryServices
{
    public interface IProductImageServices
    {
        Task<List<ResultProductImageDto>> GetAllProductImageAsync();
        Task CreateProductImageAsync(CreateProductImageDto createProductImageDto);
        Task UpdateProductImageAsync(UpdateProductImageDto updateProductImageDto );
        Task DeleteProductImageAsync(string id);
        Task<GetByIdProductImageDto> GetByProductImageIdto(string id);
    }
}
