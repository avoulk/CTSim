using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SimLib.Fields;
using SimLib.Messages;
using SimLib.Messages.Types;
using SimLib.Nodes;

namespace SimLib.Abstractions.Networking
{
	class Messenger<T>
	{
		Field field;

		public Messenger(Field field)
		{
			this.field = field;
		}

		public void deliver(T item)
		{
			if(typeof(T).FullName != "SimLib.Messages.IMessage")
			{
				return;
			}
			IMessage message = (IMessage)item;
			if (message.Envelop.Target == MessageTargets.ALL_IN_RANGE)
			{
				List<int> targets = field.Neighbors[message.Envelop.Source];
				foreach (var mTarget in targets)
				{
					INode source = field.Get(message.Envelop.Source);
					INode target = field.Get(mTarget);
					if (SimMath.Distance.WithinRange(source, target))
					{
						target.receive(message);
					}
				}
			}
			else
			{
				INode source = field.Get(message.Envelop.Source);
				INode target = field.Get(message.Envelop.Target);
				if (SimMath.Distance.WithinRange(source, target))
				{
					target.receive(message);
				}
			}
		}
	}
}
