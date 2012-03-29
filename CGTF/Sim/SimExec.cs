using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using CGTF.Sim.Clustering;
using CGTF.Sim.Messaging;
using SimLib.Fields;
using SimLib.Messages;
using SimLib.Messages.Types;

namespace CGTF
{
	public class SimExec
	{
		 Field field;
		 List<Representative> Reps;

		/// <summary>
		/// Set up environment and start simulation
		/// </summary>
		/// <param name="args"></param>
		public  void Run()
		{
			field = new Field();
			//Create the Representatives
			Reps = new List<Representative>();
			Reps.Add(new Representative(10 * SimLib.Properties.Simulation.Default.Nodes, new Point(10, 10)));
			Reps.Add(new Representative(10 * SimLib.Properties.Simulation.Default.Nodes + 1, new Point(20, 20)));
			//Add the representatives
			foreach (var Rep in Reps)
			{
				field.Add(Rep);
			}
			//Add the nodes
			for (int ID = 0; ID < SimLib.Properties.Simulation.Default.Nodes; ID++)
			{
				field.Add(new WSNode(ID, field.getEmptyRandomCoordinates()));
			}
			//Start!!!
			field.Init();
			simulate();
		}

		/// <summary>
		/// Starts actual simulation
		/// </summary>
		private  void simulate()
		{
			ExpandRepresentatives();
			SetUpNeighbors();
			ClusterInit();
			//GetNeighboringClusters();
			ReportClusters();
			Optimize();
		}

		/// <summary>
		/// Sets up the neighbors of the field nodes via messaging
		/// </summary>
		private  void SetUpNeighbors()
		{
			//Console.WriteLine("*********** NEIGHBORS ***********");
			//field.com.Restart();
			foreach (var node in field.Get())
			{
				node.transmit(new MSGNeighborDiscovery(node.Info.ID));
			}
			Field.Communications.startOfAnEra("Neighboring Setup");
		}

		/// <summary>
		/// Informs the nodes over the existence of the representatives
		/// </summary>
		private  void ExpandRepresentatives()
		{
			//Console.WriteLine("*********** EXPAND ***********");
			//field.com.Restart();
			foreach (var Rep in Reps)
			{
				Rep.Expand();
			}
			Field.Communications.startOfAnEra("Representative Expanding");
		}

		/// <summary>
		/// Initializes cluster formation
		/// </summary>
		private  void ClusterInit()
		{
			//Console.WriteLine("*********** CLUSTER INIT ***********");
			//field.com.Restart();
			foreach (var node in field.Get())
			{
				if (!node.GetType().FullName.Equals("CGTF.WSNode"))
				{
					continue;
				}
				node.transmit(new MSGClusterQuery(node.Info.ID, MessageTargets.ALL_IN_RANGE, node.Data, ((WSNode)node).RID, node.Info.ID, 1, node.Neighbors.Count));
			}
			Field.Communications.startOfAnEra("Cluster initialization");
		}

		/// <summary>
		/// Informs the nodes over their neighboring clusters
		/// </summary>
		private  void GetNeighboringClusters()
		{
			//Console.WriteLine("*********** NEIGHBORING CLUSTERS ***********");
			//field.com.Restart();
			foreach (var node in field.Get())
			{
				if (!node.GetType().FullName.Equals("CGTF.WSNode"))
				{
					continue;
				}
				node.transmit(new MSGNeighboringClusterQuery(node.Info.ID, MessageTargets.ALL_IN_RANGE, ((WSNode)node).CID, false));
			}
			Field.Communications.startOfAnEra("Get Neighboring Clusters");
		}

