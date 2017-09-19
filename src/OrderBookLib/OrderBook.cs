using OrderBookLib.Synchonization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderBookLib
{
    class OrderBook
    {
        private BidOrderBook _bidOrderBook = new BidOrderBook();
        private AskOrderBook _askOrderBook = new AskOrderBook();
        private VersionStatus _orderVersionStatus = new VersionStatus();

        public void AddOrder(Order order)
        {
            IOrderBookSide orderBookSide = GetOrderBookSide(order.Side);
            orderBookSide.Add(new OrderBookLine
            {
                Order = order,
                RemainingQuantity = order.Quantity
            });

            _orderVersionStatus.Update(order.Id);
        }

        public async Task<IReadOnlyCollection<OrderBookLine>> GetTopBidsAsync(int version, int quantity)
        {
            await _orderVersionStatus.WaitForVersionAsync(version);

            return _bidOrderBook.GetTops(quantity).ToList();
        }

        public IReadOnlyCollection<OrderBookLine> GetTopBids(int quantity)
        {
            return _bidOrderBook.GetTops(quantity).ToList();
        }

        public IReadOnlyCollection<OrderBookLine> GetTopAsks(int quantity)
        {
            return _askOrderBook.GetTops(quantity).ToList();
        }

        public IEnumerable<Trade> Match()
        {
            var topBid = _bidOrderBook.PopTop();
            var topAsk = _askOrderBook.PopTop();

            while (topBid != null && topAsk != null && topBid.Order.PriceLimit >= topAsk.Order.PriceLimit)
            {
                var matchedQuantity = Math.Min(topBid.RemainingQuantity, topAsk.RemainingQuantity);
                var matchedPrice = topBid.Order.Id < topAsk.Order.Id
                    ? topBid.Order.PriceLimit
                    : topAsk.Order.PriceLimit;

                topBid.RemainingQuantity -= matchedQuantity;
                topAsk.RemainingQuantity -= matchedQuantity;

                if (topBid.RemainingQuantity > 0)
                {
                    _bidOrderBook.Add(topBid);
                }

                if (topAsk.RemainingQuantity > 0)
                {
                    _askOrderBook.Add(topAsk);
                }

                yield return new Trade
                {
                    Quantity = matchedQuantity,
                    Price = matchedPrice,
                    BidOrder = topBid.Order,
                    AskOrder = topAsk.Order
                };

                topBid = _bidOrderBook.PopTop();
                topAsk = _askOrderBook.PopTop();
            }


            if (topBid != null && topBid.RemainingQuantity > 0)
            {
                _bidOrderBook.Add(topBid);
            }

            if (topAsk != null && topAsk.RemainingQuantity > 0)
            {
                _askOrderBook.Add(topAsk);
            }
        }

        private IOrderBookSide GetOrderBookSide(Side side)
        {
            if (side == Side.Ask)
            {
                return _askOrderBook;
            }

            return _bidOrderBook;
        }
    }
}
