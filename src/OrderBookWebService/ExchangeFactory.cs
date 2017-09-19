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
            exchange.OnNewTrades += HandleNewTrades;
            exchange.OnOrderBookUpdated += HandleOrderBookUpdated;
            var cts = new CancellationTokenSource();
            runTask = exchange.RunAsync(cts.Token);
            return exchange;
        }

        static void HandleOrderBookUpdated()
        {

        }

        static void HandleNewTrades(IReadOnlyCollection<OrderBookLib.Trade> trades)
        {

        }
    }
}
