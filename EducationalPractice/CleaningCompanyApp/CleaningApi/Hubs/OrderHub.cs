using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace CleaningApi.Hubs
{
    public class OrderHub : Hub
    {
        public async Task NotifyNewOrder(int orderId)
        {
            await Clients.All.SendAsync("NewOrder", orderId);
        }

        public async Task NotifyOrderStatusChanged(int orderId, string status)
        {
            await Clients.All.SendAsync("OrderStatusChanged", orderId, status);
        }

        public async Task NotifyBrigadeAssigned(int orderId, int brigadeId)
        {
            await Clients.All.SendAsync("BrigadeAssigned", orderId, brigadeId);
        }
    }
}
