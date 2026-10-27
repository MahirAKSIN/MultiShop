using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.CategoryDtos;
using Newtonsoft.Json;
using System.Text;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Category")]
    [AllowAnonymous]
    public class CategoryController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public CategoryController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            ViewBag.v1 = "AnaSayfa";
            ViewBag.v2 = "Kategoriler";
            ViewBag.v3 = "Kategori listesi";
            ViewBag.v0 = "Kategori Islemleri";

            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7070/api/Categories");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultCategoryDto>>(jsonData);
                return View(values ?? new List<ResultCategoryDto>());
            }

            return View(new List<ResultCategoryDto>());
        }
        [HttpGet]
        [Route("CreateCategory")]

        public IActionResult CreateCategory()
        {
            ViewBag.v1 = "AnaSayfa";
            ViewBag.v2 = "Kategoriler";
            ViewBag.v3 = "Yeni Kategori Girisi";
            ViewBag.v0 = "Kategori Islemleri";
            return View();
        }
        [HttpPost]
        [Route("CreateCategory")]
        public async Task<IActionResult> CreateCategory(CreateCategoryDto createCategoryDto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(createCategoryDto);
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("https://localhost:7070/api/Categories", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Category", new { area = "Admin" });
            }

            ViewBag.v1 = "AnaSayfa";
            ViewBag.v2 = "Kategoriler";
            ViewBag.v3 = "Yeni Kategori Girisi";
            ViewBag.v0 = "Kategori Islemleri";
            ViewBag.error = "Kategori eklenemedi. API durum kodu: " + (int)responseMessage.StatusCode;
            return View();
        }
        [Route("DeleteCategory/{id}")]

        public async Task<IActionResult> DeleteCategory(string id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.DeleteAsync("https://localhost:7070/api/Categories?id=" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Category", new { area = "Admin" });
            }
            return View();
        }

        [HttpGet]
        [Route("UpdateCategory/{id}")]
        public async Task<IActionResult> UpdateCategory(string id)
        {
            ViewBag.v1 = "AnaSayfa";
            ViewBag.v2 = "Kategoriler";
            ViewBag.v3 = "Kategori Guncelleme";
            ViewBag.v0 = "Kategori Islemleri";

            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7070/api/Categories/" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<UpdateCategoryDto>(jsonData);
                return View(values ?? new UpdateCategoryDto { CategoryId = id });
            }

            ViewBag.error = "Kategori getirilemedi. API durum kodu: " + (int)responseMessage.StatusCode;
            return View(new UpdateCategoryDto { CategoryId = id });
        }

        [HttpPost]
        [Route("UpdateCategory/{id}")]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryDto updateCategoryDto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(updateCategoryDto);
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync("https://localhost:7070/api/Categories", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Category", new { area = "Admin" });
            }

            ViewBag.v1 = "AnaSayfa";
            ViewBag.v2 = "Kategoriler";
            ViewBag.v3 = "Kategori Guncelleme";
            ViewBag.v0 = "Kategori Islemleri";
            ViewBag.error = "Kategori guncellenemedi. API durum kodu: " + (int)responseMessage.StatusCode;
            return View(updateCategoryDto);
        }
    }
}
