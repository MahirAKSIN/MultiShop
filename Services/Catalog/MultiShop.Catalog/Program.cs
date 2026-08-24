using Microsoft.Extensions.Options;
using MultiShop.Catalog.Mapping;
using MultiShop.Catalog.Services.CategoryServices;
using MultiShop.Catalog.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductServices, ProductServices>();
builder.Services.AddScoped<IProductDetailService, ProductDetailService>();
builder.Services.AddScoped<IProductImageServices, ProductImageServices>();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<GeneralMapping>();
});

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSetting"));
builder.Services.AddSingleton<IDatabaseSettings>(sp =>
{
    return sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
