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
        private static Lazy<OrderBookLib.Exchange> _exchange = new Lazy<OrderBookLib.Exchange>(CreateExchange);
        private static OrderBookLib.Exchange CreateExchange()
        {
            IMessageSerializer<OrderPlaced> serializer = new JsonMessageSerializer<OrderPlaced>();
            var orderStore = new EventStore<OrderPlaced>("OrderPlaced.txt", serializer);
            var exchange = new OrderBookLib.Exchange(orderStore);
            var cts = new CancellationTokenSource();
            var runTask = exchange.RunAsync(cts.Token);
            return exchange;
        }

        IHubContext<OrderBookHub> _orderBookHubContext;

        public OrdersController(IHubContext<OrderBookHub> orderBookHubContext)
        {
            _orderBookHubContext = orderBookHubContext;
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

            return await _exchange.Value.PlaceOrder(order.Party, order.PriceLimit, side, order.Quantity);
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
