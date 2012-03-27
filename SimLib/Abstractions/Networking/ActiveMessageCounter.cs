using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace SimLib.Abstractions.Networking
{
	public class ActiveMessageCounter
	{
		public event EventHandler AllMessagesSent;

		private int ActiveMessageCount;
		public int ActiveMessages
		{
			get
			{
				return ActiveMessageCount;
			}
			set
			{
				Console.WriteLine("Active messages: " + value);
				ActiveMessageCount = value;
			}
		}

		public ActiveMessageCounter()
		{
			ActiveMessageCount = 0;
		}

		public void Add()
		{
			ActiveMessages++;
		}

		public void Remove()
		{
			ActiveMessages--;
		}

		private void SendMessageCount()
		{
			if (this.AllMessagesSent != null && ActiveMessages == 0)
			{
				AllMessagesSent(this, EventArgs.Empty);
			}
		}
	}
}
