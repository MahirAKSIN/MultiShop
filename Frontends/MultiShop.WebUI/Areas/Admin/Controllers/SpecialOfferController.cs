using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.SpecialOfferDtos;
using Newtonsoft.Json;
using System.Text;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/ SpecialOffer")]
    [AllowAnonymous]
    public class SpecialOfferController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public SpecialOfferController(IHttpClientFactory httpClientFactory)
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
            var responseMessage = await client.GetAsync("https://localhost:7070/api/SpecialOffer");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultSpecialOfferDto>>(jsonData);
                return View(values ?? new List<ResultSpecialOfferDto>());
            }

            return View(new List<ResultSpecialOfferDto>());
        }
        [HttpGet]
        [Route("CreateSpecialOffer")]

        public IActionResult CreateSpecialOffer()
        {
            ViewBag.v1 = "AnaSayfa";
            ViewBag.v2 = "Öne cıkanlar Gorseller";
            ViewBag.v3 = "Slider Öne cıkanlar Gorseller listeesi";
            ViewBag.v0 = "Öne cıkanlar  Slider Gorseller Işlemleri";
            return View();
        }
        [HttpPost]
        [Route("CreateSpecialOffer")]
        public async Task<IActionResult> CreateSpecialOffer(CreateSpecialOfferDto createSpecialOfferDto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(createSpecialOfferDto);
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("https://localhost:7070/api/SpecialOffer", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "SpecialOffer", new { area = "Admin" });
            }

            ViewBag.v1 = "AnaSayfa";
            ViewBag.v2 = "Öne cıkanlar Gorseller";
            ViewBag.v3 = "Slider Öne cıkanlar Gorseller listeesi";
            ViewBag.v0 = "Öne cıkanlar  Slider Gorseller Işlemleri";
            ViewBag.error = "Öne cıkanlar Gorseller eklenemedi. API durum kodu: " + (int)responseMessage.StatusCode;
            return View();
        }
        [Route("DeleteSpecialOffer/{id}")]

        public async Task<IActionResult> DeleteSpecialOffer(string id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.DeleteAsync("https://localhost:7070/api/SpecialOffer?id=" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "SpecialOffer", new { area = "Admin" });
            }
            return View();
        }

        [HttpGet]
        [Route("UpdateSpecialOffer/{id}")]
        public async Task<IActionResult> UpdateSpecialOffer(string id)
        {

            ViewBag.v1 = "AnaSayfa";
            ViewBag.v2 = "Öne cıkanlar Gorseller";
            ViewBag.v3 = "Slider Öne cıkanlar Gorseller listeesi";
            ViewBag.v0 = "Öne cıkanlar  Slider Gorseller Işlemleri";

            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7070/api/SpecialOffer/" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<UpdateSpecialOfferDto>(jsonData);
                return View(values ?? new UpdateSpecialOfferDto { SpecialOfferId = id });
            }

            ViewBag.error = "Kategori getirilemedi. API durum kodu: " + (int)responseMessage.StatusCode;
            return View(new UpdateSpecialOfferDto { SpecialOfferId = id });
        }

        [HttpPost]
        [Route("UpdateSpecialOffer/{id}")]
        public async Task<IActionResult> UpdateSpecialOffer(UpdateSpecialOfferDto updateSpecialOfferDto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(updateSpecialOfferDto);
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync("https://localhost:7070/api/SpecialOffer", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "SpecialOffer", new { area = "Admin" });
            }

            ViewBag.v1 = "AnaSayfa";
            ViewBag.v2 = "Öne cıkanlar Gorseller";
            ViewBag.v3 = "Slider Öne cıkanlar Gorseller listeesi";
            ViewBag.v0 = "Öne cıkanlar  Slider Gorseller Işlemleri";
            ViewBag.error = "Kategori guncellenemedi. API durum kodu: " + (int)responseMessage.StatusCode;
            return View(updateSpecialOfferDto);
        }
    }

}
