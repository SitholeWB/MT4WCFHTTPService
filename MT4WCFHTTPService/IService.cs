using MtApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace MT4WCFHTTPService
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
	[ServiceContract]
	public interface IService
	{
		[OperationContract]
		[WebGet]
		double AccountBalance();

		[OperationContract]
		[WebGet]
		bool OrderCloseAll();

		[OperationContract]
		[WebGet]
		FXModes.MqlTick SymbolInfoTick(string symbol);

		[OperationContract]
		[WebGet]
		bool OrderClose(int ticket, int slippage);

		[OperationContract]
		[WebGet]
		double MarketInfoSPREAD(string symbol);

		[OperationContract]
		[WebGet]
		List<MtOrder> OrdersOpened();

		[OperationContract]
		[WebGet]
		DateTime ServerTimeCurrent();

		[OperationContract]
		[WebGet]
		long ChartOpen(string symbol, string timeframe);

		[OperationContract]
		[WebGet]
		bool OrderModify(int ticket, double price, double stoploss, double takeprofit, string expirationDate);

		[OperationContract]
		[WebGet]
		string LastErrorDescription();

		[OperationContract]
		[WebGet]
		int OrderSendSell(string symbol, double volume, double price, int slippage, double stoploss, double takeprofit, string comment);

		[OperationContract]
		[WebGet]
		int OrderSendBuy(string symbol, double volume, double price, int slippage, double stoploss, double takeprofit, string comment);

		[OperationContract]
		[WebGet]
		double SymbolInfoDouble(string symbol);

		[OperationContract]
		[WebGet]
		List<FXModes.MqlRates> RatesByPositions(string symbol, string timeframe, int startPosition, int count);

		[OperationContract]
		[WebGet]
		List<FXModes.MqlRates> RatesByDates(string symbol, string timeframe, string startDateString, string endDateString);

		[OperationContract]
		[WebGet]
		double iMA(string symbol, string timeframe, int ma_period, int ma_shift, int ma_method, int applied_price, int shift);
	}
}