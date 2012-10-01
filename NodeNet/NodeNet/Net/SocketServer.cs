using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NodeNet.Net
{
	public class SocketServer
	{
		private TcpListener TcpListener;

		private SocketServer()
		{
		}

		public void Listen(string IP, int Port)
		{
			this.TcpListener = new TcpListener(IPAddress.Parse(IP), Port);
			this.TcpListener.Start();
			BeginAccept();
		}

		static public SocketServer Create(Action<Socket> Handler)
		{
			var SocketServer = new SocketServer();
			SocketServer.OnConnect += Handler;
			return SocketServer;
		}

		private void BeginAccept()
		{
			TcpListener.BeginAcceptSocket(Accept, null);
		}

		private void Accept(IAsyncResult IAsyncResult)
		{
			var NativeSocket = TcpListener.EndAcceptSocket(IAsyncResult);
			Core.EnqueueTask(() =>
			{
				if (OnConnect != null) OnConnect(new Socket(NativeSocket));
			});
			BeginAccept();
		}

		private event Action<Socket> OnConnect;
	}
}
