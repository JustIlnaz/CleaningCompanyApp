using System.Threading.Tasks;
using CleaningApi.Requests;
using Microsoft.AspNetCore.Mvc;

namespace CleaningApi.Interfaces
{
    public interface IOrderServices
    {
        Task<IActionResult> GetAllOrders();
        Task<IActionResult> GetOrderById(int id);
        Task<IActionResult> CreateOrder(CreateOrder order);
        Task<IActionResult> UpdateOrder(UpdateOrder order);
        Task<IActionResult> DeleteOrder(int id);
        Task<IActionResult> GetOrdersByBrigade(int brigadeId);
        Task<IActionResult> GetOrdersByClient(int clientId);
        Task<IActionResult> GetMyOrders(string token);
        Task<IActionResult> CreateMyOrder(CreateMyOrder order, string token);
        Task<IActionResult> ChangeStatus(int orderId, string status, string token);
        Task<IActionResult> AssignBrigade(int orderId, int brigadeId);
    }
}
