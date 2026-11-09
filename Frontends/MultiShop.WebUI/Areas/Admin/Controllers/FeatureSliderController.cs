using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.FeatureSliderDtos;
using Newtonsoft.Json;
using System.Text;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/FeatureSlider")]
    [AllowAnonymous]
    public class FeatureSliderController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public FeatureSliderController(IHttpClientFactory httpClientFactory)
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
            var responseMessage = await client.GetAsync("https://localhost:7070/api/FeatureSlider");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultFeatureSliderDto>>(jsonData);
                return View(values ?? new List<ResultFeatureSliderDto>());
            }

            return View(new List<ResultFeatureSliderDto>());
        }
        [HttpGet]
        [Route("CreateFeatureSlider")]

        public IActionResult CreateFeatureSlider()
        {
            ViewBag.v1 = "AnaSayfa";
            ViewBag.v2 = "Öne cıkanlar Gorseller";
            ViewBag.v3 = "Slider Öne cıkanlar Gorseller listeesi";
            ViewBag.v0 = "Öne cıkanlar  Slider Gorseller Işlemleri";
            return View();
        }
        [HttpPost]
        [Route("CreateFeatureSlider")]
        public async Task<IActionResult> CreateFeatureSlider(CreateFeatureSliderDto createFeatureSliderDto)
        {
            createFeatureSliderDto.Statu = false;
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(createFeatureSliderDto);
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("https://localhost:7070/api/FeatureSlider", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "FeatureSlider", new { area = "Admin" });
            }

            ViewBag.v1 = "AnaSayfa";
            ViewBag.v2 = "Öne cıkanlar Gorseller";
            ViewBag.v3 = "Slider Öne cıkanlar Gorseller listeesi";
            ViewBag.v0 = "Öne cıkanlar  Slider Gorseller Işlemleri";
            ViewBag.error = "Öne cıkanlar Gorseller eklenemedi. API durum kodu: " + (int)responseMessage.StatusCode;
            return View();
        }
        [Route("DeleteFeatureSlider/{id}")]

        public async Task<IActionResult> DeleteFeatureSlider(string id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.DeleteAsync("https://localhost:7070/api/FeatureSlider?id=" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "FeatureSlider", new { area = "Admin" });
            }
            return View();
        }

        [HttpGet]
        [Route("UpdateFeatureSlider/{id}")]
        public async Task<IActionResult> UpdateFeatureSlider(string id)
        {

            ViewBag.v1 = "AnaSayfa";
            ViewBag.v2 = "Öne cıkanlar Gorseller";
            ViewBag.v3 = "Slider Öne cıkanlar Gorseller listeesi";
            ViewBag.v0 = "Öne cıkanlar  Slider Gorseller Işlemleri";

            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7070/api/FeatureSlider/" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<UpdateFeatureSliderDto>(jsonData);
                return View(values ?? new UpdateFeatureSliderDto { FeatureSliderId = id });
            }

            ViewBag.error = "Kategori getirilemedi. API durum kodu: " + (int)responseMessage.StatusCode;
            return View(new UpdateFeatureSliderDto { FeatureSliderId = id });
        }

        [HttpPost]
        [Route("UpdateFeatureSlider/{id}")]
        public async Task<IActionResult> UpdateFeatureSlider(UpdateFeatureSliderDto updateFeatureSliderDto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(updateFeatureSliderDto);
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync("https://localhost:7070/api/FeatureSlider", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "FeatureSlider", new { area = "Admin" });
            }

            ViewBag.v1 = "AnaSayfa";
            ViewBag.v2 = "Öne cıkanlar Gorseller";
            ViewBag.v3 = "Slider Öne cıkanlar Gorseller listeesi";
            ViewBag.v0 = "Öne cıkanlar  Slider Gorseller Işlemleri";
            ViewBag.error = "Kategori guncellenemedi. API durum kodu: " + (int)responseMessage.StatusCode;
            return View(updateFeatureSliderDto);
        }
    }
}

