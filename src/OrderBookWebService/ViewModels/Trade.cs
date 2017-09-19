using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderBookWebService.Orders; 

namespace OrderBookWebService.ViewModels
{
    public class Trade
    {
        public int Quantity;

        public decimal Price;

        public Order BidOrder;

        public Order AskOrder;
    }
}
