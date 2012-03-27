using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using SimLib.Fields;
using SimLib.Messages;
using SimLib.Abstractions.Networking;
using SimLib.Messages.Types;

namespace SimLib.Nodes
{
	public class Node : INode
	{
		/// <summary>
		/// Create A new generic node. This should be subclassed
		/// </summary>
		/// <param name="ID">The ID of the node</param>
		/// <param name="Coordinates">The Coordinates of the node</param>
		public Node(String id, Point Coordinates)
		{
			Info = new NodeInfo(int.Parse(id), Coordinates);
			Init();
		}

		/// <summary>
		/// Create A new generic node. This should be subclassed
		/// </summary>
		/// <param name="ID">The ID of the node</param>
		/// <param name="Coordinates">The Coordinates of the node</param>
		public Node(int id, Point Coordinates)
		{
			Info = new NodeInfo(id, Coordinates);
			Init();
		}

		public void Init()
		{
			MessageCount = new MessageCountHolder();
			Antenna = new Antenna();
			Power = new PowerInfo();
			Neighbors = new List<int>();
		}

		/// <summary>
		/// Gets/Sets node information
		/// </summary>
		public NodeInfo Info { get; set; }

		/// <summary>
		/// Gets/Sets node power information
		/// </summary>
		public PowerInfo Power { get; set; }

		/// <summary>
		/// Gets/Sets the antenna info of the node
		/// </summary>
		public Antenna Antenna { get; set; }

		/// <summary>
		/// Gets/Sets A message counter
		/// </summary>
		public MessageCountHolder MessageCount { get; set; }

		/// <summary>
		/// Gets/Sets the data sensed by the node
		/// </summary>
		public double Data { get; set; }

		/// <summary>
		/// Gets/Sets the node neighbors
		/// </summary>
		public List<int> Neighbors { get; set; }

		/// <summary>
		/// Acknowledge A new neighbor
		/// </summary>
		/// <param name="ID"></param>
		public void addNeighbor(int ID)
		{
			Neighbors.Add(ID);
		}

		/// <summary>
		/// Adds A newNeighbors of new neighbors to the acknowledged ones
		/// </summary>
		/// <param name="neighbors">The new neighbors</param>
		public void addNeighbor(List<int> neighbors)
		{
			Neighbors.AddRange(neighbors);
		}

		/// <summary>
		/// Prepares a transmission message.
		/// </summary>
		/// <param name="message">The message to transmit</param>
		public void transmit(IMessage message)
		{
			Antenna.Transmit(message);
			MessageCount.send();
		}

		/// <summary>
		/// Actually starts the transmission of the messages lying in the transmission queue
		/// </summary>
		/// <returns></returns>
		public List<IMessage> transmit()
		{
			return Antenna.StartTransmission();
		}

		/// <summary>
		/// Clears the transmission queue of the node
		/// </summary>
		public void endTransmissions()
		{
			Antenna.EndTransmission();
		}

		/// <summary>
		/// Receives A message
		/// </summary>
		/// <param name="message">The message to receive</param>
		public void receive(IMessage message)
		{
			Antenna.Receive(message);
			MessageCount.receive();
		}

		/// <summary>
		/// Processes the received messages
		/// </summary>
		public void processReceived()
		{
			foreach (IMessage message in Antenna.GetIncoming())
			{
				handleMessage(message);
			}
			Antenna.ClearIncoming();
		}

		/// <summary>
		/// Handles an incoming message
		/// </summary>
		/// <param name="message"></param>
		protected void handleMessage(IMessage message)
		{
			switch (message.Envelop.Type)
			{
				case Messages.MessageType.NEIGHBOR_DISCOVERY:
					handleNeighborDiscoveryMessage((MSGNeighborDiscovery)message);
					break;
				case Messages.MessageType.DATA_EXCHANGE:
					handleDataExchangeMessage((MSGDataExchange)message);
					break;
				default:
					handleOtherMessage(message);
					break;
			}
		}

		/// <summary>
		/// Handles a new message type
		/// SHOULD BE OVERRIDEN
		/// </summary>
		/// <param name="message"></param>
		protected virtual void handleOtherMessage(IMessage message)
		{
			return;
		}

		/// <summary>
		/// Handles A NEIGHBOR_DISCOVERY message
		/// </summary>
		/// <param name="message">The message received</param>
		protected void handleNeighborDiscoveryMessage(MSGNeighborDiscovery message)
		{
			if (!Neighbors.Contains(message.Envelop.Source))
			{
				Neighbors.Add(message.Envelop.Source);
				//Console.WriteLine(this.Info.ID + "\tAdded neighbor " + message.Envelop.Source + "\tReceived: " + MessageCount.Received);
			}
		}

		/// <summary>
		/// Handles A DATA_EXCHANGE message
		/// </summary>
		/// <param name="message">The message received</param>
		protected void handleDataExchangeMessage(IMessage message)
		{
			Console.WriteLine("Received data = " + message.Envelop.Data + " from " + message.Envelop.Source);
		}

		/// <summary>
		/// Senses the field to get a value
		/// </summary>
		public void Sense()
		{
			Data = Field.Data.Get(this.Info.Coordinates);
		}
	}
}
