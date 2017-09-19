using System;
using System.Collections.Generic;
using System.Text;

namespace OrderBookLib
{
    public class Delegates
    {
        public delegate void OnNewTradesDelegate(IReadOnlyCollection<Trade> trades);
        public delegate void OnOrderBookUpdatedDelegate(IReadOnlyCollection<OrderBookLine> orderBookAskLines, IReadOnlyCollection<OrderBookLine> orderBookBidLines);

    }
}
