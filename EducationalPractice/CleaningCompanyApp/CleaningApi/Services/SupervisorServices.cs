using System;
using System.Linq;
using System.Threading.Tasks;
using CleaningApi.DatabaseContext;
using CleaningApi.Interfaces;
using CleaningApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CleaningApi.Services
{
    public class SupervisorServices : ISupervisorServices
    {
        private readonly ContextDb _contextDb;

        public SupervisorServices(ContextDb contextDb)
        {
            _contextDb = contextDb;
        }

        public async Task<IActionResult> GetOrdersForInspection()
        {
            var orders = await _contextDb.Orders
                .Where(x => x.Status == "Завершен" && x.Act == null)
                .Include(x => x.CleaningObject)
                .Include(x => x.Brigade)
                .Include(x => x.Client)
                .OrderByDescending(x => x.ScheduledDate)
                .ToListAsync();

            return new OkObjectResult(new { status = true, orders = orders.Select(MapToView).ToList() });
        }

        public async Task<IActionResult> GetChecklistByOrderId(int orderId)
        {
            var checklistItems = await _contextDb.Checklists
                .Where(x => x.OrderId == orderId)
                .OrderBy(x => x.Id_Checklist)
                .ToListAsync();

            return new OkObjectResult(new { status = true, checklistItems = checklistItems.Select(MapToView).ToList() });
        }

        public async Task<IActionResult> UpdateChecklistItem(int checklistId, bool isVerified, string remarks)
        {
            var checklistItem = await _contextDb.Checklists.FirstOrDefaultAsync(x => x.Id_Checklist == checklistId);

            if (checklistItem == null)
            {
                return new NotFoundObjectResult(new { status = false, error = "Пункт чек-листа не найден" });
            }

            checklistItem.IsVerified = isVerified;
            checklistItem.SupervisorRemarks = remarks;

            await _contextDb.SaveChangesAsync();

            return new OkObjectResult(new { status = true, checklistItem = MapToView(checklistItem) });
        }

        public async Task<IActionResult> AcceptOrder(int orderId)
        {
            var order = await _contextDb.Orders
                .Include(x => x.Act)
                .FirstOrDefaultAsync(x => x.Id_Order == orderId);

            if (order == null)
            {
                return new NotFoundObjectResult(new { status = false, error = "Заказ не найден" });
            }

            var allVerified = await _contextDb.Checklists
                .Where(x => x.OrderId == orderId)
                .AllAsync(x => x.IsVerified);

            if (!allVerified)
            {
                return new BadRequestObjectResult(new { status = false, error = "Не все пункты чек-листа проверены" });
            }

            if (order.Act == null)
            {
                var act = new Act
                {
                    Date = DateTime.UtcNow,
                    Content = $"Акт выполненных работ по заказу №{order.Id_Order}",
                    TotalAmount = order.Price,
                    OrderId = order.Id_Order
                };

                await _contextDb.Acts.AddAsync(act);
                order.Act = act;
            }

            order.Status = "Принят";
            await _contextDb.SaveChangesAsync();

            return new OkObjectResult(new { status = true, order = MapToView(order) });
        }

        public async Task<IActionResult> RequestRework(int orderId)
        {
            var order = await _contextDb.Orders.FirstOrDefaultAsync(x => x.Id_Order == orderId);

            if (order == null)
            {
                return new NotFoundObjectResult(new { status = false, error = "Заказ не найден" });
            }

            order.Status = "На переделке";
            await _contextDb.SaveChangesAsync();

            return new OkObjectResult(new { status = true, order = MapToView(order) });
        }

        private static object MapToView(Order order)
        {
            return new
            {
                order.Id_Order,
                order.CreatedDate,
                order.ScheduledDate,
                order.Status,
                order.CleaningType,
                order.Price,
                order.PaymentStatus,
                order.ObjectId,
                ObjectAddress = order.CleaningObject?.Address,
                order.BrigadeId,
                BrigadeName = order.Brigade?.Name,
                order.ClientId,
                ClientName = order.Client?.Name
            };
        }

        private static object MapToView(Checklist checklist)
        {
            return new
            {
                checklist.Id_Checklist,
                checklist.Item,
                checklist.IsCompleted,
                checklist.IsVerified,
                checklist.SupervisorRemarks,
                checklist.OrderId
            };
        }
    }
}