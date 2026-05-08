using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CleaningApi.Interfaces
{
    public interface ISupervisorServices
    {
        Task<IActionResult> GetOrdersForInspection();
        Task<IActionResult> GetChecklistByOrderId(int orderId);
        Task<IActionResult> UpdateChecklistItem(int checklistId, bool isVerified, string remarks);
        Task<IActionResult> AcceptOrder(int orderId);
        Task<IActionResult> RequestRework(int orderId);
    }
}