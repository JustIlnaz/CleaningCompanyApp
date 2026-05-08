using System.Linq;
using System.Threading.Tasks;
using CleaningApi.CustomAttributes;
using CleaningApi.Interfaces;
using CleaningApi.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CleaningApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupervisorController : Controller
    {
        private readonly ISupervisorServices _supervisorServices;

        public SupervisorController(ISupervisorServices supervisorServices)
        {
            _supervisorServices = supervisorServices;
        }

        [HttpGet]
        [Route("GetOrdersForInspection")]
        [RoleAuthorize([1, 2, 5])] // Admin, Dispatcher, Supervisor
        public async Task<IActionResult> GetOrdersForInspection()
        {
            return await _supervisorServices.GetOrdersForInspection();
        }

        [HttpGet]
        [Route("GetChecklistByOrderId")]
        [RoleAuthorize([1, 2, 5])] // Admin, Dispatcher, Supervisor
        public async Task<IActionResult> GetChecklistByOrderId(int orderId)
        {
            return await _supervisorServices.GetChecklistByOrderId(orderId);
        }

        [HttpPut]
        [Route("UpdateChecklistItem")]
        [RoleAuthorize([1, 2, 5])] // Admin, Dispatcher, Supervisor
        public async Task<IActionResult> UpdateChecklistItem(int checklistId, bool isVerified, string remarks)
        {
            return await _supervisorServices.UpdateChecklistItem(checklistId, isVerified, remarks);
        }

        [HttpPost]
        [Route("AcceptOrder")]
        [RoleAuthorize([1, 2, 5])] // Admin, Dispatcher, Supervisor
        public async Task<IActionResult> AcceptOrder(int orderId)
        {
            return await _supervisorServices.AcceptOrder(orderId);
        }

        [HttpPost]
        [Route("RequestRework")]
        [RoleAuthorize([1, 2, 5])] // Admin, Dispatcher, Supervisor
        public async Task<IActionResult> RequestRework(int orderId)
        {
            return await _supervisorServices.RequestRework(orderId);
        }
    }
}