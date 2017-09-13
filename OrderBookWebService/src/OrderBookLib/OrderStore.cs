using System;
using System.Collections.Generic;

namespace OrderBookLib
{
    public class OrderStore
    {
        private List<Order> _orders = new List<Order>();
        private Action<Order> _callback = null;
        private Object _callbackLock = new object();

        private int nextId = 1;

        public void PlaceOrder(string party, decimal priceLimit, Side side, int quantity)
        {
            var id = nextId++;
            var order = new Order(id, party, priceLimit, side, quantity);

            lock (_callbackLock)
            {
                _orders.Add(order);
                _callback?.Invoke(order);
            }
        }

        public void Connect(Action<Order> onOrder)
        {
            lock (_callbackLock)
            {
                _callback = onOrder;
                foreach (var order in _orders)
                {
                    _callback(order);
                }
            }
        }
    }
}
