using AutoMapper;
using MongoDB.Driver;
using MultiShop.Catalog.Dtos.ProductDetailDtos;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;

namespace MultiShop.Catalog.Services.CategoryServices
{
    public class ProductDetailService : IProductDetailService
    {
        private readonly IMongoCollection<ProductDetail> _productDetailCollection;
        private readonly IMapper _mapper;
        public ProductDetailService(IDatabaseSettings _databaseSettings, IMapper mapper)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _productDetailCollection = database.GetCollection<ProductDetail>(_databaseSettings.CategoryCollectionName);
            _mapper = mapper;
        }
        public async Task CreateProductDetailAsync(CreateProductDetailDto createProductDetailDto)
        {

            var values = _mapper.Map<ProductDetail>(createProductDetailDto);
            await _productDetailCollection.InsertOneAsync(values);

        }
        public async Task DeleteProductDetailAsync(string id)
        {
            await _productDetailCollection.DeleteOneAsync(q => q.ProductDetailId == id);
        }
        public async Task<List<ResultProductDetailDto>> GetAllProductsDetailAsync()
        {

            var values = await _productDetailCollection.Find(q => true).ToListAsync();
            return _mapper.Map<List<ResultProductDetailDto>>(values);

        }
        public async Task<GetByIdProductDetailDto> GetByProductDetailIdto(string id)
        {
            var values = await _productDetailCollection.Find(q => q.ProductDetailId == id).FirstOrDefaultAsync();

            return _mapper.Map<GetByIdProductDetailDto>(values);
        }
        public async Task UpdateProductDetailAsync(UpdateProductDetailDto updateProductDetailDto)
        {
            var values = _mapper.Map<ProductDetail>(updateProductDetailDto);

            await _productDetailCollection.FindOneAndReplaceAsync(Queryable => Queryable.ProductDetailId == updateProductDetailDto.ProductId, values);

        }
    }
}
