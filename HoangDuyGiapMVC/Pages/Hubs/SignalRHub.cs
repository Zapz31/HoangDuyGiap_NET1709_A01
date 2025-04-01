using Microsoft.AspNetCore.SignalR;

namespace HoangDuyGiapMVC.Pages.Hubs
{
    public class SignalRHub : Hub
    {
        public async Task NotifyChange()
        {
            await Clients.All.SendAsync("Change");
        }
    }
}
