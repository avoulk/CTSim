using System;
using System.Collections.Generic;
using System.Drawing;
using CGTF.Sim.Messaging;
using CGTF.Sim.Messaging.Types;
using SimLib.Messages;
using SimLib.Messages.Types;
using SimLib.Nodes;

namespace CGTF
{
	public class WSNode : Node
	{
		public static int UNKNOWN_PREVIOUS = -100;
		public static int UNKNOWN_RID = -101;

		private List<int> KnownClusters;
		public int CID { get; set; }
		public double Value { get; set; }
		private double lambda = 0.75;
		public double TransmissionProbability { get; set; }
		public int SID { get; set; }
		public double SIDMeasurement { get; set; }
		private int HCSID { get; set; }
		public int Previous { get; set; }
		private bool Informed { get; set; }
		private double Accuracy { get; set; }
		private List<double> NeighborsDegree { get; set; }
		private List<int> NeighboringClusters { get; set; }
		public int RID { get; set; }
		private int HCRID { get; set; }
		private double QoS;
		public bool ClusterStarter { get; set; }
		private int coStarters;

		public WSNode(int id, Point Coordinates)
			: base(id, Coordinates)
		{
			KnownClusters = new List<int>();
			KnownClusters.Add(Info.ID);
			CID = this.Info.ID;
			Value = 0;
			Accuracy = 1;
			ClusterStarter = true;
			NeighborsDegree = new List<double>();
			NeighboringClusters = new List<int>();
			SID = Info.ID;
			SIDMeasurement = Data;
			HCSID = Int32.MaxValue;
			RID = UNKNOWN_RID;
			HCRID = Int32.MaxValue;
			Previous = UNKNOWN_PREVIOUS;
			Informed = false;
			QoS = SimLib.Properties.Simulation.Default.QoS / 100.0;
		}

