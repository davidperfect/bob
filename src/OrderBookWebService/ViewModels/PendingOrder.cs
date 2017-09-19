using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderBookWebService.ViewModels
{
    public class PendingOrder
    {
        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public int RemainingQuantity { get; set; }

        public string Party { get; set; }
    }
}
