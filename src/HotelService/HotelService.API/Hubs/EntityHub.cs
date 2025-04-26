using Microsoft.AspNetCore.SignalR;

namespace HotelService.API.Hubs
{
    public class EntityHub<T> : Hub where T : class
    {
        public async Task SendMessageAsync(T entity, CancellationToken cancellationToken)
        {
            await Clients.All.SendAsync("ReceiveMessage", entity, cancellationToken);
        }
    }
}
