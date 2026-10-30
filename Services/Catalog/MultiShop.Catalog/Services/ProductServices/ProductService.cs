using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using MultiShop.Catalog.Dtos.ProductDtos;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;

namespace MultiShop.Catalog.Services.CategoryServices
{
    public class ProductServices : IProductServices
    {
        private readonly IMongoCollection<Product> _productCollection;
        private readonly IMapper _mapper;

        public ProductServices(IDatabaseSettings _databaseSettings, IMapper mapper)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _productCollection = database.GetCollection<Product>(_databaseSettings.ProductCollectionName);
            _mapper = mapper;
        }

        public async Task CreateProductAsync(CreateProductDto createProductDto)
        {
            var values = _mapper.Map<Product>(createProductDto);
            values.ProductId = null;
            await _productCollection.InsertOneAsync(values);
        }

        public async Task DeleteProductAsync(string id)
        {
            var filter = BuildProductIdFilter(id);
            await _productCollection.DeleteOneAsync(filter);
        }

        public async Task<List<ResultProductDto>> GetAllProductsAsync()
        {
            var values = await _productCollection.Find(q => true).ToListAsync();
            return _mapper.Map<List<ResultProductDto>>(values);
        }

        public async Task<GetByIdProductDto> GetByProductIdto(string id)
        {
            var filter = BuildProductIdFilter(id);
            var values = await _productCollection.Find(filter).FirstOrDefaultAsync();
            return _mapper.Map<GetByIdProductDto>(values);
        }

        public async Task UpdateProductAsync(UpdateProductDto updateProductDto)
        {
            var values = _mapper.Map<Product>(updateProductDto);
            values.ProductId = updateProductDto.ProductId;
            var filter = BuildProductIdFilter(updateProductDto.ProductId);
            await _productCollection.FindOneAndReplaceAsync(filter, values);
        }

        private static FilterDefinition<Product> BuildProductIdFilter(string? id)
        {
            if (!string.IsNullOrWhiteSpace(id) && ObjectId.TryParse(id, out var objectId))
            {
                return Builders<Product>.Filter.Eq("_id", objectId);
            }

            return Builders<Product>.Filter.Eq(x => x.ProductId, id);
        }
    }
}
