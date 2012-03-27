using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimLib.Fields;
using SimLib.Messages;
using SimLib.Messages.Types;
using SimLib.Nodes;

namespace SimLib.Abstractions.Networking
{
	/// <summary>
	/// Implements the communications part of the simulation
	/// </summary>
    class Communication
    {
        public BlockingCollection<IMessage> Bus { get; set; }
        private List<Task> tasks;
        private Field field;

        public Communication(Field field)
        {
            this.field = field;
            initialize();
        }

		/// <summary>
		/// Initializes the Communication bus
		/// </summary>
        private void initialize()
        {
            Bus = new BlockingCollection<IMessage>();
            tasks = new List<Task>();
            Task.Factory.StartNew(() => serve());
        }

		/// <summary>
		/// Simulates the transmission environment.
		/// </summary>
        private void serve()
        {
            foreach (var item in Bus.GetConsumingEnumerable())
            {
                IMessage iMessage = item;
				deliver(iMessage);
				//Task.Factory.StartNew(new Action(() => deliver(iMessage)), CancellationToken.None, TaskCreationOptions.None, null);
            }
        }

		/// <summary>
		/// Handles A item sending. It is mainly here to cover the selective item transmission versus the multi/broad-casting.
		/// </summary>
		/// <param name="item">The item to be sent</param>
        private void deliver(IMessage message)
        {
            if (message.Envelop.Target == MessageTargets.ALL_IN_RANGE)
            {
				List<int> targets = field.Neighbors[message.Envelop.Source];
                foreach (var mTarget in targets)
                {
                    send(message, mTarget);
                }
            }
            else
            {
                send(message, message.Envelop.Target);
            }
        }

		/// <summary>
		/// Sends A item
		/// </summary>
		/// <param name="item">The item</param>
		/// <param name="mTarget">The target node ID</param>
        private void send(IMessage message, int mTarget)
        {
            INode source  = field.Get(message.Envelop.Source);
            INode target = field.Get(mTarget);
            if ( SimMath.Distance.WithinRange(source, target))
            {
                target.receive(message);
            }
        }
    }
}