		/// <summary>
		/// Handles specific message types
		/// </summary>
		/// <param name="message"></param>
		protected override void handleOtherMessage(IMessage message)
		{
			switch (message.Envelop.Type)
			{
				case CustomMessageType.MSG_CLUSTER_QUERY:
					handleMSGClusterQuery((MSGClusterQuery)message);
					break;
				case CustomMessageType.MSG_INFORM_NODES:
					handleMSGIformNodes((MSGInformNodes)message);
					break;
				case CustomMessageType.MSG_NEIGHBORING_CLUSER_QUERY:
					handleMSGNeighboringClusterQuery((MSGNeighboringClusterQuery)message);
					break;
				case CustomMessageType.MSG_REPORT_CLUSTER:
					handleMSGReportCluster((MSGReportCluster)message);
					break;
				case CustomMessageType.MSG_REPRESENTATIVE_EXPAND:
					handleMSGRepresentativeExpand((MSGRepresentativeExpand)message);
					break;
				case CustomMessageType.MSG_SEND_NODE_DEGREE:
					handleMSGSendNodeDegree((MSGSendNodeDegree)message);
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// Handles a MSGClusterQuery message
		/// </summary>
		/// <param name="message"></param>
		private void handleMSGClusterQuery(MSGClusterQuery message)
		{
			if (message.CID == CID || message.RID != RID || KnownClusters.Contains(message.CID))
				return;
			//Console.WriteLine( QoS + "\t" + msg.GetType().FullName + "\t" + DateTime.Now);
			double _ProposedValue = calculateValue(message);
			if (_ProposedValue >= Value)
			{
				Value = _ProposedValue;
				CID = message.CID;
				KnownClusters.Add(CID);
				SIDMeasurement = message.Data;
				HCSID = message.HC;
				SID = message.CID;
				ClusterStarter = false;
				transmit(new MSGClusterQuery(Info.ID, MessageTargets.ALL_IN_RANGE, SIDMeasurement, RID, CID, HCSID + 1, estimateDEG(message)));
			}
		}

		/// <summary>
		/// Handles a MSGIformNodes message
		/// </summary>
		/// <param name="message">The message to handle</param>
		private void handleMSGIformNodes(MSGInformNodes message)
		{
			if ( (!Informed) && message.RID == RID)
			{
				Informed = true;
				if (!message.CS.ContainsKey(Info.ID))
				{
					Console.WriteLine(Info.ID + "\tShit man!\t[Previous CID was " + this.CID + "]");
					CID = Info.ID;
					TransmissionProbability = 1;
					transmit(new MSGInformNodes(Info.ID, RID, message.CS));
					throw new Exception("Shit");
				}
				CID = message.CS[Info.ID];
				int newClusterSize = 0;
				foreach (var _mCID in message.CS.Values)
				{
					if (_mCID == CID)
					{
						newClusterSize++;
					}
				}
				TransmissionProbability = 1 / newClusterSize;
				transmit(new MSGInformNodes(Info.ID, RID, message.CS));
			}
		}

		/// <summary>
		/// Handles a MSGNeighboringClusterQuery message
		/// </summary>
		/// <param name="message">The message to handle</param>
		private void handleMSGNeighboringClusterQuery(MSGNeighboringClusterQuery message)
		{
			//Console.WriteLine(QoS + "\t" + msg.GetType().FullName + "\t" + DateTime.Now);
			if (!message.Reply)
			{
				if (CID == message.CID)
				{
					//Do nothing, we are in the same cluster
				}
				else
				{
					if (!NeighboringClusters.Contains(message.CID))
					{
						//Add to the neighboring clusters
						NeighboringClusters.Add(message.CID);
					}
					//Send a reply :-)
					transmit(new MSGNeighboringClusterQuery(Info.ID, message.Target, message.CID, true));
				}
			}
			else //it is a reply message
			{
				if (!NeighboringClusters.Contains(message.CID))
				{
					//Add to the known neighboring clusters
					NeighboringClusters.Add(message.CID);
				}
			}
		}

		/// <summary>
		/// Handles a MSGReportCluster message
		/// </summary>
		/// <param name="message">The message to handle</param>
		private void handleMSGReportCluster(MSGReportCluster message)
		{
			if (message.RID == RID)
			{
				if (Previous != UNKNOWN_PREVIOUS)
				{
					transmit(new MSGReportCluster(Info.ID, Previous, message.OriginalNode, message.RID));
				}
			}
		}

		/// <summary>
		/// Handles a MSGRepresentativeExpand message
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="message">The message to expand</param>
		private void handleMSGRepresentativeExpand(MSGRepresentativeExpand message)
		{
			//Console.WriteLine(QoS + "\t" + msg.GetType().FullName + "\t" + DateTime.Now);
			if (message.HC < HCRID)	//This representative is closer
			{
				HCRID = message.HC;
				RID = message.RID;
				Previous = message.Source;
				transmit(new MSGRepresentativeExpand(Info.ID, RID, HCRID + 1));
			}
		}

		/// <summary>
		/// Handles a MSGSendNodeDegree message
		/// </summary>
		/// <param name="message">The message to handle</param>
		private void handleMSGSendNodeDegree(MSGSendNodeDegree message)
		{
			NeighborsDegree.Add(message.Data);
			if (!message.Reply)
			{
				transmit(new MSGSendNodeDegree(Info.ID, message.Source, Neighbors.Count, true));
			}
			if (NeighborsDegree.Count == Neighbors.Count)
			{
				foreach (var nDeg in NeighborsDegree)
				{
					if (Neighbors.Count > nDeg)
					{
						continue;
					}
					else if (Neighbors.Count == nDeg)
					{
						coStarters++;
					}
					else
					{
						ClusterStarter = false;
						break;
					}
				}
			}
		}

		/// <summary>
		/// Estimates the average degree in a cluster
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		private double estimateDEG(MSGClusterQuery message)
		{
			return lambda * message.DEG + (1 - lambda) * Neighbors.Count;
		}

		/// <summary>
		/// Calculates the possible Value for the node if it joins the cluster
		/// </summary>
		/// <param name="message">The message containing the negotiation stuff</param>
		/// <returns>The possible new node value</returns>
		private double calculateValue(MSGClusterQuery message)
		{
			double _accuracy = getDataChange(message.Data);
			return (getClusterSizeEstimation(message) - 1) * _accuracy;
		}

		/// <summary>
		/// Gets an estimation over the size of a cluster
		/// </summary>
		/// <param name="message">The message containing the negotiation data stuff</param>
		/// <returns>The estimated cluster size</returns>
		private double getClusterSizeEstimation(MSGClusterQuery message)
		{
			//Overlapping paper, Eq. (5)
			return estimateDEG(message) * Math.Pow(message.HC, 2);
		}

		/// <summary>
		/// Checks the % data change occuring when joining a cluster
		/// </summary>
		/// <param name="nData">The neighbor data</param>
		/// <returns>The % data change</returns>
		private double getDataChange(double nData)
		{
			if (1 - Math.Abs((Data - nData) / nData) >= QoS)
			{
				return 1 - Math.Abs((Data - nData) / nData);
			}
			else
			{
				return double.MinValue;
			}
		}

		/// <summary>
		/// Fix uninitialized data
		/// </summary>
		public void beginOwnCluster()
		{
			SIDMeasurement = Data;
		}
	}
}
