using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderBookLib.EventStorage
{
    interface IOutputEventSteam<TEvent>
    {
        Task WriteEventAsync(TEvent anEvent);
    }
}
