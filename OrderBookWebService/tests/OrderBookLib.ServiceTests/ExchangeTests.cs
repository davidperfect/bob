using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace OrderBookLib.ServiceTests
{
    [TestClass]
    public class ExchangeTests
    {
        [TestMethod]
        public void Bid_shows_up_in_top_bids()
        {
            var orderStore = new OrderStore();
            var exchange = new Exchange(orderStore);

            string party = "party";
            decimal priceLimit = 10.0m;
            int quantity = 12;
            exchange.Bid(party, priceLimit, quantity);

            // Act
            var result = exchange.GetTopBids(1);

            // Assert
            var orderBookLine = result.Single();
            Assert.AreEqual(party, orderBookLine.Order.Party);
            Assert.AreEqual(priceLimit, orderBookLine.Order.PriceLimit);
            Assert.AreEqual(quantity, orderBookLine.Order.Quantity);
            Assert.AreEqual(quantity, orderBookLine.RemainingQuantity);
        }
    }
}
