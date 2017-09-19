using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OrderBookLib.Events;
using OrderBookLib.EventStorage;
using OrderBookWebService.Orders;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OrderBookWebService
{
    public class ExchangeFactory
    {
        static Task runTask;

        public static OrderBookLib.Exchange CreateExchange(IHubContext<TradesHub> hub)
        {
            IMessageSerializer<OrderPlaced> serializer = new JsonMessageSerializer<OrderPlaced>();
            var orderStore = new EventStore<OrderPlaced>("OrderPlaced.txt", serializer);
            var exchange = new OrderBookLib.Exchange(orderStore);
            exchange.OnNewTrades += (IReadOnlyCollection<OrderBookLib.Trade> trades) => HandleNewTrades(hub, trades);
            exchange.OnOrderBookUpdated += HandleOrderBookUpdated;
            var cts = new CancellationTokenSource();
            runTask = exchange.RunAsync(cts.Token);
            return exchange;
        }

        static void HandleOrderBookUpdated()
        {
        }

        static void HandleNewTrades(IHubContext<TradesHub> hub, IReadOnlyCollection<OrderBookLib.Trade> trades)
        {
            var tradeViewModels = trades.Select(x => new ViewModels.Trade()
            {
                AskOrder = new Order()
                {
                    Party = x.AskOrder.Party,
                    Quantity = x.AskOrder.Quantity,
                    PriceLimit = x.AskOrder.PriceLimit,
                    Side = x.AskOrder.Side.ToString()
                },
                BidOrder = new Order()
                {
                    Party = x.BidOrder.Party,
                    Quantity = x.BidOrder.Quantity,
                    PriceLimit = x.BidOrder.PriceLimit,
                    Side = x.BidOrder.Side.ToString()
                },
                Quantity = x.Quantity,
                Price = x.Price
            });

            hub.Clients.All.InvokeAsync("trade", tradeViewModels)
                .Wait();


            //var trade = GenerateRandomOrder("ask");
            //trade.Quantity = 99999;
            //hub.Clients.All.InvokeAsync("trade", new List<ViewModels.Trade>
            //{
            //    new ViewModels.Trade()
            //    {
            //        AskOrder = trade,
            //        BidOrder = trade,
            //        Price = 999999,
            //        Quantity = 999
            //    }
            //}).Wait();
        }

        private static Order GenerateRandomOrder(string side)
        {
            var r = new Random();
            return new Order
            {
                Party = Parties[r.Next(0, Parties.Length)],
                PriceLimit = r.Next(1, 1000),
                Quantity = r.Next(1, 50),
                Side = side
            };
        }

        private static string[] Parties = "Noah,Liam,Sophia,Mason,Ava,Jacob,William,Isabella,Ethan,Mia,James,Alexander,Michael,Logan,Benjamin,Elijah,Aiden,Daniel,Matthew,Abigail,Lucas,Jackson" .Split(',');
    }
}
