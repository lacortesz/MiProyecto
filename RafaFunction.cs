using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NinjaTrader.Cbi;
using NinjaTrader.Gui;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Gui.SuperDom;
using NinjaTrader.Gui.Tools;
using NinjaTrader.Data;
using NinjaTrader.NinjaScript;
using NinjaTrader.Core.FloatingPoint;
using NinjaTrader.NinjaScript.Indicators;
using NinjaTrader.NinjaScript.DrawingTools;

namespace NinjaTrader.Custom.Strategies
{
    class Functions
    {
        protected string RedOrGreenBar(Bar bar)
        {
            string result = "Green";
            if (bar.Close > bar.Open)
            {
                return "Red";
            }
            return result;
        }

    }
}
