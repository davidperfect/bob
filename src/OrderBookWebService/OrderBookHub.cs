using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace OrderBookWebService
{
    public class OrderBookHub : Hub
    {
        public OrderBookHub()
        {

        }

        public Task Send(string message)
        {
            return Clients.All.InvokeAsync("Send", message);
        }
    }
}
