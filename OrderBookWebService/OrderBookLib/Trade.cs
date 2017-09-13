using System;
using System.Collections.Generic;
using System.Text;

namespace OrderBookLib
{
    class Trade
    {
        public int Quantity;

        public decimal Price;

        public Order BidOrder;

        public Order AskOrder;
    }
}
