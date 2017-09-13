using System;
using System.Collections.Generic;
using System.Text;

namespace OrderBookLib
{
    class MatchingEngine
    {
        OrderBook _orderBook;
        public MatchingEngine(OrderStore orderStore, OrderBook orderBook)
        {
            _orderBook = orderBook;
            orderStore.Connect(OnOrder);
        }

        public void OnOrder(Order order)
        {
            _orderBook.AddOrder(order);
            var trades = _orderBook.Match();
            
        }
    }
}
