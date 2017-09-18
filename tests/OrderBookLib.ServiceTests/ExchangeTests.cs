using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using OrderBookLib.Events;
using OrderBookLib.EventStorage;
using System.Threading.Tasks;
using System.Threading;

namespace OrderBookLib.ServiceTests
{
    [TestClass]
    public class ExchangeTests
    {
        [TestMethod]
        public async Task Bid_shows_up_in_top_bids()
        {
            IMessageSerializer<OrderPlaced> serializer = new JsonMessageSerializer<OrderPlaced>();
            var orderStore = new EventStorage.EventStore<OrderPlaced>("OrderPlaced.txt", serializer);
            var exchange = new Exchange(orderStore);
            var cts = new CancellationTokenSource();
            var runTask = exchange.RunAsync(cts.Token);

            string party = "party";
            decimal priceLimit = 10.0m;
            int quantity = 12;
            var version = await exchange.Bid(party, priceLimit, quantity);

            // Act
            var result = await exchange.GetTopBidsAsync(version, 1);
            cts.Cancel();
            runTask.Wait();

            // Assert
            var orderBookLine = result.Single();
            Assert.AreEqual(party, orderBookLine.Order.Party);
            Assert.AreEqual(priceLimit, orderBookLine.Order.PriceLimit);
            Assert.AreEqual(quantity, orderBookLine.Order.Quantity);
            Assert.AreEqual(quantity, orderBookLine.RemainingQuantity);
        }
    }
}
