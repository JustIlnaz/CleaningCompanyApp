using CleaningFrontend.ApiRequests.Model;
using System.Net.Http.Json;

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CleaningFrontend.ApiRequests.Services
{
    public class BrigadeApiService
    {
        private readonly HttpClient _httpClient;

        public BrigadeApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<BrigadeModel>> GetAllBrigades(string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var response = await _httpClient.GetFromJsonAsync<BrigadesResult>("api/brigade/getallbrigades");
            return response?.Brigades ?? new List<BrigadeModel>();
        }

        public async Task<BrigadeModel> GetBrigadeById(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var response = await _httpClient.GetFromJsonAsync<BrigadeResult>($"api/brigade/getbrigadebyid?id={id}");
            return response?.Brigade;
        }

        public async Task<bool> CreateBrigade(BrigadeModel brigade, string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var response = await _httpClient.PostAsJsonAsync("api/brigade/createbrigade", brigade);
            var result = await response.Content.ReadFromJsonAsync<ActionResult>();
            return result?.status ?? false;
        }

        public async Task<bool> UpdateBrigade(BrigadeModel brigade, string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var response = await _httpClient.PutAsJsonAsync("api/brigade/updatebrigade", brigade);
            var result = await response.Content.ReadFromJsonAsync<ActionResult>();
            return result?.status ?? false;
        }

        public async Task<bool> DeleteBrigade(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var response = await _httpClient.DeleteAsync($"api/brigade/deletebrigade?id={id}");
            var result = await response.Content.ReadFromJsonAsync<ActionResult>();
            return result?.status ?? false;
        }
    }

    public class BrigadesResult
    {
        public bool status { get; set; }
        public List<BrigadeModel> Brigades { get; set; }
    }

    public class BrigadeResult
    {
        public bool status { get; set; }
        public BrigadeModel Brigade { get; set; }
    }
}
