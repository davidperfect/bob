using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OrderBookWebService.Orders;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OrderBookWebService.Controllers
{
    [Route("api/[controller]")]
    public class TradesController : Controller
    {
        private readonly IHubContext<TradesHub> _tradesHub;

        public TradesController(IHubContext<TradesHub> tradesHub)
        {
            _tradesHub = tradesHub;
        }

        // GET: api/trades
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            var ask = GenerateRandomOrder("ask");
            var bid = GenerateRandomOrder("bid"); 
            var trade = new ViewModels.Trade
            {
                AskOrder = ask,
                BidOrder = bid,
                Quantity = Math.Min(ask.Quantity, bid.Quantity),
                Price = bid.PriceLimit
            };

            await _tradesHub.Clients.All.InvokeAsync("Trade", trade);

            return new string[] { "value1", "value2" };
        }

        private Order GenerateRandomOrder(string side)
        {
            var r = new Random();
            return new Order
            {
                Party = Parties[r.Next(0, Parties.Length)],
                PriceLimit = r.Next(1, 1000),
                Quantity = r.Next(1, 50),
                Side = side
            };
        }

        private static string[] Parties =
            "Noah,Olivia,Liam,Sophia,Mason,Ava,Jacob,William,Isabella,Ethan,Mia,James,Alexander,Michael,Logan,Benjamin,Elijah,Aiden,Daniel,Matthew,Abigail,Lucas,Jackson"
                .Split(',');
    }
}
