using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace NodeNet
{
	public class Core
	{
		static public void Loop(Action Action)
		{
			Action();

			while (true)
			{
				ActionsAdded.WaitOne();
				while (true)
				{
					Action CAction;
					lock (Actions)
					{
						if (Actions.Count == 0) break;
						CAction = Actions.Dequeue();
					}
					CAction();
				}
			}
		}

		static AutoResetEvent ActionsAdded = new AutoResetEvent(true);
		static Queue<Action> Actions = new Queue<Action>();

		static public void EnqueueTask(Action Action)
		{
			lock (Actions)
			{
				Actions.Enqueue(Action);
			}
			ActionsAdded.Set();
		}
	}
}
