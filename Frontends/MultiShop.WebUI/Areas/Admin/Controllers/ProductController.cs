using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MultiShop.DtoLayer.CatalogDtos.CategoryDtos;
using MultiShop.DtoLayer.ProductsDtos;
using Newtonsoft.Json;
using System.Text;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Product")]
    [AllowAnonymous]
    public class ProductController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            ViewBag.v1 = "AnaSayfa";
            ViewBag.v2 = "Products";
            ViewBag.v3 = "Products listesi";
            ViewBag.v0 = "Products Islemleri";

            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7070/api/Products");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultProductDto>>(jsonData);
                return View(values ?? new List<ResultProductDto>());
            }

            return View(new List<ResultProductDto>());
        }

        [Route("ProductWithCategory")]
        public async Task<IActionResult> ProductWithCategory()
        {
            ViewBag.v1 = "AnaSayfa";
            ViewBag.v2 = "Products";
            ViewBag.v3 = "Products listesi";
            ViewBag.v0 = "Products Islemleri";

            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7070/api/Products/ProductWithCategory");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultProductWithCategoryDto>>(jsonData);
                return View(values ?? new List<ResultProductWithCategoryDto>());
            }

            return View(new List<ResultProductWithCategoryDto>());
        }
        [HttpGet]
        [Route("CreateProduct")]
        public async Task<IActionResult> CreateProduct()
        {
            ViewBag.v1 = "AnaSayfa";
            ViewBag.v2 = "Products";
            ViewBag.v3 = "Yeni Product Girisi";
            ViewBag.v0 = "Products Islemleri";
            await LoadCategoriesAsync();
            return View(new CreateProductDto());
        }

        [HttpPost]
        [Route("CreateProduct")]
        public async Task<IActionResult> CreateProduct(CreateProductDto createProductDto)
        {
            createProductDto.ProductImageUrl ??= string.Empty;
            createProductDto.ProductDescription ??= string.Empty;

            if (string.IsNullOrWhiteSpace(createProductDto.ProductName))
            {
                ModelState.AddModelError(nameof(createProductDto.ProductName), "Urun adi zorunludur.");
            }

            if (string.IsNullOrWhiteSpace(createProductDto.CategoryId))
            {
                ModelState.AddModelError(nameof(createProductDto.CategoryId), "Kategori secimi zorunludur.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.v1 = "AnaSayfa";
                ViewBag.v2 = "Products";
                ViewBag.v3 = "Yeni Product Girisi";
                ViewBag.v0 = "Products Islemleri";
                await LoadCategoriesAsync();
                return View(createProductDto);
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                var jsonData = JsonConvert.SerializeObject(createProductDto);
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var responseMessage = await client.PostAsync("https://localhost:7070/api/Products", content);
                if (responseMessage.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Product", new { area = "Admin" });
                }

                var errorBody = await responseMessage.Content.ReadAsStringAsync();
                ViewBag.error = "Urun eklenemedi. API durum kodu: " + (int)responseMessage.StatusCode + " " + errorBody;
            }
            catch (Exception ex)
            {
                ViewBag.error = "Catalog API'ye ulasilamadi (https://localhost:7070). Catalog projesini https profiliyle baslatin. " + ex.Message;
            }

            ViewBag.v1 = "AnaSayfa";
            ViewBag.v2 = "Products";
            ViewBag.v3 = "Yeni Product Girisi";
            ViewBag.v0 = "Products Islemleri";
            await LoadCategoriesAsync();
            return View(createProductDto);
        }

        private async Task LoadCategoriesAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7070/api/Categories");
            if (!responseMessage.IsSuccessStatusCode)
            {
                ViewBag.categoryValue = new List<SelectListItem>();
                return;
            }

            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultCategoryDto>>(jsonData) ?? new List<ResultCategoryDto>();
            ViewBag.categoryValue = values.Select(x => new SelectListItem
            {
                Text = x.CategoryName,
                Value = x.CategoryId
            }).ToList();
        }

        [Route("DeleteProduct/{id}")]

        public async Task<IActionResult> DeleteProduct(string id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.DeleteAsync("https://localhost:7070/api/Products?id=" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Product", new { area = "Admin" });
            }
            return View();
        }

        [HttpGet]
        [Route("UpdateProduct")]
        [Route("UpdateProduct/{id}")]
        public async Task<IActionResult> UpdateProduct(string? id)
        {
            ViewBag.v1 = "AnaSayfa";
            ViewBag.v2 = "Products";
            ViewBag.v3 = "Product Guncelleme";
            ViewBag.v0 = "Product Islemleri";
            await LoadCategoriesAsync();

            if (string.IsNullOrWhiteSpace(id))
            {
                return View(new UpdateProductDto());
            }

            var client = _httpClientFactory.CreateClient();
            UpdateProductDto? values = null;
            HttpResponseMessage? responseMessage = null;

            try
            {
                responseMessage = await client.GetAsync("https://localhost:7070/api/Products/" + id);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonData = await responseMessage.Content.ReadAsStringAsync();
                    if (!string.IsNullOrWhiteSpace(jsonData) && jsonData != "null")
                    {
                        values = JsonConvert.DeserializeObject<UpdateProductDto>(jsonData);
                    }
                }

                if (values == null || string.IsNullOrWhiteSpace(values.ProductName))
                {
                    var listResponse = await client.GetAsync("https://localhost:7070/api/Products");
                    if (listResponse.IsSuccessStatusCode)
                    {
                        var listJson = await listResponse.Content.ReadAsStringAsync();
                        var list = JsonConvert.DeserializeObject<List<ResultProductDto>>(listJson) ?? new List<ResultProductDto>();
                        var product = list.FirstOrDefault(x => x.ProductId == id);
                        if (product != null)
                        {
                            values = new UpdateProductDto
                            {
                                ProductId = product.ProductId,
                                ProductName = product.ProductName,
                                ProductPrice = product.ProductPrice,
                                ProductImageUrl = product.ProductImageUrl,
                                ProductDescription = product.ProductDescription,
                                CategoryId = product.CategoryId
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.error = "Catalog API'ye ulasilamadi. " + ex.Message;
            }

            if (values == null)
            {
                ViewBag.error ??= "Product getirilemedi. API durum kodu: " + (responseMessage != null ? ((int)responseMessage.StatusCode).ToString() : "baglanti yok");
                values = new UpdateProductDto { ProductId = id };
            }
            else
            {
                values.ProductId ??= id;
            }

            return View(values);
        }

        [HttpPost]
        [Route("UpdateProduct")]
        [Route("UpdateProduct/{id}")]
        public async Task<IActionResult> UpdateProduct(UpdateProductDto updateProductDto, string? id)
        {
            updateProductDto.ProductId = string.IsNullOrWhiteSpace(updateProductDto.ProductId) ? id : updateProductDto.ProductId;
            updateProductDto.ProductImageUrl ??= string.Empty;
            updateProductDto.ProductDescription ??= string.Empty;
            updateProductDto.ProductName ??= string.Empty;
            updateProductDto.CategoryId ??= string.Empty;

            try
            {
                var client = _httpClientFactory.CreateClient();
                var jsonData = JsonConvert.SerializeObject(updateProductDto);
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var responseMessage = await client.PutAsync("https://localhost:7070/api/Products", content);
                if (responseMessage.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Product", new { area = "Admin" });
                }

                var errorBody = await responseMessage.Content.ReadAsStringAsync();
                ViewBag.error = "Product guncellenemedi. API durum kodu: " + (int)responseMessage.StatusCode + " " + errorBody;
            }
            catch (Exception ex)
            {
                ViewBag.error = "Catalog API'ye ulasilamadi (https://localhost:7070). Catalog projesini https profiliyle baslatin. " + ex.Message;
            }

            ViewBag.v1 = "AnaSayfa";
            ViewBag.v2 = "Products";
            ViewBag.v3 = "Product Guncelleme";
            ViewBag.v0 = "Product Islemleri";
            await LoadCategoriesAsync();
            return View(updateProductDto);
        }
    }
}
