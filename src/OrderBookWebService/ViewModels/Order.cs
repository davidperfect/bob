namespace OrderBookWebService.Orders
{
    public class Order
    {
       // public int Id { get;  set; }

        public string Party { get;  set; }

        public decimal PriceLimit { get;  set; }

        public string Side { get;  set; }

        public int Quantity { get;  set; }
    }
}
