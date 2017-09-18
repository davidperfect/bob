using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderBookWebService.ViewModels
{
    public class AggregatedOrder
    {
        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
