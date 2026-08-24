# MultiShop

ASP.NET Core 9 mikroservis e-ticaret projesi. Şu an **Catalog** servisi hazır: MongoDB üzerinde kategori, ürün, ürün detayı ve ürün görseli için REST API.

Repo: [github.com/MahirAKSIN/MultiShop](https://github.com/MahirAKSIN/MultiShop)

---

## Ne yapıldı?

Catalog servisi katmanlı yapıyla kuruldu: entity, DTO, AutoMapper, servis, controller.

### 1. Entity katmanı

MongoDB belgelerini temsil eder. `_id` alanı `ObjectId` olarak tutulur.

| Sınıf | Koleksiyon | Ana alanlar |
|---|---|---|
| `Category` | Categories | `CategoryId`, `CategoryName` |
| `Product` | Products | `ProductName`, `ProductPrice`, `ProductImageUrl`, `ProductDescription`, `CategoryId` |
| `ProductDetail` | ProductDetails | `ProductDescription`, `ProductInfo`, `ProductId` |
| `ProductImage` | ProductImages | `Images1`, `Images2`, `Images3`, `ProductId` |

`[BsonIgnoreExtraElements]` eklendi. Belgede sınıfta olmayan bir alan varsa deserialize hatası vermez.

Navigation property’ler (`Category`, `Product`) `[BsonIgnore]` ile işaretli; MongoDB’ye yazılmaz.

### 2. DTO katmanı

API entity’yi dışarı açmaz. Her işlem için ayrı DTO vardır:

- `Create*Dto` — ekleme (Id yok, MongoDB üretir)
- `Update*Dto` — güncelleme (Id var)
- `Result*Dto` — liste
- `GetById*Dto` — tek kayıt

### 3. AutoMapper (`GeneralMapping`)

Entity ile DTO arasında dönüşüm `GeneralMapping` profilinde tanımlı. Örnek:

```csharp
var values = _mapper.Map<Product>(createProductDto);
await _productCollection.InsertOneAsync(values);
```

`CreateProductDto` → `Product` map edilir, sonra MongoDB’ye eklenir. `ProductId` DTO’da yoktur; insert sırasında üretilir.

### 4. Servis katmanı (CRUD)

Her kaynak için interface + implementation:

- `ICategoryService` / `CategoryService`
- `IProductServices` / `ProductServices`
- `IProductDetailService` / `ProductDetailService`
- `IProductImageServices` / `ProductImageServices`

Metotlar:

| Metot | İş |
|---|---|
| `GetAll*Async` | Tüm kayıtlar |
| `GetBy*Id` | Id ile tek kayıt |
| `Create*Async` | `InsertOneAsync` |
| `Update*Async` | `FindOneAndReplaceAsync` |
| `Delete*Async` | `DeleteOneAsync` |

Her servis kendi koleksiyonunu kullanır (`ProductCollectionName`, `ProductDetailCollectionName`, …). Hepsi `Categories` koleksiyonuna yazılırsa belgeler karışır ve `FormatException` oluşur.

### 5. Controller katmanı

Hepsi `CategoriesController` ile aynı kalıpta:

- `GET /api/{controller}` — liste
- `GET /api/{controller}/{id}` — tek kayıt
- `POST /api/{controller}` — ekle
- `PUT /api/{controller}` — güncelle
- `DELETE /api/{controller}?id=` — sil

Controller’lar:

- `CategoriesController` → `/api/Categories`
- `ProductController` → `/api/Product`
- `ProductDetailController` → `/api/ProductDetail`
- `ProductImageController` → `/api/ProductImage`

### 6. Dependency Injection (`Program.cs`)

- Servisler `AddScoped` ile kaydedildi
- `DatabaseSetting` section `IDatabaseSettings` olarak `AddSingleton` ile bağlandı (yoksa `Unable to resolve IDatabaseSettings` hatası)
- AutoMapper yalnızca `GeneralMapping` profilini yükler (`AddMaps` ile tüm assembly taranmaz; `ReflectionTypeLoadException` olmasın diye)
- Swagger: `AddSwaggerGen` + `UseSwagger` / `UseSwaggerUI`

### 7. MongoDB ayarları

`appsettings.json`:

```json
"DatabaseSetting": {
  "CategoryCollectionName": "Categories",
  "ProductCollectionName": "Products",
  "ProductDetailCollectionName": "ProductDetails",
  "ProductImageCollectionName": "ProductImages",
  "ConnectionString": "mongodb://localhost:27017",
  "DatabaseName": "MultiShopCatalogDb"
}
```

---

## Kullanılan paketler

Hedef framework: **.NET 9.0**

| Paket | Sürüm | Ne işe yarar? |
|---|---|---|
| **AutoMapper** | 16.2.0 | Entity ↔ DTO dönüşümü. `GeneralMapping` profili `Program.cs` içinde `AddProfile` ile kaydedilir. |
| **MongoDB.Driver** | 2.30.0 | Ana C# sürücüsü. `MongoClient`, `IMongoCollection`, `InsertOneAsync`, `Find`, `DeleteOneAsync`, `FindOneAndReplaceAsync`. |
| **MongoDB.Bson** | 2.30.0 | BSON serileştirme. `[BsonId]`, `[BsonRepresentation]`, `[BsonIgnore]`, `[BsonIgnoreExtraElements]`. `MongoDB.Driver` ile birlikte gelir; açık referans da duruyor. |
| **MongoDB.Driver.Core** | 2.30.0 | Düşük seviye sürücü (bağlantı, wire protocol). `MongoDB.Driver` bağımlılığıdır. |
| **Swashbuckle.AspNetCore** | 9.0.6 | Swagger UI. Endpoint’leri tarayıcıdan denemek için `/swagger`. .NET 9 ile uyumlu 9.x kullanıldı (10.x ASP.NET Core 10 içindir). |
| **Microsoft.AspNetCore.OpenApi** | 9.0.17 | .NET 9 yerleşik OpenAPI belgesi (`AddOpenApi` / `MapOpenApi`). Swagger’a ek olarak OpenAPI JSON üretir. |

SDK’dan gelenler (NuGet’te ayrıca yok):

- `Microsoft.NET.Sdk.Web` — Kestrel, MVC controllers, DI, `WebApplication`
- `Microsoft.Extensions.Options` — `IOptions<DatabaseSettings>` ile config binding

---

## Klasör yapısı

```
MultiShop/
├── MultiShop.sln
└── Services/Catalog/MultiShop.Catalog/
    ├── Controllers/          REST endpoint’ler
    ├── Dtos/                 API modelleri
    ├── Entities/             MongoDB belgeleri
    ├── Mapping/              AutoMapper profili
    ├── Services/             İş kuralları + MongoDB CRUD
    ├── Settings/             IDatabaseSettings
    ├── Program.cs
    └── appsettings.json
```

---

## API özeti

Tüm controller’larda aynı HTTP fiilleri:

| HTTP | Route | Açıklama |
|---|---|---|
| GET | `/api/Categories` | Kategori listesi |
| GET | `/api/Categories/{id}` | Tek kategori |
| POST | `/api/Categories` | Kategori ekle |
| PUT | `/api/Categories` | Kategori güncelle |
| DELETE | `/api/Categories?id=` | Kategori sil |

Aynı kalıp `Product`, `ProductDetail`, `ProductImage` için geçerlidir.

Örnek ürün ekleme gövdesi:

```json
{
  "productName": "Kablosuz Mouse",
  "productPrice": 799.90,
  "productImageUrl": "/images/mouse.jpg",
  "productDescription": "Bluetooth 5.0",
  "categoryId": "66c7abcd1234567890abcdef"
}
```

---

## Çalıştırma

1. MongoDB’nin `localhost:27017` üzerinde açık olması gerekir.
2. Projeyi çalıştır:

```bash
dotnet run --project Services/Catalog/MultiShop.Catalog
```

3. Adresler (`launchSettings.json`):
   - HTTPS: `https://localhost:7231`
   - HTTP: `http://localhost:5149`
   - Swagger UI: `https://localhost:7231/swagger`

---

## Karşılaşılan hatalar ve çözümleri

| Hata | Neden | Çözüm |
|---|---|---|
| `Unable to resolve IDatabaseSettings` | DI’ye arayüz kaydedilmemişti | `AddSingleton<IDatabaseSettings>` |
| `ReflectionTypeLoadException` | AutoMapper `AddMaps` tüm assembly’yi tarıyordu | `AddProfile<GeneralMapping>()` |
| `UseSwagger` / `AddSwaggerGen` CS1061 | Swashbuckle paketi yoktu | `Swashbuckle.AspNetCore 9.0.6` |
| `Element 'ProductImageUrl' does not match ProductDetail` | Tüm servisler `Categories` koleksiyonunu kullanıyordu | Her servise kendi koleksiyon adı |
| `Element 'ProductName' does not match Category` | Eski ürün belgeleri `Categories` içinde kalmıştı | `[BsonIgnoreExtraElements]` + `CategoryName` filtresi |

Eski karışık belgeler hâlâ `Categories` koleksiyonunda duruyor olabilir. Yeni ürünler `Products` koleksiyonuna yazılır.
