using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NodeNet.Net.Http
{
	public class HttpServer
	{
		SocketServer SocketServer;

		private HttpServer(SocketServer SocketServer)
		{
			this.SocketServer = SocketServer;
		}

		static public HttpServer Create(Action<HttpRequest, HttpResponse> Handler)
		{
			var HttpServer = new HttpServer(SocketServer.Create((Socket) =>
			{
				Handler(new HttpRequest(), new HttpResponse(Socket));
			}));
			return HttpServer;
		}

		public void Listen(string IP, int Port)
		{
			SocketServer.Listen(IP, Port);
		}
	}
}
