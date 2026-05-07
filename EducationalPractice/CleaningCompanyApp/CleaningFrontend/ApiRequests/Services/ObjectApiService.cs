using CleaningFrontend.ApiRequests.Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CleaningFrontend.ApiRequests.Services
{
    public class ObjectApiService
    {
        private readonly HttpClient _httpClient;

        public ObjectApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ObjectModel>> GetAllObjects(string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var response = await _httpClient.GetFromJsonAsync<ObjectsResult>("api/object/getallobjects");
            return response?.Objects ?? new List<ObjectModel>();
        }
    }

    public class ObjectsResult
    {
        public bool status { get; set; }
        public List<ObjectModel> Objects { get; set; }
    }
}
