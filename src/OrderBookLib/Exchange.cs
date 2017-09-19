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

        public Delegates.OnNewTradesDelegate OnNewTrades;
        public Delegates.OnOrderBookUpdatedDelegate OnOrderBookUpdated;

        private int nextId = 1;

        public Exchange(EventStore<OrderPlaced> orderStore)
        {
            _orderStore = orderStore;
            _orderBook = new OrderBook();

        }

        public Task RunAsync(CancellationToken ct)
        {
            MatchingEngine me = new MatchingEngine(_orderBook);
            me.OnNewTrades += HandleNewTrades;
            me.OnOrderBookUpdated += HandleOrderBookUpdated;
            return _orderStore.ReadAsync(me, ct);
        }

        private void HandleNewTrades(IReadOnlyCollection<Trade> trades)
        {
            OnNewTrades?.Invoke(trades);
        }

        private void HandleOrderBookUpdated(IReadOnlyCollection<OrderBookLine> orderBookAskLines, IReadOnlyCollection<OrderBookLine> orderBookBidLines)
        {
            OnOrderBookUpdated?.Invoke(orderBookAskLines, orderBookBidLines);
        }

        public async Task<int> PlaceOrder(string party, decimal priceLimit, Side side, int quantity)
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
