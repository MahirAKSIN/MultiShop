using MultiShop.Catalog.Dtos.ProductDetailDtos;

namespace MultiShop.Catalog.Services.CategoryServices
{
    public interface IProductDetailService
    {
        Task<List<ResultProductDetailDto>> GetAllProductsDetailAsync();
        Task CreateProductDetailAsync(CreateProductDetailDto createProductDetailDto);
        Task UpdateProductDetailAsync(UpdateProductDetailDto updateProductDetailDto );
        Task DeleteProductDetailAsync(string id);
        Task<GetByIdProductDetailDto> GetByProductDetailIdto(string id);
    }
}
