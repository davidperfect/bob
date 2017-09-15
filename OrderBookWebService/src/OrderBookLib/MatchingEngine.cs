using OrderBookLib.Events;
using OrderBookLib.EventStorage;
using System.Threading.Tasks;

namespace OrderBookLib
{
    class MatchingEngine: IEventStreamSubscriber<OrderPlaced>
    {
        OrderBook _orderBook;

        public MatchingEngine(OrderBook orderBook)
        {
            _orderBook = orderBook;
        }

        public Task HandleEventAsync(OrderPlaced anEvent)
        {
            _orderBook.AddOrder(anEvent.Order);
            var trades = _orderBook.Match();
            return Task.FromResult(0);
        }
    }
}
