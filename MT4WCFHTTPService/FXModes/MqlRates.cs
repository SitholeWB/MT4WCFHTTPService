using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MT4WCFHTTPService.FXModes
{
	public class MqlRates : MtApi.MqlRates
	{
		public DateTime time { get; set; }
		public long mt_time { get; set; }
		public double open { get; set; }
		public double high { get; set; }
		public double low { get; set; }
		public double close { get; set; }
		public long tick_volume { get; set; }
		public int spread { get; set; }
		public long real_volume { get; set; }
	}
}