using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using NodeNet.Net;
using NodeNet.Net.Http;

namespace NodeNet
{
    public class Sample
    {
		static public void Main(string[] args)
		{
			Core.Loop(() =>
			{
				var Server = HttpServer.Create((Request, Response) =>
				{
					//Console.WriteLine("aa");
					Response.WriteHead(200, new[] {
						new KeyValuePair<string, string>("Connection", "close"),
						new KeyValuePair<string, string>("Content-Type", "text/plain"),
						new KeyValuePair<string, string>("Date", "Mon, 01 Oct 2012 06:36:31 GMT"),
						new KeyValuePair<string, string>("Transfer-Encoding", "chunked"),
					});
					Response.Write("Hello world!");
					Response.End();
				});
				Server.Listen("0.0.0.0", 80);
			});
		}
    }
}
