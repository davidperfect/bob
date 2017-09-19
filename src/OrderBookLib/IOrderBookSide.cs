using System;
using System.Collections.Generic;
using System.Text;

namespace OrderBookLib
{
    interface IOrderBookSide
    {
        OrderBookLine PopTop();

        OrderBookLine GetTop();

        void Add(OrderBookLine line);
    }
}
