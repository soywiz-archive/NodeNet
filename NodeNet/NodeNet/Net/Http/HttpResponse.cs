using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NodeNet.Net.Http
{
	public class HttpResponse
	{
		public Socket Socket;

		public HttpResponse(Socket Socket)
		{
			this.Socket = Socket;
		}
	}
}
