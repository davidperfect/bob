using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrderBookWebService.Orders;
using OrderBookLib.EventStorage;
using OrderBookLib.Events;
using System.Threading;

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

        // GET api/orders
        [HttpGet]
        public IEnumerable<string> Get()
        {
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
