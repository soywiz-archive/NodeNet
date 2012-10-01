using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace NodeNet
{
	public class Core
	{
		static bool InsideLoop = false;

		static public void Loop(Action Action)
		{
			try
			{
				InsideLoop = true;
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
			finally
			{
				InsideLoop = false;
			}
		}

		static AutoResetEvent ActionsAdded = new AutoResetEvent(true);
		static Queue<Action> Actions = new Queue<Action>();

		static public void EnqueueTask(Action Action)
		{
			if (!InsideLoop) Console.Error.WriteLine("Must call Core.Loop(...)");

			lock (Actions)
			{
				Actions.Enqueue(Action);
			}
			ActionsAdded.Set();
		}
	}
}
