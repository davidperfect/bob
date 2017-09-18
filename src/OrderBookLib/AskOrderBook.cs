using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderBookLib
{
    class AskOrderBook : IOrderBookSide
    {
        List<OrderBookLine> _orders = new List<OrderBookLine>();

        public void Add(OrderBookLine line)
        {
            _orders.Add(line);
        }

        public OrderBookLine GetTop()
        {
            return _orders
                .OrderBy(x => x.Order.PriceLimit)
                .ThenBy(x => x.Order.Id)
                .FirstOrDefault();
        }

        public IEnumerable<OrderBookLine> GetTops(int quantity)
        {
            int quantitySum = 0;
            var enumerator = _orders
                .OrderBy(x => x.Order.PriceLimit)
                .ThenBy(x => x.Order.Id)
                .GetEnumerator();

            while (quantitySum < quantity && enumerator.MoveNext())
            {
                quantitySum += enumerator.Current.RemainingQuantity;
                yield return enumerator.Current;
            }
        }
    }
}
