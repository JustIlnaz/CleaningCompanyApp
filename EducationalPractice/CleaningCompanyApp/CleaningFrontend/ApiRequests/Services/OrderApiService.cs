using CleaningFrontend.ApiRequests.Model;
using System.Net.Http.Json;

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CleaningFrontend.ApiRequests.Services
{
    public class OrderApiService
    {
        private readonly HttpClient _httpClient;

        public OrderApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<OrderModel>> GetAllOrders(string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var response = await _httpClient.GetFromJsonAsync<OrdersResult>("api/order/getallorders");
            return response?.Orders ?? new List<OrderModel>();
        }

        public async Task<OrderModel> GetOrderById(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var response = await _httpClient.GetFromJsonAsync<OrderResult>($"api/order/getorderbyid?id={id}");
            return response?.Order;
        }

        public async Task<bool> CreateOrder(OrderModel order, string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var response = await _httpClient.PostAsJsonAsync("api/order/createorder", order);
            var result = await response.Content.ReadFromJsonAsync<ActionResult>();
            return result?.status ?? false;
        }

        public async Task<bool> UpdateOrder(OrderModel order, string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var response = await _httpClient.PutAsJsonAsync("api/order/updateorder", order);
            var result = await response.Content.ReadFromJsonAsync<ActionResult>();
            return result?.status ?? false;
        }

        public async Task<bool> DeleteOrder(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var response = await _httpClient.DeleteAsync($"api/order/deleteorder?id={id}");
            var result = await response.Content.ReadFromJsonAsync<ActionResult>();
            return result?.status ?? false;
        }
    }

    public class OrdersResult
    {
        public bool status { get; set; }
        public List<OrderModel> Orders { get; set; }
    }

    public class OrderResult
    {
        public bool status { get; set; }
        public OrderModel Order { get; set; }
    }
}
