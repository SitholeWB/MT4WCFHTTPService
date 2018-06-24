using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MT4WCFHTTPService.FXModes
{
	public class MqlTick : MtApi.MqlTick
	{
		public long MtTime { get; set; }
		public double bid { get; set; }
		public double ask { get; set; }
		public double last { get; set; }
		public ulong volume { get; set; }
		public DateTime time { get; set; }
	}
}