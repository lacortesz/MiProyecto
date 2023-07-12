//
// Copyright (C) 2021, NinjaTrader LLC <www.ninjatrader.com>.
// NinjaTrader reserves the right to modify or overwrite this NinjaScript component with each release.
//
#region Using declarations
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Gui;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Gui.SuperDom;
using NinjaTrader.Data;
using NinjaTrader.NinjaScript;
using NinjaTrader.Core.FloatingPoint;
using NinjaTrader.NinjaScript.Indicators;
using NinjaTrader.NinjaScript.DrawingTools;
#endregion

//This namespace holds strategies in this folder and is required. Do not change it.
namespace NinjaTrader.NinjaScript.Strategies
{

	public class SampleMACrossOver2 : Strategy

	{
		private SMA smaFast;
		private SMA smaSlow;

		int[] moves = new int[4];

		private Order longEntryOrder;
		private Order longTakeProfitOrder;
		private Order longStopLossOrder;
		private Order shortEntryOrder;
		private Order shortTakeProfitOrder;
		private Order shortStopLossOrder;

		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description	= NinjaTrader.Custom.Resource.NinjaScriptStrategyDescriptionSampleMACrossOver;
				Name		= "Rafa";
				Fast		= 10;
				Slow		= 25;
				// This strategy has been designed to take advantage of performance gains in Strategy Analyzer optimizations
				// See the Help Guide for additional information
				IsInstantiatedOnEachOptimizationIteration = false;
			}
			else if (State == State.Configure)
			{
				AddDataSeries(BarsPeriodType.Minute, 5);   // 4 hours
				AddDataSeries(BarsPeriodType.Minute, 15);  // 1 Day
				AddDataSeries(BarsPeriodType.Minute, 60);  // 1 Week

			}
			else if (State == State.DataLoaded)
			{
				smaFast = SMA(Fast);
				smaSlow = SMA(Slow);

				smaFast.Plots[0].Brush = Brushes.Goldenrod;
				smaSlow.Plots[0].Brush = Brushes.SeaGreen;

				AddChartIndicator(smaFast);
				AddChartIndicator(smaSlow);
			}
		}

		protected override void OnBarUpdate()
		{
			if (BarsInProgress == 3)
			{
				if (CurrentBars[3] > 1)
				{
					moves[3] = DefineMovement(SMA(BarsArray[3], Fast)[0], SMA(BarsArray[3], Slow)[0]);
					//Print("major timeframe bar #:" + CurrentBars[3]);
				}
			}

			if (BarsInProgress == 2)
			{
				if (CurrentBars[2] > 1)
				{
					moves[2] = DefineMovement(SMA(BarsArray[2], Fast)[0], SMA(BarsArray[2], Slow)[0]);
					//Print("mediun timeframe bar #:" + CurrentBars[2]);
				}
			}

			if (BarsInProgress == 1)
			{
				if (CurrentBars[1] > 1)
				{
					moves[1] = DefineMovement(SMA(BarsArray[1], Fast)[0], SMA(BarsArray[1], Slow)[0]);
					//Print("minor timeframe bar #:" + CurrentBars[1]);
				}
				if (CurrentBars[3] > 1)
				Print(BarsArray[0].GetTime(BarsArray[0].Count) + " TF Week: " + moves[3] + " TF Day: " + moves[2] + " TF 4 hours: " + moves[1]);
			}

			if (CurrentBars[3] > 1)
			{
				int movesSum = moves[1] + moves[2] + moves[3];

				if (movesSum == 3 && CrossAbove(smaFast, smaSlow, 1))
					EnterLong();
					//longEntryOrder = EnterLongStopMarket(0, true, 1, GetCurrentAsk(), "longEntryOrder");
				else if (movesSum == -3 && CrossBelow(smaFast, smaSlow, 1))
					EnterShort();
					//shortEntryOrder = EnterShortStopMarket(0, true, 1, GetCurrentBid(), "shortEntryOrder"); // Entry Order
			} 

			/*
			if (CrossAbove(smaFast, smaSlow, 1))
				EnterLong();
			else if(CrossBelow(smaFast, smaSlow, 1))
				EnterShort(); */

		}



		protected int DefineMovement (double fast, double slow)
        {
			if (slow >= fast)
				return 1;
			else
				return -1;
        }




		#region Properties
		[Range(1, int.MaxValue), NinjaScriptProperty]
		[Display(ResourceType = typeof(Custom.Resource), Name = "Fast", GroupName = "NinjaScriptStrategyParameters", Order = 0)]
		public int Fast
		{ get; set; }

		[Range(1, int.MaxValue), NinjaScriptProperty]
		[Display(ResourceType = typeof(Custom.Resource), Name = "Slow", GroupName = "NinjaScriptStrategyParameters", Order = 1)]
		public int Slow
		{ get; set; }
		#endregion
	}
}
