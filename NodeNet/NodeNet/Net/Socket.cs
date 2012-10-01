using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NativeSocket = System.Net.Sockets.Socket;

namespace NodeNet.Net
{
	public class Socket
	{
		private NativeSocket NativeSocket;
		private List<ArraySegment<byte>> Buffers;

		internal Socket(NativeSocket NativeSocket)
		{
			//NativeSocket.NoDelay = true;
			//NativeSocket.Blocking = false;

			Buffers = new List<ArraySegment<byte>>()
			{
				new ArraySegment<byte>(new byte[4 * 1024])
			};

			this.NativeSocket = NativeSocket;

			//if (OnConnect != null) OnConnect();

			// OnData
			ReceiveAsync((Data) =>
			{
				if (OnData != null) OnData(Data);

				DisconnectAsync(() =>
				{
				});
				//Console.WriteLine("lol : " + Encoding.ASCII.GetString(Data));
			});
			//OnEnd.BeginInvoke(new AsyncCallback(new IAsyncResult()), null);
			//Socket.BeginReceive(
		}

		public Encoding Encoding = new UTF8Encoding(false, false);
		public AsyncQueue AsyncQueue = new AsyncQueue();

		public void SendAsync(string Data, Action Callback = null)
		{
			SendAsync(Encoding.GetBytes(Data), Callback);
		}

		public void SendAsync(byte[] Data, Action Callback = null)
		{
			var DataCopy = new byte[Data.Length];
			Array.Copy(Data, DataCopy, Data.Length);

			var BuffersCopy = new List<ArraySegment<byte>>() { new ArraySegment<byte>(DataCopy) };

			AsyncQueue.Add(() =>
			{
				try
				{
					NativeSocket.BeginSend(BuffersCopy, SocketFlags.None, (IAsyncResult) =>
					{
						SocketError SocketError;
						NativeSocket.EndSend(IAsyncResult, out SocketError);
						Core.EnqueueTask(() =>
						{
							if (Callback != null) Callback();
							AsyncQueue.Next();
						});
					}, null);
				}
				catch (SocketException)
				{
					if (OnEnd != null) OnEnd();
				}
			});
		}

		public void DisconnectAsync(Action Action = null)
		{
			AsyncQueue.Add(() =>
			{
				try
				{
					NativeSocket.BeginDisconnect(true, (IAsyncResult) =>
					{
						NativeSocket.EndDisconnect(IAsyncResult);
						Core.EnqueueTask(() =>
						{
							if (Action != null) Action();
							AsyncQueue.Next();
						});
					}, null);
				}
				catch (SocketException)
				{
					if (OnEnd != null) OnEnd();
				}
			});
		}

		private void ReceiveAsync(Action<byte[]> Action)
		{
			try
			{
				//Console.WriteLine("[1]");
				NativeSocket.BeginReceive(Buffers, SocketFlags.None, (IAsyncResult) =>
				{
					//Console.WriteLine("[2]");
					SocketError SocketError;
					var Readed = NativeSocket.EndReceive(IAsyncResult, out SocketError);

					//Console.WriteLine("[3]");

					//Console.WriteLine(Readed);
					var ArrayBuffer = Buffers[0];
					var Buffer = new byte[Readed];
					Array.Copy(ArrayBuffer.Array, ArrayBuffer.Offset, Buffer, 0, Buffer.Length);
					//Buffers[0].
					Core.EnqueueTask(() =>
					{
						Action(Buffer);
					});
					ReceiveAsync(Action);
				}, null);
			}
			catch (SocketException)
			{
				//Console.Error.WriteLine(SocketException);
				if (OnEnd != null) OnEnd();
			}
		}

		public event Action OnConnect;
		public event Action<byte[]> OnData;
		public event Action OnEnd;
		public event Action OnTimeout;
		public event Action OnDrain;
		public event Action OnError;
		public event Action OnClose;
	}
}
