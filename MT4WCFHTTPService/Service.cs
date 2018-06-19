using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
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

		public string GetLastErrorDescription()
		{
			RetryConnecting();
			return ((MtErrorCode)mtApiClient.GetLastError()).ToString();
		}

		public List<MtOrder> GetOrdersOpened()
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

		public MqlTick SymbolInfoTick(string symbol)
		{
			RetryConnecting();
			return mtApiClient.SymbolInfoTick(symbol);
		}

		public DateTime TimeCurrent()
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