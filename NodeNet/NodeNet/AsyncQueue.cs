using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace NodeNet
{
	public class AsyncQueue
	{
		Queue<Action> Queue = new Queue<Action>();
		bool Running = false;

		public void Add(Action Action)
		{
			//Console.WriteLine("AsyncQueue.Add: " + Thread.CurrentThread.ManagedThreadId);

			//lock (Queue)
			{
				Queue.Enqueue(Action);
			}
			if (!Running) Next();
		}

		public void Next()
		{
			//Console.WriteLine("AsyncQueue.Next: " + Thread.CurrentThread.ManagedThreadId);

			//lock (Queue)
			{
				Running = (Queue.Count > 0);
			}

			if (Running)
			{
				Action Action;
				//lock (Queue)
				{
					Action = Queue.Dequeue();
				}
				if (Action != null) Action();
			}
			else
			{
				Running = false;
			}
		}
	}
}
