using OrderBookLib.Events;
using OrderBookLib.EventStorage;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderBookLib
{
    class MatchingEngine : IEventStreamSubscriber<OrderPlaced>
    {
        OrderBook _orderBook;

        public Delegates.OnNewTradesDelegate OnNewTrades;
        public Delegates.OnOrderBookUpdatedDelegate OnOrderBookUpdated;

        public MatchingEngine(OrderBook orderBook)
        {
            _orderBook = orderBook;
        }

        public Task HandleEventAsync(OrderPlaced anEvent)
        {
            _orderBook.AddOrder(anEvent.Order);
            var trades = _orderBook.Match().ToList();
            if (trades.Any())
            {
                OnNewTrades?.Invoke(trades);
            }
            OnOrderBookUpdated?.Invoke();

            return Task.FromResult(0);
        }
    }
}
