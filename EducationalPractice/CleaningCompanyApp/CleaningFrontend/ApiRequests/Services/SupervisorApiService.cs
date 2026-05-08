using CleaningFrontend.ApiRequests.Model;
using CleaningFrontend.ApiRequests.Models;
using System.Net.Http.Json;

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CleaningFrontend.ApiRequests.Services
{
    public class SupervisorApiService
    {
        private readonly HttpClient _httpClient;

        public SupervisorApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<OrdersResult> GetOrdersForInspection(string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var response = await _httpClient.GetFromJsonAsync<OrdersResult>("api/supervisor/getordersforinspection");
            return response ?? new OrdersResult();
        }

        public async Task<ChecklistResult> GetChecklistByOrderId(int orderId, string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var response = await _httpClient.GetFromJsonAsync<ChecklistResult>($"api/supervisor/getchecklistbyorderid?orderId={orderId}");
            return response ?? new ChecklistResult();
        }

        public async Task<ActionResult> UpdateChecklistItem(int checklistId, bool isVerified, string remarks, string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var response = await _httpClient.PutAsJsonAsync("api/supervisor/updatechecklistitem", new { checklistId, isVerified, remarks });
            var result = await response.Content.ReadFromJsonAsync<ActionResult>();
            return result ?? new ActionResult { status = false, error = "Не удалось обновить пункт чек-листа" };
        }

        public async Task<ActionResult> AcceptOrder(int orderId, string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var response = await _httpClient.PostAsJsonAsync("api/supervisor/acceptorder", new { orderId });
            var result = await response.Content.ReadFromJsonAsync<ActionResult>();
            return result ?? new ActionResult { status = false, error = "Не удалось принять заказ" };
        }

        public async Task<ActionResult> RequestRework(int orderId, string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var response = await _httpClient.PostAsJsonAsync("api/supervisor/requestrework", new { orderId });
            var result = await response.Content.ReadFromJsonAsync<ActionResult>();
            return result ?? new ActionResult { status = false, error = "Не удалось запросить переделку" };
        }
    }
}