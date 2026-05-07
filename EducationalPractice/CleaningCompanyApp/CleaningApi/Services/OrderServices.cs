using System;
using System.Linq;
using System.Threading.Tasks;
using CleaningApi.DatabaseContext;
using CleaningApi.Interfaces;
using CleaningApi.Models;
using CleaningApi.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CleaningApi.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly ContextDb _contextDb;

        public OrderServices(ContextDb contextDb)
        {
            _contextDb = contextDb;
        }

        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _contextDb.Orders
                .Include(x => x.CleaningObject)
                .Include(x => x.Brigade)
                .Include(x => x.Client)
                .ToListAsync();

            if (orders == null || orders.Count == 0)
            {
                return new NotFoundObjectResult(new { error = "Заказы не найдены" });
            }

            return new OkObjectResult(new { status = true, orders });
        }

        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _contextDb.Orders
                .Include(x => x.CleaningObject)
                .Include(x => x.Brigade)
                .Include(x => x.Client)
                .FirstOrDefaultAsync(x => x.Id_Order == id);

            if (order == null)
            {
                return new NotFoundObjectResult(new { error = "Заказ не найден" });
            }

            return new OkObjectResult(new { status = true, order });
        }

        public async Task<IActionResult> CreateOrder(CreateOrder newOrder)
        {
            var order = new Order
            {
                CreatedDate = DateTime.Now,
                ScheduledDate = newOrder.ScheduledDate,
                Status = "Новый",
                CleaningType = newOrder.CleaningType,
                Price = newOrder.Price,
                PaymentStatus = "Не оплачен",
                ObjectId = newOrder.ObjectId,
                ClientId = newOrder.ClientId
            };

            await _contextDb.Orders.AddAsync(order);
            await _contextDb.SaveChangesAsync();

            return new OkObjectResult(new { status = true, order });
        }

        public async Task<IActionResult> UpdateOrder(UpdateOrder updateOrder)
        {
            var order = await _contextDb.Orders.FirstOrDefaultAsync(x => x.Id_Order == updateOrder.Id_Order);

            if (order == null)
            {
                return new NotFoundObjectResult(new { error = "Заказ не найден" });
            }

            if (updateOrder.ScheduledDate.HasValue) order.ScheduledDate = updateOrder.ScheduledDate.Value;
            if (!string.IsNullOrEmpty(updateOrder.Status)) order.Status = updateOrder.Status;
            if (!string.IsNullOrEmpty(updateOrder.CleaningType)) order.CleaningType = updateOrder.CleaningType;
            if (updateOrder.Price.HasValue) order.Price = updateOrder.Price.Value;
            if (!string.IsNullOrEmpty(updateOrder.PaymentStatus)) order.PaymentStatus = updateOrder.PaymentStatus;
            if (updateOrder.BrigadeId.HasValue) order.BrigadeId = updateOrder.BrigadeId;

            await _contextDb.SaveChangesAsync();

            return new OkObjectResult(new { status = true, order });
        }

        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _contextDb.Orders.FirstOrDefaultAsync(x => x.Id_Order == id);

            if (order == null)
            {
                return new NotFoundObjectResult(new { error = "Заказ не найден" });
            }

            _contextDb.Orders.Remove(order);
            await _contextDb.SaveChangesAsync();

            return new OkObjectResult(new { status = true, order });
        }

        public async Task<IActionResult> GetOrdersByBrigade(int brigadeId)
        {
            var orders = await _contextDb.Orders
                .Include(x => x.CleaningObject)
                .Include(x => x.Client)
                .Where(x => x.BrigadeId == brigadeId)
                .ToListAsync();

            return new OkObjectResult(new { status = true, orders });
        }

        public async Task<IActionResult> GetOrdersByClient(int clientId)
        {
            var orders = await _contextDb.Orders
                .Include(x => x.CleaningObject)
                .Include(x => x.Brigade)
                .Where(x => x.ClientId == clientId)
                .ToListAsync();

            return new OkObjectResult(new { status = true, orders });
        }
    }
}
