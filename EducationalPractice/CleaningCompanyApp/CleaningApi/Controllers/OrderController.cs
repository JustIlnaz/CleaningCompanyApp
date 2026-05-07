using System.Threading.Tasks;
using CleaningApi.CustomAttributes;
using CleaningApi.Interfaces;
using CleaningApi.Requests;
using Microsoft.AspNetCore.Mvc;

namespace CleaningApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly IOrderServices _orderServices;

        public OrderController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }

        [HttpGet]
        [Route("GetAllOrders")]
        [RoleAuthorize([1, 2])]
        public async Task<IActionResult> GetAllOrders()
        {
            return await _orderServices.GetAllOrders();
        }

        [HttpGet]
        [Route("GetOrderById")]
        [RoleAuthorize([1, 2, 3])]
        public async Task<IActionResult> GetOrderById(int id)
        {
            return await _orderServices.GetOrderById(id);
        }

        [HttpPost]
        [Route("CreateOrder")]
        [RoleAuthorize([1, 2, 4])]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrder order)
        {
            return await _orderServices.CreateOrder(order);
        }

        [HttpPut]
        [Route("UpdateOrder")]
        [RoleAuthorize([1, 2])]
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrder order)
        {
            return await _orderServices.UpdateOrder(order);
        }

        [HttpDelete]
        [Route("DeleteOrder")]
        [RoleAuthorize([1])]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            return await _orderServices.DeleteOrder(id);
        }

        [HttpGet]
        [Route("GetOrdersByBrigade")]
        [RoleAuthorize([1, 2, 3])]
        public async Task<IActionResult> GetOrdersByBrigade(int brigadeId)
        {
            return await _orderServices.GetOrdersByBrigade(brigadeId);
        }

        [HttpGet]
        [Route("GetOrdersByClient")]
        [RoleAuthorize([1, 2, 4])]
        public async Task<IActionResult> GetOrdersByClient(int clientId)
        {
            return await _orderServices.GetOrdersByClient(clientId);
        }
    }
}
