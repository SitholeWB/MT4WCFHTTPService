using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MT4WCFHTTPService.FXModes;
using MT4WCFHTTPService.Helpers;
using MtApi;

namespace MT4WCFHTTPService
{
	public class Service : IService
	{
		private static MtApiClient mtApiClient = new MtApiClient();

		public Service()
		{
			int i = 0;
			while (i < 5 && mtApiClient.ConnectionState != MtConnectionState.Connected)
			{
				i++;
				try
				{
					mtApiClient.BeginConnect("localhost", 8222);
				}
				catch (Exception e)
				{
					Console.WriteLine(DateTime.Now + " <<>> " + e.Message);
					Task.Delay(70000);
					Console.WriteLine(DateTime.Now + " <<!!>> " + e.Message);
				}
			}
		}

		public double AccountBalance()
		{
			RetryConnecting();
			return mtApiClient.AccountBalance();
		}

		public string LastErrorDescription()
		{
			RetryConnecting();
			return ((MtErrorCode)mtApiClient.GetLastError()).ToString();
		}

		public List<MtOrder> OrdersOpened()
		{
			RetryConnecting();
			return mtApiClient.GetOrders(OrderSelectSource.MODE_TRADES);
		}

		public double MarketInfoSPREAD(string symbol)
		{
			RetryConnecting();
			return mtApiClient.MarketInfo(symbol, MarketInfoModeType.MODE_SPREAD);
		}

		public bool OrderClose(int ticket, int slippage)
		{
			RetryConnecting();
			return mtApiClient.OrderClose(ticket: ticket, slippage: slippage);
		}

		public bool OrderCloseAll()
		{
			RetryConnecting();
			return mtApiClient.OrderCloseAll();
		}

		public bool OrderModify(int ticket, double price, double stoploss, double takeprofit, string expirationDate)
		{
			RetryConnecting();
			var date = DateTime.ParseExact(expirationDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
			return mtApiClient.OrderModify(ticket: ticket, price: price, stoploss: stoploss, takeprofit: takeprofit, expiration: date);
		}

		public FXModes.MqlTick SymbolInfoTick(string symbol)
		{
			RetryConnecting();
			var infoTick = mtApiClient.SymbolInfoTick(symbol);
			return new FXModes.MqlTick
			{
				ask = infoTick.Ask,
				bid = infoTick.Bid,
				volume = infoTick.Volume,
				MtTime = infoTick.MtTime,
				last = infoTick.Last,
				time = infoTick.Time
			};
		}

		public DateTime ServerTimeCurrent()
		{
			RetryConnecting();
			return mtApiClient.TimeCurrent();
		}

		public int OrderSendSell(string symbol, double volume, double price, int slippage, double stoploss, double takeprofit, string comment)
		{
			RetryConnecting();
			return mtApiClient.OrderSend(symbol, TradeOperation.OP_SELL, volume, price, slippage, stoploss, takeprofit, comment);
		}

		public int OrderSendBuy(string symbol, double volume, double price, int slippage, double stoploss, double takeprofit, string comment)
		{
			RetryConnecting();
			return mtApiClient.OrderSend(symbol, TradeOperation.OP_BUY, volume, price, slippage, stoploss, takeprofit, comment);
		}

		public double SymbolInfoDouble(string symbol)
		{
			RetryConnecting();
			return mtApiClient.SymbolInfoDouble(symbol, EnumSymbolInfoDouble.SYMBOL_POINT);
		}

		public List<FXModes.MqlRates> RatesByPositions(string symbol, string timeframe, int startPosition, int count)
		{
			RetryConnecting();
			if (count <= 0 || startPosition < 0)
			{
				return new List<FXModes.MqlRates>();
			}
			ENUM_TIMEFRAMES enumTimeframe = (ENUM_TIMEFRAMES)Enum.Parse(typeof(ENUM_TIMEFRAMES), timeframe);
			var candles = mtApiClient.CopyRates(symbol, enumTimeframe, startPosition, count);

			//This is done so that time is not ignored from xml
			var newCandles = new List<FXModes.MqlRates>();
			foreach (var m in candles)
			{
				newCandles.Add(new FXModes.MqlRates
				{
					mt_time = m.MtTime,
					close = m.Close,
					high = m.High,
					low = m.Low,
					open = m.Open,
					real_volume = m.RealVolume,
					spread = m.Spread,
					tick_volume = m.TickVolume,
					time = m.Time
				});
			}
			return newCandles;
		}

		public List<FXModes.MqlRates> RatesByDates(string symbol, string timeframe, string startDateString, string endDateString)
		{
			RetryConnecting();
			var startDate = DateTime.ParseExact(startDateString, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
			var endDate = DateTime.ParseExact(endDateString, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);

			if ((endDate - startDate).TotalMinutes <= TimeframeHelper.GetMinutesFromForTimeframe(timeframe))
			{
				return new List<FXModes.MqlRates>();
			}
			if ((mtApiClient.TimeCurrent() - startDate).TotalMinutes <= TimeframeHelper.GetMinutesFromForTimeframe(timeframe))
			{
				return new List<FXModes.MqlRates>();
			}

			ENUM_TIMEFRAMES enumTimeframe = (ENUM_TIMEFRAMES)Enum.Parse(typeof(ENUM_TIMEFRAMES), timeframe);
			var candles = mtApiClient.CopyRates(symbol, enumTimeframe, startDate, endDate);

			//This is done so that time is not ignored from xml
			var newCandles = new List<FXModes.MqlRates>();
			foreach (var m in candles)
			{
				newCandles.Add(new FXModes.MqlRates
				{
					mt_time = m.MtTime,
					close = m.Close,
					high = m.High,
					low = m.Low,
					open = m.Open,
					real_volume = m.RealVolume,
					spread = m.Spread,
					tick_volume = m.TickVolume,
					time = m.Time
				});
			}
			return newCandles;
		}

		public FXModes.MqlRates CurrentIncompleteCandle(string symbol, string timeframe)
		{
			RetryConnecting();
			ENUM_TIMEFRAMES enumTimeframe = (ENUM_TIMEFRAMES)Enum.Parse(typeof(ENUM_TIMEFRAMES), timeframe);
			var candles = mtApiClient.CopyRates(symbol, enumTimeframe, 0, 1);

			var m = candles[0];
			//This is done so that time is not ignored from xml
			return new FXModes.MqlRates
			{
				mt_time = m.MtTime,
				close = m.Close,
				high = m.High,
				low = m.Low,
				open = m.Open,
				real_volume = m.RealVolume,
				spread = m.Spread,
				tick_volume = m.TickVolume,
				time = m.Time
			};
		}

		#region private methods

		private static void RetryConnecting()
		{
			int i = 0;
			while (i < 5 && mtApiClient.ConnectionState != MtConnectionState.Connected)
			{
				i++;
				try
				{
					mtApiClient.BeginConnect("localhost", 8222);
					mtApiClient.TimeCurrent();
				}
				catch (Exception e)
				{
					Console.WriteLine(DateTime.Now + " <<>> " + e.Message);
					Task.Delay(5000);
					Console.WriteLine(DateTime.Now + " <<!!>> " + e.Message);
				}
			}
		}

		#endregion private methods
	}
}