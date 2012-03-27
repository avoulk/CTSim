using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using SimLib.Messages;
using System.Collections.Concurrent;
using SimLib.Abstractions.Networking;

namespace SimLib.Nodes
{
	public interface INode
	{
		NodeInfo Info { get; set; }
		PowerInfo Power { get; set; }
		Antenna Antenna { get; set; }

		List<int> Neighbors { get; set; }
		void addNeighbor(int ID);
		void addNeighbor(List<int> neighbors);

		void receive(IMessage message);
		void processReceived();

		List<IMessage> transmit();
		void transmit(IMessage message);
		void endTransmissions();

		MessageCountHolder MessageCount { get; set; }

		double Data { get; set; }
		void Sense();
	}
}
