using System.Collections.Generic;

namespace OrderBookWebService.ViewModels
{
    public class OrderBook
    {
        public List<AggregatedOrder> AggregatedBids { get; set; }

        public List<AggregatedOrder> AggregatedAsks { get; set; }
    }
}
