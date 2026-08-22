using AutoMapper;
using MongoDB.Driver;
using MultiShop.Catalog.Dtos.ProductDtos;
using MultiShop.Catalog.Dtos.ProductImageDtos;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;
using static MongoDB.Driver.WriteConcern;

namespace MultiShop.Catalog.Services.CategoryServices
{
    public class ProductImageServices : IProductImageServices
    {
        private readonly IMongoCollection<ProductImage> _productImageCollection;
        private readonly IMapper _mapper;


        public ProductImageServices(IDatabaseSettings _databaseSettings, IMapper mapper)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _productImageCollection = database.GetCollection<ProductImage>(_databaseSettings.CategoryCollectionName);
            _mapper = mapper;
        }
        public async Task CreateProductImageAsync(CreateProductImageDto createProductImageDto)
        {
            var values = _mapper.Map<ProductImage>(createProductImageDto);
            await _productImageCollection.InsertOneAsync(values);
        }
        public async Task DeleteProductImageAsync(string id)
        {
            await _productImageCollection.DeleteOneAsync(q => q.ProductImageId == id);
        }
        public async Task<List<ResultProductImageDto>> GetAllProductImageAsync()
        {
            var values = await _productImageCollection.Find(q => true).ToListAsync();
            return _mapper.Map<List<ResultProductImageDto>>(values);
        }
        public async Task<GetByIdProductImageDto> GetByProductImageIdto(string id)
        {
            var values = await _productImageCollection.Find(q => q.ProductImageId == id).FirstOrDefaultAsync();
            return _mapper.Map<GetByIdProductImageDto>(values);
        }
        public async Task UpdateProductImageAsync(UpdateProductImageDto updateProductImageDto)
        {
            var values=_mapper.Map<ProductImage>(updateProductImageDto);
          await _productImageCollection.FindOneAndReplaceAsync(q => q.ProductImageId == updateProductImageDto.ProductImageId, values);
        }
    }
}