		/// <summary>
		/// Initiating the cluster reporting to the representatives
		/// </summary>
		private  void ReportClusters()
		{
			//Console.WriteLine("*********** REPORT ***********");
			//field.com.Restart();
			foreach (var node in field.Get())
			{
				if (!node.GetType().FullName.Equals("CGTF.WSNode"))
				{
					continue;
				}
				if (((WSNode)node).Previous != WSNode.UNKNOWN_PREVIOUS)
				{
					node.transmit(new MSGReportCluster(node.Info.ID, ((WSNode)node).Previous, ((WSNode)node), ((WSNode)node).RID));
				}
			}
			Field.Communications.startOfAnEra("Cluster Reporting");
		}

		/// <summary>
		/// Optimizes the coalition formation
		/// </summary>
		private  void Optimize()
		{
			//Optimize CS
			foreach (var Rep in Reps)
			{
				Rep.optimize();
			}
			//Notify nodes of the current CS
			//field.com.Start();
			foreach (var Rep in Reps)
			{
				Rep.notify();
			}
			Field.Communications.startOfAnEra("Optimization");
		}

		#region Statistics
		/// <summary>
		/// Gets the average WSN lifetime increase per node
		/// </summary>
		/// <returns>The WSN aggregate lifetime increase</returns>
		public  double getWLI()
		{
			double ret = 0;
			foreach (var Rep in Reps)
			{
				foreach (var cluster in Rep.Clusters)
				{
					ret += (cluster.Nodes.Count - 1);
				}
			}
			return double.Parse(String.Format("{0:0.00}", 100.0 * ret / getNC()));
		}

		/// <summary>
		/// Gets the average minimum reportings accuracy derived from the field clustering
		/// </summary>
		/// <returns>The average reportings accuracy</returns>
		public  double getACC()
		{
			double ret = 0;
			foreach (var Rep in Reps)
			{
				ret += Rep.getACC();
			}
			return double.Parse(String.Format("{0:0.00}", ret / Reps.Count));
		}

		/// <summary>
		/// Gets the number of coalitions rising from the coalitional process
		/// </summary>
		/// <returns>The number of coalitons</returns>
		public  double getNC()
		{
			double ret = 0;
			foreach (var Rep in Reps)
			{
				foreach (var Cluster in Rep.Clusters)
				{
					ret++;
				}
			}
			return ret;
		}

		/// <summary>
		/// Gets the average number of messages sent per node
		/// </summary>
		/// <returns>The average number of messages/node.</returns>
		public  double getMSG()
		{
			double ret = 0;
			foreach (var node in field.Get())
			{
				ret += (node.MessageCount.Sent);
			}
			return ret / (1.0 * SimLib.Properties.Simulation.Default.Nodes);
		}

		/// <summary>
		/// Gets the average node degree
		/// </summary>
		public  void getDegree()
		{
			int ret = 0;
			foreach (var neighbors in field.Neighbors.Values)
			{
				ret += neighbors.Count;
			}
			System.Console.WriteLine("Mean degree: " + ret / SimLib.Properties.Simulation.Default.Nodes);
			return;
		}

		#endregion
		#region Printing
		/// <summary>
		/// Prints the current cluster formation
		/// </summary>
		public void printClusters()
		{
			Console.WriteLine("\nPrinting Clusters\n" +
					"==============");
			int MaxLength = field.Get().Count.ToString().Length;
			MaxLength+=4;
			for (int x = 0; x < field.Width; x++)
			{
				for (int y = 0; y < field.Height; y++)
				{
					if (field.Get(new Point(x, y)) != null && field.Get(new Point(x, y)).GetType().FullName.Equals("CGTF.WSNode"))
					{
						Console.Write(((WSNode)field.Get(new Point(x, y))).CID.ToString().PadLeft(MaxLength));
					}
					else if (field.Get(new Point(x, y)) != null && !field.Get(new Point(x, y)).GetType().FullName.Equals("CGTF.WSNode"))
					{
						Console.Write((field.Get(new Point(x, y))).Info.ID.ToString().PadLeft(MaxLength));
					}
					else
					{
						Console.Write("".PadLeft(MaxLength));
					}
				}
				Console.WriteLine();
			}
		}
		#endregion
	}
}
