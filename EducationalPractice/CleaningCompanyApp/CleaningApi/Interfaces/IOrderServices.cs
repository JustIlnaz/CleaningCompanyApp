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
    }
}
