using System;
using System.Linq;
using System.Threading.Tasks;
using CleaningApi.DatabaseContext;
using CleaningApi.Hubs;
using CleaningApi.Interfaces;
using CleaningApi.Models;
using CleaningApi.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace CleaningApi.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly ContextDb _contextDb;
        private readonly IHubContext<OrderHub> _orderHub;

        public OrderServices(ContextDb contextDb, IHubContext<OrderHub> orderHub)
        {
            _contextDb = contextDb;
            _orderHub = orderHub;
        }

        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _contextDb.Orders
                .Include(x => x.CleaningObject)
                .Include(x => x.Brigade)
                .Include(x => x.Client)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();

            return new OkObjectResult(new { status = true, orders = orders.Select(MapToView).ToList() });
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
                return new NotFoundObjectResult(new { status = false, error = "Заказ не найден" });
            }

            return new OkObjectResult(new { status = true, order = MapToView(order) });
        }

        public async Task<IActionResult> CreateOrder(CreateOrder newOrder)
        {
            var order = new Order
            {
                CreatedDate = DateTime.UtcNow,
                ScheduledDate = DateTime.SpecifyKind(newOrder.ScheduledDate, DateTimeKind.Utc),
                Status = "Новый",
                CleaningType = newOrder.CleaningType,
                Price = newOrder.Price,
                PaymentStatus = "Не оплачен",
                ObjectId = newOrder.ObjectId,
                ClientId = newOrder.ClientId
            };

            await _contextDb.Orders.AddAsync(order);
            await _contextDb.SaveChangesAsync();

            await _orderHub.Clients.All.SendAsync("NewOrder", order.Id_Order);

            return new OkObjectResult(new { status = true, order = await LoadView(order.Id_Order) });
        }

        public async Task<IActionResult> UpdateOrder(UpdateOrder updateOrder)
        {
            var order = await _contextDb.Orders.FirstOrDefaultAsync(x => x.Id_Order == updateOrder.Id_Order);

            if (order == null)
            {
                return new NotFoundObjectResult(new { status = false, error = "Заказ не найден" });
            }

            if (updateOrder.ScheduledDate.HasValue) order.ScheduledDate = DateTime.SpecifyKind(updateOrder.ScheduledDate.Value, DateTimeKind.Utc);
            if (!string.IsNullOrEmpty(updateOrder.Status)) order.Status = updateOrder.Status;
            if (!string.IsNullOrEmpty(updateOrder.CleaningType)) order.CleaningType = updateOrder.CleaningType;
            if (updateOrder.Price.HasValue) order.Price = updateOrder.Price.Value;
            if (!string.IsNullOrEmpty(updateOrder.PaymentStatus)) order.PaymentStatus = updateOrder.PaymentStatus;
            if (updateOrder.BrigadeId.HasValue)
            {
                order.BrigadeId = updateOrder.BrigadeId;
                await _orderHub.Clients.All.SendAsync("BrigadeAssigned", order.Id_Order, updateOrder.BrigadeId.Value);
            }

            await _contextDb.SaveChangesAsync();
            await _orderHub.Clients.All.SendAsync("OrderStatusChanged", order.Id_Order, order.Status);

            return new OkObjectResult(new { status = true, order = await LoadView(order.Id_Order) });
        }

        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _contextDb.Orders.FirstOrDefaultAsync(x => x.Id_Order == id);

            if (order == null)
            {
                return new NotFoundObjectResult(new { status = false, error = "Заказ не найден" });
            }

            _contextDb.Orders.Remove(order);
            await _contextDb.SaveChangesAsync();

            return new OkObjectResult(new { status = true, order });
        }

        public async Task<IActionResult> GetOrdersByBrigade(int brigadeId)
        {
            var orders = await _contextDb.Orders
                .Include(x => x.CleaningObject)
                .Include(x => x.Brigade)
                .Include(x => x.Client)
                .Where(x => x.BrigadeId == brigadeId)
                .ToListAsync();

            return new OkObjectResult(new { status = true, orders = orders.Select(MapToView).ToList() });
        }

        public async Task<IActionResult> GetOrdersByClient(int clientId)
        {
            var orders = await _contextDb.Orders
                .Include(x => x.CleaningObject)
                .Include(x => x.Brigade)
                .Include(x => x.Client)
                .Where(x => x.ClientId == clientId)
                .ToListAsync();

            return new OkObjectResult(new { status = true, orders = orders.Select(MapToView).ToList() });
        }

        public async Task<IActionResult> GetMyOrders(string token)
        {
            var session = await _contextDb.Sessions.Include(x => x.User).FirstOrDefaultAsync(x => x.Token == token);
            if (session == null)
            {
                return new UnauthorizedObjectResult(new { status = false, error = "Сессия не найдена" });
            }

            var query = _contextDb.Orders
                .Include(x => x.CleaningObject)
                .Include(x => x.Brigade)
                .Include(x => x.Client)
                .AsQueryable();

            switch (session.User.Role_Id)
            {
                case 4:
                    query = query.Where(x => x.ClientId == session.UserId);
                    break;
                case 3:
                    var brigadeId = await _contextDb.Brigades
                        .Where(b => b.BrigadierId == session.UserId)
                        .Select(b => (int?)b.Id_Brigade)
                        .FirstOrDefaultAsync();
                    if (brigadeId == null)
                    {
                        return new OkObjectResult(new { status = true, orders = new object[0] });
                    }
                    query = query.Where(x => x.BrigadeId == brigadeId);
                    break;
            }

            var orders = await query.OrderByDescending(x => x.CreatedDate).ToListAsync();
            return new OkObjectResult(new { status = true, orders = orders.Select(MapToView).ToList() });
        }

        public async Task<IActionResult> CreateMyOrder(CreateMyOrder newOrder, string token)
        {
            var session = await _contextDb.Sessions.Include(x => x.User).FirstOrDefaultAsync(x => x.Token == token);
            if (session == null)
            {
                return new UnauthorizedObjectResult(new { status = false, error = "Сессия не найдена" });
            }

            if (session.User.Role_Id != 4)
            {
                return new ObjectResult(new { status = false, error = "Только клиент может создавать свой заказ" }) { StatusCode = 403 };
            }

            var cleaningObject = new CleaningObject
            {
                Address = newOrder.Address,
                Type = newOrder.ObjectType,
                Area = newOrder.Area,
                ClientName = session.User.Name ?? string.Empty,
                ClientPhone = session.User.Phone ?? string.Empty
            };
            await _contextDb.CleaningObjects.AddAsync(cleaningObject);
            await _contextDb.SaveChangesAsync();

            var order = new Order
            {
                CreatedDate = DateTime.UtcNow,
                ScheduledDate = DateTime.SpecifyKind(newOrder.ScheduledDate, DateTimeKind.Utc),
                Status = "Новый",
                CleaningType = newOrder.CleaningType,
                Price = CalculatePrice(newOrder.CleaningType, newOrder.Area),
                PaymentStatus = "Не оплачен",
                ObjectId = cleaningObject.Id_Object,
                ClientId = session.UserId
            };

            await _contextDb.Orders.AddAsync(order);
            await _contextDb.SaveChangesAsync();

            await _orderHub.Clients.All.SendAsync("NewOrder", order.Id_Order);

            return new OkObjectResult(new { status = true, order = await LoadView(order.Id_Order) });
        }

        public async Task<IActionResult> ChangeStatus(int orderId, string status, string token)
        {
            var session = await _contextDb.Sessions.Include(x => x.User).FirstOrDefaultAsync(x => x.Token == token);
            if (session == null)
            {
                return new UnauthorizedObjectResult(new { status = false, error = "Сессия не найдена" });
            }

            var order = await _contextDb.Orders
                .Include(x => x.Brigade)
                .FirstOrDefaultAsync(x => x.Id_Order == orderId);
            if (order == null)
            {
                return new NotFoundObjectResult(new { status = false, error = "Заказ не найден" });
            }

            var role = session.User.Role_Id;
            switch (status)
            {
                case "В работе":
                case "На приёмке":
                    if (role != 1 && role != 2 && role != 3)
                    {
                        return new ObjectResult(new { status = false, error = "Недостаточно прав" }) { StatusCode = 403 };
                    }
                    if (role == 3)
                    {
                        var myBrigade = await _contextDb.Brigades
                            .Where(b => b.BrigadierId == session.UserId)
                            .Select(b => (int?)b.Id_Brigade)
                            .FirstOrDefaultAsync();
                        if (myBrigade == null || order.BrigadeId != myBrigade)
                        {
                            return new ObjectResult(new { status = false, error = "Заказ не принадлежит вашей бригаде" }) { StatusCode = 403 };
                        }
                    }
                    break;
                case "Завершен":
                case "Отменен":
                    if (role == 4 && order.ClientId != session.UserId)
                    {
                        return new ObjectResult(new { status = false, error = "Это не ваш заказ" }) { StatusCode = 403 };
                    }
                    if (role != 1 && role != 2 && role != 4)
                    {
                        return new ObjectResult(new { status = false, error = "Недостаточно прав" }) { StatusCode = 403 };
                    }
                    break;
                default:
                    return new BadRequestObjectResult(new { status = false, error = "Неизвестный статус" });
            }

            order.Status = status;
            if (status == "Завершен" && order.PaymentStatus == "Не оплачен")
            {
                order.PaymentStatus = "Ожидает оплаты";
            }
            await _contextDb.SaveChangesAsync();

            await _orderHub.Clients.All.SendAsync("OrderStatusChanged", order.Id_Order, order.Status);

            return new OkObjectResult(new { status = true, order = await LoadView(order.Id_Order) });
        }

        public async Task<IActionResult> AssignBrigade(int orderId, int brigadeId)
        {
            var order = await _contextDb.Orders.FirstOrDefaultAsync(x => x.Id_Order == orderId);
            if (order == null)
            {
                return new NotFoundObjectResult(new { status = false, error = "Заказ не найден" });
            }

            order.BrigadeId = brigadeId;
            await _contextDb.SaveChangesAsync();

            await _orderHub.Clients.All.SendAsync("BrigadeAssigned", order.Id_Order, brigadeId);

            return new OkObjectResult(new { status = true, order = await LoadView(order.Id_Order) });
        }

        private static double CalculatePrice(string cleaningType, double area)
        {
            var rate = cleaningType switch
            {
                "Генеральная" => 250.0,
                "После ремонта" => 350.0,
                "Клининг окон" => 180.0,
                _ => 150.0
            };
            return Math.Round(rate * Math.Max(area, 1.0));
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

        private async Task<object> LoadView(int orderId)
        {
            var loaded = await _contextDb.Orders
                .Include(x => x.CleaningObject)
                .Include(x => x.Brigade)
                .Include(x => x.Client)
                .FirstAsync(x => x.Id_Order == orderId);
            return MapToView(loaded);
        }
    }
}
