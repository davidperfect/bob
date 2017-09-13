using System;
using System.Collections.Generic;
using System.Text;

namespace OrderBookLib
{
    class Exchange
    {

        OrderStore _orderStore;
        OrderBook _orderBook;

        Exchange(OrderStore orderStore)
        {
            _orderStore = orderStore;
            _orderBook = new OrderBook();
            MatchingEngine me = new MatchingEngine(_orderStore, _orderBook);

        }

        //TODO: Return a version that will be integrated in the event feed
        void Bid(string party, decimal priceLimit, int quantity)
        {
            _orderStore.PlaceOrder(party, priceLimit, Side.Bid, quantity);
        }

        void Ask(string party, decimal priceLimit, int quantity)
        {
            _orderStore.PlaceOrder(party, priceLimit, Side.Ask, quantity);
        }

        //TODO: Pass a version that will block the call until the state is updated
        public IReadOnlyCollection<OrderBookLine> GetTopBids(int quantity)
        {
            return _orderBook.GetTopBids(quantity);
        }

        public IReadOnlyCollection<OrderBookLine> GetTopAsks(int quantity)
        {
            return _orderBook.GetTopAsks(quantity);
        }


    }
}
