using System;
using System.Collections.Generic;
using System.Text;

namespace OrderBookLib
{
    public static class SideExtensions
    {
        public static Side Opposite(this Side side)
        {
            return side == Side.Ask ? Side.Bid : Side.Ask;
        }
    }
}
