using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CGTF.Sim.Clustering
{
	public class Cluster
	{
		public int ID { get; set; }
		public List<WSNode> Nodes { get; set; }
		public List<int> Neighboring { get; set; }
		public double Min { get; set; }
		public double Max { get; set; }
		public double StartData { get; set; }
		public int StartID { get; set; }
		private double value;
		public double Value { get { return Nodes.Count * (Nodes.Count - 1); } set { this.value = value; } }

		public Cluster(int ID)
		{
			this.ID = ID;
			Nodes = new List<WSNode>();
			Neighboring = new List<int>();
			Min = Double.MaxValue;
			Max = Double.MinValue;
			Value = 0;
			StartData = Double.MinValue;
			StartID = Int32.MinValue;
		}

		/// <summary>
		/// Adds a node to the cluster
		/// </summary>
		/// <param name="Node">The node to add</param>
		public void Add(WSNode node)
		{
			if (Nodes.Contains(node))
			{
				return;
			}
			Nodes.Add(node);
			Max = Math.Max(Max, node.Data);
			Min = Math.Min(Min, node.Data);
			if (StartData == Double.MinValue)
			{
				StartData = node.SIDMeasurement;
				StartID = node.SID;
			}
			else if (StartID != node.SID)
			{
				Console.WriteLine("Holy SHit!");
			}
			//TODO: Add neighboring clusters
		}

		public double getAccuracy()
		{
			double _MinAccuracy = 10;
			foreach (var node1 in Nodes)
			{
				double __Accuracy = getDataChange(node1.Data, StartData);
				_MinAccuracy = Math.Min(__Accuracy, _MinAccuracy); //It is actually the minimum accuracy :-)
			}
			if (_MinAccuracy > 1)
			{
				_MinAccuracy = double.MaxValue;
			}
			else if (_MinAccuracy < 0)
			{
				_MinAccuracy = double.MinValue;
			}
			return _MinAccuracy;
		}

		private double getDataChange(double otherData, double referenceData)
		{
			return 1 - Math.Abs((otherData - referenceData) / referenceData);
		}
	}
}
