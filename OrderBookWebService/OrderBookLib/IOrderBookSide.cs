using System;
using System.Collections.Generic;
using System.Text;

namespace OrderBookLib
{
    interface IOrderBookSide
    {
        OrderBookLine GetTop();

        void Add(OrderBookLine line);
    }
}
