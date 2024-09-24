using BuildingShopFront.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
                <List<Product>>(content);
            await LoadCategories();
            return View(data);
        }
        
        public async Task LoadCategories()
        {
            var response = await _httpClient.GetAsync("api/ProductCategory");
            response.EnsureSuccessStatusCode();
            var content=await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject
                <List<ProductCategory>>(content);
            ViewBag.Categories = new SelectList(data, "Id", "Name");
        }
        
        public async Task<IActionResult> Add(string name,
            int count, int categoryId)
        {
            if (!string.IsNullOrEmpty(name) && count > 0 && categoryId > 0)
            {
                var requestData = new
                {
                    name = name,
                    count = count,
                    categoryId = categoryId
                };
                var content = JsonContent.Create(requestData);
                var response = await _httpClient.PostAsync
                    ("api/Product", content);
                response.EnsureSuccessStatusCode();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(long id,string name,
            int count, int categoryId)
        {
            if (!string.IsNullOrEmpty(name) && count > 0 && categoryId > 0)
            {
                var requestData = new
                {
                    id = id,
                    name = name,
                    count = count,
                    categoryId = categoryId
                };
                var content = JsonContent.Create(requestData);
                var response = await _httpClient.PutAsync
                    ("api/Product", content);
                response.EnsureSuccessStatusCode();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(long id)
        {
            var response = await _httpClient.DeleteAsync
                ($"api/Product/{id}");
            response.EnsureSuccessStatusCode();
            return RedirectToAction("Index");
        }
    }
}
