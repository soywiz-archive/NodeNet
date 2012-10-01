using System;
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
					Response.Socket.SendAsync("HTTP/1.1 / 200 OK\r\nConnection:close\r\n\r\n");
					Response.Socket.DisconnectAsync();
				});
				Server.Listen("0.0.0.0", 80);
			});
		}
    }
}
