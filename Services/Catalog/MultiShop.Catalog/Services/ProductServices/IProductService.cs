using MultiShop.Catalog.Dtos.CategoryDtos;
using MultiShop.Catalog.Dtos.ProductDtos;

namespace MultiShop.Catalog.Services.CategoryServices
{
    public interface IProductServices
    {
        Task<List<ResultProductDto>> GetAllProductsAsync();
        Task CreateProductAsync(CreateProductDto createProductDto);
        Task UpdateProductAsync(UpdateProductDto updateProductDto );
        Task DeleteProductAsync(string id);
        Task<GetByIdProductDto> GetByProductIdto(string id);
    }
}
