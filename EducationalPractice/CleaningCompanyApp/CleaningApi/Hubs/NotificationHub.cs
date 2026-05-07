using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace CleaningApi.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification(string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", message);
        }

        public async Task NotifyBrigade(int brigadeId, string message)
        {
            await Clients.Group($"Brigade_{brigadeId}").SendAsync("ReceiveNotification", message);
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var brigadeId = httpContext.Request.Query["brigadeId"];
            
            if (!string.IsNullOrEmpty(brigadeId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"Brigade_{brigadeId}");
            }

            await base.OnConnectedAsync();
        }
    }
}
