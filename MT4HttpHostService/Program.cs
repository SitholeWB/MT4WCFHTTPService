using MT4WCFHTTPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace MT4HTTPService
{
	public class Program
	{
		private static void Main(string[] args)
		{
			WebServiceHost host = new WebServiceHost(typeof(Service), new Uri("http://localhost:9000/"));
			try
			{
				ServiceEndpoint ep = host.AddServiceEndpoint(typeof(IService), new WebHttpBinding(), "");
				host.Open();
				using (ChannelFactory<IService> cf = new ChannelFactory<IService>(new WebHttpBinding(), "http://localhost:9000"))
				{
					cf.Endpoint.Behaviors.Add(new WebHttpBehavior());

					IService channel = cf.CreateChannel();

					Console.WriteLine("Calling AccountBalance via HTTP GET: ");
					var results = channel.AccountBalance();
					Console.WriteLine("   Output: {0}", results);

					Console.WriteLine("");
					Console.WriteLine("This can also be accomplished by navigating to");
					Console.WriteLine("http://localhost:9000/AccountBalance");
					Console.WriteLine("Calls with parameters can be done like...");
					Console.WriteLine("http://localhost:9000/SymbolInfoTick?symbol=EURUSD");
					Console.WriteLine("in a web browser while this sample is running.");

					Console.WriteLine("");
				}

				Console.WriteLine("Press <ENTER> to terminate");
				Console.ReadLine();

				host.Close();
			}
			catch (CommunicationException cex)
			{
				Console.WriteLine("An exception occurred: {0}", cex.Message);
				host.Abort();
				Console.ReadLine();
			}
		}
	}
}