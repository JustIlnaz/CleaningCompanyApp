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
            return await ReadStatus(response);
        }

        public async Task<bool> UpdateOrder(OrderModel order, string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var response = await _httpClient.PutAsJsonAsync("api/order/updateorder", order);
            return await ReadStatus(response);
        }

        public async Task<bool> DeleteOrder(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var response = await _httpClient.DeleteAsync($"api/order/deleteorder?id={id}");
            return await ReadStatus(response);
        }

        private static async Task<bool> ReadStatus(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode) return false;
            try
            {
                var result = await response.Content.ReadFromJsonAsync<ActionResult>();
                return result?.status ?? false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<OrderModel>> GetMyOrders(string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            try
            {
                var response = await _httpClient.GetFromJsonAsync<OrdersResult>("api/order/getmyorders");
                return response?.Orders ?? new List<OrderModel>();
            }
            catch (System.Net.Http.HttpRequestException)
            {
                return new List<OrderModel>();
            }
        }

        public async Task<bool> CreateMyOrder(CreateMyOrderModel order, string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var response = await _httpClient.PostAsJsonAsync("api/order/createmyorder", order);
            if (!response.IsSuccessStatusCode) return false;
            var result = await response.Content.ReadFromJsonAsync<ActionResult>();
            return result?.status ?? false;
        }

        public async Task<bool> ChangeStatus(int orderId, string status, string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var url = $"api/order/changestatus?id={orderId}&status={System.Uri.EscapeDataString(status)}";
            var response = await _httpClient.PostAsync(url, null);
            if (!response.IsSuccessStatusCode) return false;
            var result = await response.Content.ReadFromJsonAsync<ActionResult>();
            return result?.status ?? false;
        }

        public async Task<bool> AssignBrigade(int orderId, int brigadeId, string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var url = $"api/order/assignbrigade?orderId={orderId}&brigadeId={brigadeId}";
            var response = await _httpClient.PostAsync(url, null);
            if (!response.IsSuccessStatusCode) return false;
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
