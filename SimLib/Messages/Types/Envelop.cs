using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimLib.Messages
{
	public class Envelop : EventArgs
	{
		public int Source { get; set; }
		public int Target { get; set; }
		public int Type { get; set; }
		public double Data { get; set; }

		public Envelop(int Source, int Target, int Type, double Data)
		{
			this.Source = Source;
			this.Target = Target;
			this.Type = Type;
			this.Data = Data;
		}

		public Message getMessage()
		{
			return new Message(Source, Target, Type, Data);
		}
	}
}
