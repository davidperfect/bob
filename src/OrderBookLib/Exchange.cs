using OrderBookLib.Events;
using OrderBookLib.EventStorage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrderBookLib
{
    public class Exchange
    {

        EventStore<OrderPlaced> _orderStore;
        OrderBook _orderBook;

        private int nextId = 1;


        public Exchange(EventStore<OrderPlaced> orderStore)
        {
            _orderStore = orderStore;
            _orderBook = new OrderBook();

        }

        public Task RunAsync(CancellationToken ct)
        {
            MatchingEngine me = new MatchingEngine(_orderBook);
            return _orderStore.ReadAsync(me, ct);
        }

        //TODO: Return a version that will be integrated in the event feed
        public Task<int> Bid(string party, decimal priceLimit, int quantity)
        {

            return PlaceOrder(party, priceLimit, Side.Bid, quantity);
        }

        public Task<int> Ask(string party, decimal priceLimit, int quantity)
        {
            return PlaceOrder(party, priceLimit, Side.Ask, quantity);
        }

        private async Task<int> PlaceOrder(string party, decimal priceLimit, Side side, int quantity)
        {
            var id = nextId++;
            var order = new Order(id, party, priceLimit, side, quantity);

            var orderPlacedEvent = new OrderPlaced
            {
                Order = order
            };

            await _orderStore.WriteEventAsync(orderPlacedEvent);

            return id;
        }

        public IReadOnlyCollection<OrderBookLine> GetTopBids(int quantity)
        {
            return _orderBook.GetTopBids(quantity);
        }

        public async Task<IReadOnlyCollection<OrderBookLine>> GetTopBidsAsync(int version, int quantity)
        {
            return await _orderBook.GetTopBidsAsync(version, quantity);
        }

        public IReadOnlyCollection<OrderBookLine> GetTopAsks(int quantity)
        {
            return _orderBook.GetTopAsks(quantity);
        }
    }
}
