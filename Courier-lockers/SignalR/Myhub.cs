using Microsoft.AspNetCore.SignalR;

namespace Courier_lockers.SignalR
{
    public class Myhub :Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
