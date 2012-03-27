using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimLib.Messages;

namespace SimLib.Nodes
{
    public class Antenna
    {
        public double Range { get; set; }

        private List<IMessage> incoming;
        private List<IMessage> outgoing;

        public Antenna()
        {
            Range = Properties.Simulation.Default.Range;
            incoming = new List<IMessage>();
            outgoing = new List<IMessage>();
        }

        /// <summary>
        /// Adds a message to the incoming messages queue
        /// </summary>
        /// <param name="message">The received message</param>
        public void Receive(IMessage message)
        {
            incoming.Add(message);
        }

        /// <summary>
        /// Adds a message to the outgoing messages queue
        /// </summary>
        /// <param name="message"></param>
        public void Transmit(IMessage message)
        {
            outgoing.Add(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<IMessage> StartTransmission()
        {
            return outgoing;
        }

        /// <summary>
        /// Ends a transmission phase
        /// [clears all messages from the transmission queue]
        /// </summary>
        public void EndTransmission()
        {
            outgoing.Clear();
        }

        /// <summary>
        /// Gets the list of incoming messages
        /// </summary>
        /// <returns></returns>
        public List<IMessage> GetIncoming()
        {
            return incoming;
        }

        /// <summary>
        /// Clears the list of incoming messages
        /// </summary>
        public void ClearIncoming()
        {
            incoming.Clear();
        }
    }
}
