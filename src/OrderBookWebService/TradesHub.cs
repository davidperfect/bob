using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace OrderBookWebService
{
    public class TradesHub : Hub
    {
        public TradesHub()
        {
        }

        public Task Trade(object o)
        {
            return Clients.All.InvokeAsync("Trade", o); 
        }
    }
}
