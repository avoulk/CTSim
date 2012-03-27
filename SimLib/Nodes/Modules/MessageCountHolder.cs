
namespace SimLib.Nodes
{
    public class MessageCountHolder
    {
        /// <summary>
        /// Holds the number of messages handled by the node
        /// </summary>
        public MessageCountHolder()
        {
            Sent = 0;
            Received = 0;
        }

        /// <summary>
        /// Gets/Sets the number of sent messages
        /// </summary>
        public int Sent { get; set; }

        /// <summary>
        /// Gets/Sets the number of received messages
        /// </summary>
        public int Received { get; set; }

        /// <summary>
        /// Receive A item
        /// </summary>
        public void receive()
        {
            Received++;
        }

        /// <summary>
        /// Send A item
        /// </summary>
        public void send()
        {
            Sent++;
        }
    }
}
