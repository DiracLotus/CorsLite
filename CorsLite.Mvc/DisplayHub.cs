using CorsLite.Mvc.Models;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace CorsLite.Mvc.Web
{
    /// <summary>
    /// SignalR hub for pushing messages to the client to keep them informed
    /// as to how their drink is coming along.
    /// </summary>
    public class DisplayHub : Hub
    {
        public async Task SendMessage(NotificationRequest request)
        {
            await Clients.All.SendAsync("ReceiveMessage", request);
        }
    }
}
