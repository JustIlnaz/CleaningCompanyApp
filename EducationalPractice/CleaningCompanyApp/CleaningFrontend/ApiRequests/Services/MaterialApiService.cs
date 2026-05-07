using CleaningFrontend.ApiRequests.Model;
using System.Net.Http.Json;

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CleaningFrontend.ApiRequests.Services
{
    public class MaterialApiService
    {
        private readonly HttpClient _httpClient;

        public MaterialApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<MaterialModel>> GetAllMaterials(string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var response = await _httpClient.GetFromJsonAsync<MaterialsResult>("api/material/getallmaterials");
            return response?.Materials ?? new List<MaterialModel>();
        }

        public async Task<bool> CreateMaterial(MaterialModel material, string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var response = await _httpClient.PostAsJsonAsync("api/material/creatematerial", material);
            var result = await response.Content.ReadFromJsonAsync<ActionResult>();
            return result?.status ?? false;
        }

        public async Task<bool> UpdateMaterial(MaterialModel material, string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var response = await _httpClient.PutAsJsonAsync("api/material/updatematerial", material);
            var result = await response.Content.ReadFromJsonAsync<ActionResult>();
            return result?.status ?? false;
        }

        public async Task<bool> DeleteMaterial(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var response = await _httpClient.DeleteAsync($"api/material/deletematerial?id={id}");
            var result = await response.Content.ReadFromJsonAsync<ActionResult>();
            return result?.status ?? false;
        }

        public async Task<bool> RequestMaterial(int materialId, int quantity, string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var response = await _httpClient.PostAsJsonAsync("api/material/request", new { MaterialId = materialId, Quantity = quantity });
            if (!response.IsSuccessStatusCode) return false;
            var result = await response.Content.ReadFromJsonAsync<ActionResult>();
            return result?.status ?? false;
        }
    }

    public class MaterialsResult
    {
        public bool status { get; set; }
        public List<MaterialModel> Materials { get; set; }
    }

    public class ActionResult
    {
        public bool status { get; set; }
        public string error { get; set; }
    }
}
