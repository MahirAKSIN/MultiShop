# MultiShop

ASP.NET Core 9 mikroservis e-ticaret projesi. Catalog (MongoDB), Discount (Dapper + SQL Server) ve Order (CQRS + EF Core) servisleri içerir.

Repo: [github.com/MahirAKSIN/MultiShop](https://github.com/MahirAKSIN/MultiShop)

---

## Servisler

| Servis | Veri katmanı | Açıklama |
|---|---|---|
| **Catalog** | MongoDB | Kategori, ürün, ürün detayı, ürün görseli CRUD |
| **Discount** | Dapper + SQL Server | Kupon CRUD |
| **Order** | EF Core + CQRS | Adres, sipariş detayı (OrderDetail), sipariş (Ordering) |

Hedef framework: **.NET 9.0**

---

## 1. Catalog servisi

Katmanlı yapı: entity, DTO, AutoMapper, servis, controller. MongoDB üzerinde çalışır.

### Entity / koleksiyonlar

| Sınıf | Koleksiyon | Ana alanlar |
|---|---|---|
| `Category` | Categories | `CategoryId`, `CategoryName` |
| `Product` | Products | `ProductName`, `ProductPrice`, `ProductImageUrl`, `ProductDescription`, `CategoryId` |
| `ProductDetail` | ProductDetails | `ProductDescription`, `ProductInfo`, `ProductId` |
| `ProductImage` | ProductImages | `Images1`, `Images2`, `Images3`, `ProductId` |

`[BsonIgnoreExtraElements]` ile belgede fazla alan olsa bile deserialize patlamaz.

### DTO

- `Create*Dto` — ekleme (Id yok)
- `Update*Dto` — güncelleme (Id var)
- `Result*Dto` — liste
- `GetById*Dto` — tek kayıt

### Servisler

`ICategoryService`, `IProductServices`, `IProductDetailService`, `IProductImageServices` — her biri kendi MongoDB koleksiyonunu kullanır.

### API

| HTTP | Route | Açıklama |
|---|---|---|
| GET | `/api/Categories` | Liste |
| GET | `/api/Categories/{id}` | Tek kayıt |
| POST | `/api/Categories` | Ekle |
| PUT | `/api/Categories` | Güncelle |
| DELETE | `/api/Categories?id=` | Sil |

Aynı kalıp: `Product`, `ProductDetail`, `ProductImage`.

### Paketler

| Paket | Sürüm | Ne işe yarar? |
|---|---|---|
| AutoMapper | 16.2.0 | Entity ↔ DTO |
| MongoDB.Driver | 2.30.0 | MongoDB CRUD |
| MongoDB.Bson | 2.30.0 | BSON / `[BsonId]` |
| MongoDB.Driver.Core | 2.30.0 | Düşük seviye sürücü |
| Swashbuckle.AspNetCore | 9.0.6 | Swagger UI |
| Microsoft.AspNetCore.OpenApi | 9.0.17 | OpenAPI belgesi |

### Çalıştırma

```bash
dotnet run --project Services/Catalog/MultiShop.Catalog
```

- HTTPS: `https://localhost:7231`
- HTTP: `http://localhost:5149`
- Swagger: `https://localhost:7231/swagger`
- MongoDB: `mongodb://localhost:27017` / `MultiShopCatalogDb`

---

## 2. Discount servisi

SQL Server + Dapper ile kupon CRUD. EF Core yalnızca migration / tablo oluşturmak için kullanılır; runtime sorgular Dapper ile gider.

### Entity

`Coupon`: `CouponId`, `Code`, `Rate`, `IsActive`, `Validate`

### Yapı

- `DapperContext` — connection string + `createConnection()`
- `IDiscountService` / `DiscountService` — SQL CRUD
- `DiscountsController` — REST API

### API

| HTTP | Route |
|---|---|
| GET | `/api/Discounts` |
| GET | `/api/Discounts/{id}` |
| POST | `/api/Discounts` |
| PUT | `/api/Discounts` |
| DELETE | `/api/Discounts/{id}` |

### Paketler

| Paket | Sürüm | Ne işe yarar? |
|---|---|---|
| Dapper | 2.1.79 | Hafif SQL erişimi |
| Microsoft.EntityFrameworkCore | 9.0.19 | Migration / DbContext |
| Microsoft.EntityFrameworkCore.SqlServer | 9.0.19 | SQL Server provider |
| Microsoft.EntityFrameworkCore.Design / Tools | 9.0.19 | `Add-Migration`, `Update-Database` |
| Swashbuckle.AspNetCore | 9.0.6 | Swagger UI |
| Microsoft.AspNetCore.OpenApi | 9.0.17 | OpenAPI |

> Not: EF Core **10.x** yalnızca `net10.0` ile uyumludur. Proje `net9.0` olduğu için **9.0.x** kullanılmalıdır.

### Çalıştırma

```bash
dotnet run --project Services/Discount/MultiShop.Discount
```

- HTTPS: `https://localhost:7108`
- HTTP: `http://localhost:5259`
- Swagger kök adreste açılır
- DB: `MultiShopDiscount` (SQL Server, Windows auth)

Migration:

```powershell
Add-Migration mig1 -StartupProject MultiShop.Discount -Project MultiShop.Discount
Update-Database -StartupProject MultiShop.Discount -Project MultiShop.Discount
```

---

## 3. Order servisi

Clean Architecture + CQRS. Katmanlar:

| Proje | Rol |
|---|---|
| `MultiShop.Order.Domain` | Entity’ler: `Address`, `Ordering`, `OrderDetail` |
| `MultiShop.Order.Application` | CQRS Commands / Queries / Handlers / Results, `IRepository` |
| `MultiShop.Order.Infrastructure` | `OrderContext` (EF Core), `Repository` |
| `MultiShop.Order.Presention` | API controller’lar |

### Domain

- **Address:** `AddressId`, `UserId`, `District`, `City`, `Detail`
- **Ordering:** `OrderingId`, `UserId`, `TotalPrice`, `OrderDate`, `OrderDetails`
- **OrderDetail:** `OrderDetailId`, `ProductId`, `ProductName`, `ProductPrice`, `ProductAmount`, `ProductTotalPrice`, `OrderingId`

### CQRS klasör yapısı

```
Features/CQRS/
├── Commands/
│   ├── AddressCommands/
│   ├── OrderDetailCommands/
│   └── OrderingCommands/
├── Handlers/
│   ├── AddressHandlers/
│   ├── OrderDetailHandlers/
│   └── OrderingHandlers/
├── Queries/
│   ├── AddressQueries/
│   ├── OrderDetailQueries/
│   └── OrderingQueries/
└── Results/
    ├── AddressResults/
    ├── OrderDetailResults/
    └── OrderingResults/
```

Her kaynak için tipik dosyalar:

- Commands: Create / Update / Remove (Delete)
- Queries: GetById (+ liste query)
- Results: Get*QueryResult, Get*ByIdQueryResult
- Handlers: Create / Update / Remove / GetAll / GetById

### Infrastructure

- `OrderContext` — EF Core `DbContext`
- `Repository<T>` — `IRepository<T>` implementasyonu (`GetAllAsync`, `GetByIdAsync`, `CreateAsync`, `UpdateAsync`, `DeleteAsync`, `GetByFilterAsync`)

### API (Presention)

- `AddressesController`
- `OrderDetailController`

### Paketler

| Paket | Sürüm | Ne işe yarar? |
|---|---|---|
| Microsoft.EntityFrameworkCore | 9.0.0 | ORM |
| Microsoft.EntityFrameworkCore.SqlServer | 9.0.0 | SQL Server |
| Microsoft.EntityFrameworkCore.Design / Tools | 9.0.0 | Migration araçları |
| Microsoft.AspNetCore.OpenApi | 9.0.17 | OpenAPI (Presention) |

### Çalıştırma

```bash
dotnet run --project Services/Order/Presention/MultiShop.Order.Presention
```

---

## Klasör yapısı

```
MultiShop/
├── MultiShop.sln
├── README.md
└── Services/
    ├── Catalog/MultiShop.Catalog/
    ├── Discount/MultiShop.Discount/
    └── Order/
        ├── Core/
        │   ├── MultiShop.Order.Domain/
        │   ├── MultiShop.Order.Application/
        │   ├── MultiShop.Order.Infrastructure/
        │   └── MultiShop.Order.WebApi/
        └── Presention/MultiShop.Order.Presention/
```

---

## Karşılaşılan hatalar ve çözümleri

| Hata | Neden | Çözüm |
|---|---|---|
| `Unable to resolve IDatabaseSettings` | DI’ye arayüz kaydedilmemişti | `AddSingleton<IDatabaseSettings>` |
| `ReflectionTypeLoadException` | AutoMapper `AddMaps` tüm assembly’yi tarıyordu | `AddProfile<GeneralMapping>()` |
| `UseSwagger` / `AddSwaggerGen` CS1061 | Swashbuckle paketi yoktu | `Swashbuckle.AspNetCore 9.0.6` |
| `Element 'ProductImageUrl' does not match ProductDetail` | Yanlış MongoDB koleksiyonu | Her servise kendi koleksiyon adı |
| `Element 'ProductName' does not match Category` | Eski karışık belgeler | `[BsonIgnoreExtraElements]` + filtre |
| EF Core 10.x restore hatası | `net9.0` ile uyumsuz | EF Core **9.0.x** kullan |
| Migration Startup Project hatası | Catalog seçiliydi | Startup: `MultiShop.Discount` |
| Swagger conflicting GET path | İki `[HttpGet]` aynı route | `GET {id}` ayır |
| `Cannot perform runtime binding on a null reference` | Dapper generic tip yoktu | `QueryFirstOrDefaultAsync<T>` |
