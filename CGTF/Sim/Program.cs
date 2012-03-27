using System.Threading;
using SimLib.Fields;
using SimLib.Messages.Types;
using SimLib.Messages;
using System;
using SimLib.Abstractions.Networking;

namespace RunningTest
{
	public class Program
	{
		static Field field;
		static Thread mainThread;

		/// <summary>
		/// Set up environment and start simulation
		/// </summary>
		/// <param name="args"></param>
		public static void Main(string[] args)
		{
			Program.field = new Field();
			for (int ID = 0; ID < SimLib.Properties.Simulation.Default.Nodes; ID++)
			{
				field.Add(new WSNode(ID, field.getEmptyRandomCoordinates()));
            }
            mainThread = Thread.CurrentThread;
			field.Init();
			field.com.EmptyQueueEvent += Proceed;
            simulate();
            try
            {
                Thread.Sleep(Timeout.Infinite);
            }
            catch (ThreadInterruptedException) { }
			Console.WriteLine("MSG: " + getMSG());
		}

		/// <summary>
		/// Starts actual simulation
		/// </summary>
		private static void simulate()
		{
			field.com.Start();
			SetUpNeighbors();
		}

		/// <summary>
		/// Sets up the neighbors of the field nodes via messaging
		/// </summary>
		private static void SetUpNeighbors()
		{
			foreach (var node in field.Get())
			{
				node.transmit(new MSGNeighborDiscovery(node.Info.ID));
			}
		}

		/// <summary>
		/// Gets the average WSN lifetime increase
		/// </summary>
		/// <returns>The WSN aggregate lifetime increase</returns>
		public static double getWLI()
		{
			double ret = 0;
			return ret;
		}

		/// <summary>
		/// Gets the average reportings accuracy derived from the field clustering
		/// </summary>
		/// <returns>The average reportings accuracy</returns>
		public static double getACC()
		{
			double ret = 0;
			return ret;
		}

		/// <summary>
		/// Gets the number of coalitions rising from the coalitional process
		/// </summary>
		/// <returns>The number of coalitons</returns>
		public static double getNC()
		{
			double ret = 0;
			return ret;
		}

		/// <summary>
		/// Gets the average number of messages sent per node
		/// </summary>
		/// <returns>The average number of messages/node.</returns>
		public static double getMSG()
		{
			double ret = 0;
			foreach (var node in field.Get())
			{
				ret += (node.MessageCount.Sent + node.MessageCount.Received);
			}
			return ret / (1.0 * SimLib.Properties.Simulation.Default.Nodes);
		}

		/// <summary>
		/// Gets the average node degree
		/// </summary>
		public static void getDegree()
		{
			int ret = 0;
			foreach (var neighbors in field.Neighbors.Values)
			{
				ret += neighbors.Count;
			}
			System.Console.WriteLine("Mean degree: " + ret / SimLib.Properties.Simulation.Default.Nodes);
			return;
		}

		/// <summary>
		/// Instructs the program to proceed execution
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="eqea"></param>
		private static void Proceed(Object sender, SimLib.Abstractions.Networking.Com<IMessage>.EmptyQueueEventArgs eqea)
		{
			mainThread.Interrupt();
		}
	}
}
