using Newtonsoft.Json;
using ProyectoFinal.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Services
{
    public class ApiService
    {
        private const string BaseUrl = "https://fakestoreapi.com/products";

        private HttpClient _httpClient = new HttpClient();

        public async Task<List<Product>> GetProductsAsync()
        {
            var response = await _httpClient.GetStringAsync(BaseUrl);
            return JsonConvert.DeserializeObject<List<Product>>(response);
        }

        public async Task<Product> GetProductAsync(int id)
        {
            var response = await _httpClient.GetStringAsync($"{BaseUrl}/{id}");
            return JsonConvert.DeserializeObject<Product>(response);
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            var content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(BaseUrl, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Product>(responseContent);
        }

        public async Task UpdateProductAsync(Product product)
        {
            var content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
            await _httpClient.PutAsync($"{BaseUrl}/{product.Id}", content);
        }

        public async Task DeleteProductAsync(int id)
        {
            await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
        }
    }
}
