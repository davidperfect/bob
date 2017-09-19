using System.Collections.Generic;

namespace OrderBookWebService.ViewModels
{
    public class OrderBook
    {
        public List<PendingOrder> PendingBids { get; set; }

        public List<PendingOrder> PendingAsks { get; set; }
    }
}
