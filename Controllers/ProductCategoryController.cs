using BuildingShopFront.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BuildingShopFront.Controllers
{
    public class ProductCategoryController:Controller
    {
        private readonly HttpClient _httpClient;
        public ProductCategoryController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("api/ProductCategory");
            response.EnsureSuccessStatusCode();
            var content=await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject
                <List<ProductCategory>>(content);
            return View(data);
        }
        public async Task<IActionResult> Add(string name)
        {
            var content = new StringContent(name);
            var response = await _httpClient.PostAsync
                ("api/ProductCategory", content);
            response.EnsureSuccessStatusCode();
            return RedirectToAction("Index");
        }
    }
}
