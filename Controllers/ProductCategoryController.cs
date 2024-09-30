using BuildingShopFront.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics.Eventing.Reader;
using System.Text.Json.Nodes;

namespace BuildingShopFront.Controllers
{
    public class ProductCategoryController:Controller
    {
        private readonly HttpClient _httpClient;
        public ProductCategoryController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IActionResult> Index(string? searchString,
            int page=1)
        {
            return View(await GetData("","",page));
        }
        public async Task<IActionResult> Search(string? searchString,
            int page = 1)
        {
            return PartialView("_CategoriesTable", await GetData("", searchString, page));
        }
        private async Task<Object> GetData(
            string? sortOrder, string? searchString, int page=1)
        {
            var response = await _httpClient
                .GetAsync("api/ProductCategory/sort" +
                $"?sortOrder={sortOrder}&searchString={searchString}&page={page}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(content);
            List<ProductCategory> categories = new List<ProductCategory>();
            foreach (var item in json["items"])
            {
                categories.Add(JsonConvert.DeserializeObject
                    <ProductCategory>(item.ToString()));
            }
            ViewBag.CurrentPage = json["currentPage"];
            ViewBag.TotalPages = json["totalPages"];
            ViewBag.TotalCount = json["totalCount"];
            return categories;
        }
        public async Task<object> Add(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var requestData = new { name = name };
                var content1 = JsonContent.Create(requestData);
                var response1 = await _httpClient.PostAsync
                    ("api/ProductCategory", content1);
                response1.EnsureSuccessStatusCode();
            }
            return PartialView("_CategoriesTable",await GetData("", ""));
        }
        [HttpPost]
        public async Task<IActionResult> Update(long id,string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var requestData = new { id = id, name = name };
                var content = JsonContent.Create(requestData);
                var response = await _httpClient.PutAsync
                    ("api/ProductCategory", content);
                response.EnsureSuccessStatusCode();
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(long id)
        {
            var response = await _httpClient.DeleteAsync
                ($"api/ProductCategory/{id}");
            response?.EnsureSuccessStatusCode();
            return RedirectToAction("Index");
        }
    }
}
