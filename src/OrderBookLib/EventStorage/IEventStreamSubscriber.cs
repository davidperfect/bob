using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderBookLib.EventStorage
{
    public interface IEventStreamSubscriber<TEvent>
    {
        Task HandleEventAsync(TEvent anEvent);
    }
}
