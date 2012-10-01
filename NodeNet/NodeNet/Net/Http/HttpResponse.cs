using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NodeNet.Net.Http
{
	public class HttpResponse
	{
		private Socket Socket;

		public HttpResponse(Socket Socket)
		{
			this.Socket = Socket;
		}

		public void WriteHead(int ResponseCode, IList<KeyValuePair<string, string>> Headers)
		{
			Socket.SendAsync("HTTP/1.1 " + ResponseCode + " OK\r\n");
			foreach (var Header in Headers)
			{
				Socket.SendAsync(Header.Key + ":" + Header.Value + "\r\n");
			}
			Socket.SendAsync("\r\n");
		}

		private void WriteRaw(string Text)
		{
			var SendData = Convert.ToString((int)Socket.Encoding.GetByteCount(Text), 16).ToUpper() + "\r\n" + Text;
			//Console.WriteLine(SendData);
			Socket.SendAsync(SendData);
		}

		public void Write(string Text)
		{
			if ((Text != null) && (Text.Length > 0))
			{
				WriteRaw(Text);
			}
		}

		public void WriteEnd()
		{
			WriteRaw("");
		}

		public void End(string Text = "")
		{
			Write(Text);
			WriteEnd();
			Socket.DisconnectAsync();
		}
	}
}
