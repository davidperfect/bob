using System;

namespace OrderBookLib
{
    public class Order
    {
        public Order(int id, string party, decimal priceLimit, Side side, int quantity)
        {
            Id = id;
            Party = party;
            PriceLimit = priceLimit;
            Side = side;
            Quantity = quantity;
        }

        public int Id { get; private set; }

        public string Party { get; private set; }

        public decimal PriceLimit { get; private set; }

        public Side Side { get; private set; }

        public int Quantity { get; private set; }
    }
}
