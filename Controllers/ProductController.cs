using BuildingShopFront.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BuildingShopFront.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient _httpClient;
        public ProductController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IActionResult> Index()
        {
            var response =await _httpClient.GetAsync("api/Product");
            response.EnsureSuccessStatusCode();
            var content=await response.Content.ReadAsStringAsync();
            var data=JsonConvert.DeserializeObject
                <List<ProductController>>(content);
            return View(data);
        }
    }
}
