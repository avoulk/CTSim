using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimLib.Nodes;
using System.Drawing;
using SimLib.Messages;
using CGTF.Sim.Messaging;
using MathNet.Numerics;
using System.Diagnostics;
using CGTF.Sim.Messaging.Types;

namespace CGTF.Sim.Clustering
{
	public class Representative : Node
	{
		public List<Cluster> Clusters { get; set; }
		List<int> ClustersI { get; set; }
		private double QoS { get; set; }
		//TODO: Express N, P

		public Representative(int ID, Point Coordinates)
			: base(ID, Coordinates)
		{
			Clusters = new List<Cluster>();
			ClustersI = new List<int>();
			QoS = SimLib.Properties.Simulation.Default.QoS / 100.0;
			//TODO: N, P
		}

		protected override void handleOtherMessage(IMessage message)
		{
			switch (message.Envelop.Type)
			{
				case CustomMessageType.MSG_REPORT_CLUSTER:
					handleMSGReportCluster((MSGReportCluster)message);
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// Handles a MSGReport message that the nodes sent to inform the representative
		/// </summary>
		/// <param name="message">The message</param>
		private void handleMSGReportCluster(MSGReportCluster message)
		{
			bool clusterFound = false;
			if (message.RID == Info.ID)
			{
				
				foreach (var cluster in Clusters)
				{
					if (cluster.ID == message.OriginalNode.CID)
					{
						cluster.Add(message.OriginalNode);
						clusterFound = true;
						//Console.Write("Added " + message.OriginalNode.Info.ID + " to existing CID " + cluster.ID + ". Members: ");
						//foreach (var item in cluster.Nodes)
						//{
						//    Console.Write(item.Info.ID + " ");
						//}
						//Console.WriteLine();
						break;
					}
				}
				if (!clusterFound)
				{
					Cluster cluster = new Cluster(message.OriginalNode.CID);
					cluster.Add(message.OriginalNode);
					Clusters.Add(cluster);
					ClustersI.Add(cluster.ID);
				}
			}
			//Console.WriteLine("Got message from node " + message.OriginalNode.Info.ID + " with CID " + message.OriginalNode.CID + "\t" + clusterFound);
		}

		/// <summary>
		/// Informs the nodes of its existence
		/// </summary>
		public void Expand()
		{
			transmit(new MSGRepresentativeExpand(Info.ID, Info.ID, 1));
		}

		/// <summary>
		/// Optimizes the coalition formation
		/// </summary>
		public void optimize()
		{
			foreach (var cluster in Clusters) { fixCSID(cluster); }
			//Sort clusters according to their size
			Clusters.Sort(delegate(Cluster A, Cluster B) { return A.Nodes.Count.CompareTo(B.Nodes.Count); });
			Clusters.Reverse();
			bool changedState = true;
			while (changedState)
			{
				changedState = false;
				Cluster reference = null;
				Cluster other = null;
				foreach (var referenceCluster in Clusters)
				{
					reference = referenceCluster;
					foreach (var otherCluster in Clusters)
					{
						other = otherCluster;
						if (referenceCluster.ID == otherCluster.ID)
						{
							continue;
						}
						if (checkValidity(referenceCluster, otherCluster))
						{
							changedState = true;
							break;
						}
					}
					if (changedState)
					{
						break;
					}
				}
				if (changedState)
				{
					foreach (var node in other.Nodes)
					{
						node.SID = reference.StartID;
						node.SIDMeasurement = reference.StartData;
						node.CID = reference.ID;
						reference.Min = Math.Min(reference.Min, node.Data);
						reference.Max = Math.Max(reference.Max, node.Data);
						reference.Add(node);
					}
					other.Nodes.RemoveAll(x => x.CID == reference.ID);
					Clusters.RemoveAll(x => x == other);
					//Console.WriteLine("Removed CID " + other.ID);
					//Console.WriteLine("Merged " + reference.ID + " and " + other.ID + "\tAccuracy: " + String.Format("{0:0.00}", reference.getAccuracy()));
				}
			}
			//printClusters();
		}

		/// <summary>
		/// Checks the validity between two clusters
		/// </summary>
		/// <param name="A">The first cluster</param>
		/// <param name="S">The second cluster</param>
		/// <returns>True if they can merge, false otherwise</returns>
		private bool checkValidity(Cluster A, Cluster S)
		{
			if (A.StartData == S.StartData)
			{
				return true;
			}
			else if (A.StartData > S.StartData)
			{
				if ( 1 - Math.Abs((A.StartData - S.Min) / A.StartData) >= QoS)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			else if (A.StartData < S.StartData)
			{
				if ( 1 - Math.Abs((A.StartData - S.Max) / A.StartData) >= QoS)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Gets a cluster out of its ID
		/// </summary>
		/// <param name="ID">The ID of the cluster to check</param>
		/// <returns>The cluster with the specified ID</returns>
		private Cluster getCluster(int ID)
		{
			foreach (var item in Clusters)
			{
				if (item.ID == ID)
				{
					return item;
				}
			}
			return null;
		}

		/// <summary>
		/// Gets the value of a CS
		/// </summary>
		/// <param name="CS">The coalition structure to check</param>
		/// <returns>The value of the CS</returns>
		private double ValueOfCS()
		{
			double ret = 0;
			foreach (var HyperC in Clusters)
			{
				ret += HyperC.Value;
			}
			return ret;
		}

		/// <summary>
		/// Prints the current cluster formation around the specified representative
		/// </summary>
		public void printClusters()
		{
			Console.WriteLine("\nREP ID: " + this.Info.ID + " Clusters\n" +
							  "===================");
			foreach (var cluster in Clusters)
			{
				Console.Write(cluster.ID + ":\t");
				Console.Write("\t(" + cluster.Nodes.Count + ")\t");
				Console.Write("\t[" + String.Format("{0:0.00}", cluster.getAccuracy()) + " / " + QoS + "]\t");
				Console.Write("\tSID = " + cluster.StartID + " --> \t");
				foreach (var NID in cluster.Nodes)
				{
					Console.Write(NID.Info.ID + " ");
				}
				Console.WriteLine();
			}
		}

		/// <summary>
		/// Fixes the cluster formation to make it more realistic (in case the starter node has gone absent)
		/// </summary>
		/// <param name="cluster">The cluster to check</param>
		internal void fixCSID(Cluster cluster)
		{
			//Fix Singleton
			foreach (var cCluster in Clusters)
			{
				if (cCluster.Nodes.Count == 1)
				{
					cCluster.ID = SimLib.Properties.Simulation.Default.Nodes * 100 + cCluster.Nodes[0].Info.ID;
					cCluster.StartData = cCluster.Nodes[0].Data;
					cCluster.StartID = cCluster.Nodes[0].Info.ID;
					cCluster.Max = cCluster.StartData;
					cCluster.Min = cCluster.StartData;
				}
			}
			bool foundSID = false;
			int SID = cluster.StartID;
			double SIDM = cluster.StartData;
			foreach (var node in cluster.Nodes)
			{
				if (node.Info.ID == cluster.ID)
				{
					foundSID = true;
					break;
				}
			}
			if (!foundSID)
			{
				//Fix SID selection
				double MaxAccuracy = -2;
				int _SID = SID;
				double _SIDMeasurement = SIDM;
				foreach (var reference in cluster.Nodes)
				{
					bool isValid = true;
					int __SID = reference.Info.ID;
					double __SIDMeasurement = reference.Data;
					double _MinAccuracy = double.MaxValue;
					foreach (var node1 in cluster.Nodes)
					{
						double __Accuracy = getDataChange(node1.Data, __SIDMeasurement);
						if (__Accuracy < 0)
						{
							isValid = false;
							break;
						}
						_MinAccuracy = Math.Min(__Accuracy, _MinAccuracy); //It is actually the minimum accuracy :-)
					}
					if (!isValid)
					{
						continue;
					}
					if (_MinAccuracy > MaxAccuracy) //Min-Max criterion
					{
						_SID = __SID;
						_SIDMeasurement = __SIDMeasurement;
						MaxAccuracy = _MinAccuracy;
					}
				}
				cluster.StartID = _SID;
				cluster.StartData = _SIDMeasurement;
				cluster.ID = (-1 * cluster.StartID);
				//Fix Min, Max values
				double _Min = Double.MaxValue;
				double _Max = Double.MinValue;
				foreach (var node in cluster.Nodes)
				{
					node.CID = cluster.ID;
					node.SID = cluster.StartID;
					node.SIDMeasurement = cluster.StartData;
					_Min = Math.Min(node.Data, _Min);
					_Max = Math.Max(node.Data, _Max);
				}
				cluster.Max = _Max;
				cluster.Min = _Min;
			}
		}

		/// <summary>
		/// Gets the data change when a node with data nData joins a cluster with reference node with referenceData
		/// </summary>
		/// <param name="otherData">The data of the starter node</param>
		/// <param name="referenceData">The data of the cluster node</param>
		/// <returns>The data change in a scale [0,1]</returns>
		private double getDataChange(double otherData, double referenceData)
		{
			if (1 - Math.Abs((otherData - referenceData) / referenceData) >= QoS)
			{
				return Math.Abs(1 - Math.Abs((otherData - referenceData) / referenceData));
			}
			else
			{
				return -1;
			}
		}

		/// <summary>
		/// Gets the resulting accuracy for the CS
		/// </summary>
		/// <returns>The resulting accuracy</returns>
		public double getACC()
		{
			double ret = 0;
			foreach (var Cluster in Clusters)
			{
				ret += (Cluster.Nodes.Count * Cluster.getAccuracy());
			}
			return 100.0 * ret / getNodeCount();
		}

		/// <summary>
		/// Gets teh number of nodes reporting to this representative
		/// </summary>
		/// <returns></returns>
		public double getNodeCount()
		{
			int ret = 0;
			foreach (var cluster in Clusters)
			{
				ret += cluster.Nodes.Count;
			}
			return 1.0 * ret;
		}

		/// <summary>
		/// Notifies the nodes for the current coalition formation
		/// </summary>
		public void notify()
		{
			// CS contains information over the cluster formation,
			// the keys showing the node ID and the values the respective
			// cluster IDs.
			Dictionary<int, int> CS = new Dictionary<int, int>();
			foreach (var cluster in Clusters)
			{
				foreach (var node in cluster.Nodes)
				{
					CS.Add(node.Info.ID, cluster.ID);
				}
			}
			transmit(new MSGInformNodes(Info.ID, Info.ID, CS));
		}
	}
}
