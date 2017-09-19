using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OrderBookLib.Events;
using OrderBookLib.EventStorage;
using OrderBookWebService.Orders;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OrderBookWebService.Controllers
{
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        IHubContext<OrderBookHub> _orderBookHubContext;
        private static OrderBookLib.Exchange _exchange;

        public OrdersController(IHubContext<OrderBookHub> orderBookHubContext, OrderBookLib.Exchange exchange)
        {
            _orderBookHubContext = orderBookHubContext;
            _exchange = exchange;
        }

        // GET api/orders
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            var ob = new ViewModels.OrderBook
            {
                AggregatedAsks = new List<ViewModels.AggregatedOrder>
                {
                    new ViewModels.AggregatedOrder
                    {
                        Price = 20.5m,
                        Quantity = 3
                    }
                },
                AggregatedBids = new List<ViewModels.AggregatedOrder>
                {
                    new ViewModels.AggregatedOrder
                    {
                        Price = 19.5m,
                        Quantity = 5
                    }
                }
            };

            await _orderBookHubContext.Clients.All.InvokeAsync("Send", ob);

            return new string[] { "value1", "value2" };
        }

        // GET api/orders/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/orders
        [HttpPost]
        public async Task<int> Post([FromBody]Order order)
        {
            var side = order.Side.ToLower() == "bid"
                ? OrderBookLib.Side.Bid
                : OrderBookLib.Side.Ask;

            return await _exchange.PlaceOrder(order.Party, order.PriceLimit, side, order.Quantity);
        }

        // PUT api/orders/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/orders/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
