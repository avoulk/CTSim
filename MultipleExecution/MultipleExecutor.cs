using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Threading;
using CGTF;

namespace MultipleExecution
{
	// ! IMPORTANT !
	//
	// Input for the single simulation should be given in the following functions:
	//	1. Main					: when the method simulate is called, to enable single simulation process. 
	//	2. InitializeStatistics	: to Add specific data gatherers and mappers.
	//
	class MultipleExecutor
	{
		// Main (single) simulation method
		delegate void CoreSingleSimulation(string[] args);
		// Method of the main simulation class gathering statistics
		delegate double StatisticsGatherer();
		// Data Mapper
		static Dictionary<string, StatisticsGatherer> RepetitionStatisticsMapper;
		// Data handler for a single repetition
		static Dictionary<string, List<double>> RepetitionStatisticsHolder;
		// Data hander for a single Point repetition (a single point)
		static Dictionary<string, double> PointStatisticsHolder;
		// Data handler for the whole simulation
		static Dictionary<int, Dictionary<int, Dictionary<string, double>>> SimulationStatisticsHolder;


		/// <summary>
		/// Main execution of the multiple execution paradigm.
		/// Remember to define the delegate!!!!
		/// </summary>
		/// <param name="args">Command line paramters</param>
		static void Main(string[] args)
		{
			Console.Write("Would you like to check the configuration [y/N]? ");
			if (Console.ReadLine().Equals("y", StringComparison.InvariantCultureIgnoreCase))
			{
				print();
			}
			Console.Write("\nUse existing Config [Y/n]? ");
			if (Console.ReadLine().Equals("n", StringComparison.InvariantCultureIgnoreCase))
			{
				inputConfig();
			}
			print();
			Console.WriteLine("\nDone Configuring.\nStarting simulation\n");
			//Main simulation stuff
			//Here come the changes for the statistics gathering!
			InitializeStatistics();
			//Insert as delegate the main function of the main simulation project file!!
			simulate();
			//Gathering Full Statistics
			Console.WriteLine("Gathering full simulation statistics..");
			writeStatistics();
		}

		/// <summary>
		/// Actual multiple simulation
		/// </summary>
		private static void simulate()
		{
			//Iterate through the Slow Changing Variable
			foreach (string SlowChangeVariableValueString in Properties.MultipleExecution.Default.Slow_Changing_Variable_Values)
			{
				//Change Slow Changing Variable
				Console.WriteLine("Changing " + Properties.MultipleExecution.Default.Slow_Changing_Variable + " to " + SlowChangeVariableValueString);
				Properties.MultipleExecution.Default.Slow_Changing_Variable_Current = Int32.Parse(SlowChangeVariableValueString);
				Properties.MultipleExecution.Default.Save();
				//Iterate through the slow changing variable
				SimulationStatisticsHolder.Add(Properties.MultipleExecution.Default.Slow_Changing_Variable_Current, new Dictionary<int, Dictionary<string, double>>());
				Console.Write(Properties.MultipleExecution.Default.Fast_Changing_Variable + ": ");
				for (int FastChangingVariableCurrent = Properties.MultipleExecution.Default.Fast_Changing_Variable_Minimum;
						 FastChangingVariableCurrent <= Properties.MultipleExecution.Default.Fast_Changing_Variable_Maximum;
						 FastChangingVariableCurrent += Properties.MultipleExecution.Default.Fast_Changing_Variable_Increment)
				{
					Console.Write(FastChangingVariableCurrent + " ");
					//Save current value for fast changing variable
					Properties.MultipleExecution.Default.Fast_Changing_Variable_Current = FastChangingVariableCurrent;
					Properties.MultipleExecution.Default.Save();
					//Change single simulation configuration values
					SimLib.Properties.Simulation.Default[Properties.MultipleExecution.Default.Slow_Changing_Variable] = Properties.MultipleExecution.Default.Slow_Changing_Variable_Current;
					SimLib.Properties.Simulation.Default[Properties.MultipleExecution.Default.Fast_Changing_Variable] = Properties.MultipleExecution.Default.Fast_Changing_Variable_Current;
					SimLib.Properties.Simulation.Default.Save();

					RepetitionStatisticsHolder = new Dictionary<string, List<double>>();
					RepetitionStatisticsHolder.Add("WLI", new List<double>());
					RepetitionStatisticsHolder.Add("NC", new List<double>());
					RepetitionStatisticsHolder.Add("MSG", new List<double>());
					RepetitionStatisticsHolder.Add("ACC", new List<double>());

					//Iterate through the multiple repetitions of a single simulation configuration
					for (int repetition = 0; repetition < Properties.MultipleExecution.Default.Repetitions_Per_Single_Execution; repetition++)
					{
						//Perform actual simulation
						SimExec Sim = new SimExec();
						RepetitionStatisticsMapper = new Dictionary<string, StatisticsGatherer>();
						RepetitionStatisticsMapper.Add("WLI", Sim.getWLI);
						RepetitionStatisticsMapper.Add("NC", Sim.getNC);
						RepetitionStatisticsMapper.Add("MSG", Sim.getMSG);
						RepetitionStatisticsMapper.Add("ACC", Sim.getACC);

						Sim.Run();
						//Gather repetition statistics
						gatherPointStatistics(false);
						Sim = null;
						GC.Collect();
						GC.WaitForPendingFinalizers();
					}
					//Gather statistics for single plot point
					gatherPointStatistics(true);
					SimulationStatisticsHolder[Properties.MultipleExecution.Default.Slow_Changing_Variable_Current].Add(FastChangingVariableCurrent, new Dictionary<string, double>());
					foreach (var Metric in PointStatisticsHolder.Keys)
					{
						SimulationStatisticsHolder[Properties.MultipleExecution.Default.Slow_Changing_Variable_Current][FastChangingVariableCurrent].Add(Metric, PointStatisticsHolder[Metric]);
					}
					//Clear current statistics
					PointStatisticsHolder = new Dictionary<string, double>();
					foreach (var sg in RepetitionStatisticsMapper.Keys)
					{
						RepetitionStatisticsHolder[sg] = new List<double>();
					}
				}
				Console.WriteLine();
			}
			//Revert simulation configuration file to its defaults
			Console.WriteLine("\nDone!\nReverting simulation configuration file to its defaults..");
			SimLib.Properties.Simulation.Default[Properties.MultipleExecution.Default.Slow_Changing_Variable] = Properties.MultipleExecution.Default.Slow_Changing_Variable_Initial;
			SimLib.Properties.Simulation.Default[Properties.MultipleExecution.Default.Fast_Changing_Variable] = Properties.MultipleExecution.Default.Fast_Changing_Variable_Initial;
		}

		#region Statistics
		/// <summary>
		/// Initializes statistics gathering.
		/// The data handlers should be defined and initialized here!
		/// </summary>
		private static void InitializeStatistics()
		{
			PointStatisticsHolder = new Dictionary<string, double>();
			SimulationStatisticsHolder = new Dictionary<int, Dictionary<int, Dictionary<string, double>>>();
		}

		/// <summary>
		/// Gathers meaningful statistics for the simulation performed
		/// </summary>
		/// <param name="GatherBlockStatisticsSummary">If true, then the simulation is supposed to be over, therefore multiple simulation handling is performed. If false, then the output of a single repetition is examined.</param>
		private static void gatherPointStatistics(bool GatherBlockStatisticsSummary)
		{
			if (GatherBlockStatisticsSummary) // Global statistics calculation
			{
				foreach (var StatisticsID in RepetitionStatisticsMapper.Keys)
				{
					double avgStat = 0;
					foreach (double metric in RepetitionStatisticsHolder[StatisticsID])
					{
						avgStat += metric / (1.0 * RepetitionStatisticsHolder[StatisticsID].Count);
					}
					//if (StatisticsID.Equals("MSG"))
					//    Console.WriteLine("Added to point statistics list: " + StatisticsID + " " + avgStat);
					PointStatisticsHolder.Add(StatisticsID, avgStat);
				}
			}
			else // Add single simulation statistics
			{
				foreach (var StatisticsID in RepetitionStatisticsMapper.Keys)
				{
					double value = RepetitionStatisticsMapper[StatisticsID].Invoke();
					RepetitionStatisticsHolder[StatisticsID].Add(value);
					//if (StatisticsID.Equals("MSG"))
					//    Console.WriteLine("Added to repetition list: " + StatisticsID + " " + value);
				}
			}
		}

		/// <summary>
		/// Exports the simulation statistics to the user desktop folder
		/// </summary>
		private static void writeStatistics()
		{
			string UpperDirectoryPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), Properties.MultipleExecution.Default.Slow_Changing_Variable + "-" + Properties.MultipleExecution.Default.Fast_Changing_Variable);
			while (System.IO.Directory.Exists(UpperDirectoryPath))
			{
				UpperDirectoryPath += ("-" + System.IO.Path.GetRandomFileName());
				Console.WriteLine("\t[APP] Output directory exists. Generating new one..");
			}
			Console.WriteLine("Outup directory: " + UpperDirectoryPath);
			System.IO.Directory.CreateDirectory(UpperDirectoryPath);
			String mFilePath = System.IO.Path.Combine(UpperDirectoryPath, "plotData.m");
			System.IO.FileStream mFile = System.IO.File.Create(mFilePath);
			mFile.Close();
			//Write .m File
			string[] markers = { "-d", "--s", ":*", "-.x", "-.v", "--", ":s", "-v", "->", "-<", "-nData", "-h" };
			String mData = "clc" + Environment.NewLine + "clear" + Environment.NewLine;
			//Write simulation data
			foreach (var Metric in RepetitionStatisticsMapper.Keys)
			{
				int _marker = 0;
				mData += Environment.NewLine + "% Calculating and plotting data over metric :: " + Metric + Environment.NewLine;
				mData += "Fig" + Metric + " = figure();" + Environment.NewLine;
				foreach (var Slow in SimulationStatisticsHolder.Keys)
				{
					String SlowDirectoryPath = System.IO.Path.Combine(UpperDirectoryPath, Slow.ToString());
					System.IO.Directory.CreateDirectory(SlowDirectoryPath);
					String metricDataFilePath = System.IO.Path.Combine(SlowDirectoryPath, Metric + ".m");
					System.IO.FileStream metricDataFile = System.IO.File.Create(metricDataFilePath);
					metricDataFile.Close();
					String data = "";
					foreach (var Fast in SimulationStatisticsHolder[Slow].Keys)
					{
						data += Fast.ToString() + "\t" + SimulationStatisticsHolder[Slow][Fast][Metric].ToString().Replace(",", ".") + Environment.NewLine;
					}
					System.IO.File.WriteAllText(metricDataFilePath, data);

					//Now write the plot stuff :-)
					mData += Metric + Slow.ToString() + " = importdata ('" + System.IO.Path.Combine(Slow.ToString(), Metric + ".m") + "', '\\t');" + Environment.NewLine;
					mData += "x" + Metric + Slow.ToString() + " = " + Metric + Slow.ToString() + "(:,1);" + Environment.NewLine;
					mData += "y" + Metric + Slow.ToString() + " = " + Metric + Slow.ToString() + "(:,2);" + Environment.NewLine;
					mData += "figure(" + "Fig" + Metric + ")" + Environment.NewLine;
					mData += "plot(" + "x" + Metric + Slow.ToString() + ", y" + Metric + Slow.ToString() + ", '" + markers[_marker] + "');" + Environment.NewLine;
					_marker++;
					mData += "hold on;" + Environment.NewLine + "grid on;" + Environment.NewLine;
				}
				mData += "xlabel('" + Properties.MultipleExecution.Default.Fast_Changing_Variable + "');" + Environment.NewLine + "ylabel('" + Metric + "');" + Environment.NewLine + "legend('";
				int _count = 0;
				foreach (var Slow in SimulationStatisticsHolder.Keys)
				{
					_count++;
					mData += Properties.MultipleExecution.Default.Slow_Changing_Variable + " = " + Slow.ToString() + "'" + (_count != (SimulationStatisticsHolder.Keys.Count) ? ", '" : "");
				}
				mData += ");" + Environment.NewLine;
			}
			mData += Environment.NewLine + "% Done Plotting! :-)" + Environment.NewLine + "hold off;" + Environment.NewLine;
			System.IO.File.WriteAllText(mFilePath, mData);
		}
		#endregion

		#region Input configuration
		/// <summary>
		/// Gets NEW configuration information
		/// </summary>
		private static void inputConfig()
		{
			getFastChangingVariableStuff();
			getSlowChangingVariableStuff();
			saveConfig();
		}

		/// <summary>
		/// Gets the fast changing variable stuff
		/// </summary>
		private static void getFastChangingVariableStuff()
		{
			Console.Write("Enter the FAST changing (x-axis) variable. Possible valid options are: Nodes, Field, Representatives, Sigma, Range, QoS, Data_Changes. Enter option: ");
			Properties.MultipleExecution.Default.Fast_Changing_Variable = Console.ReadLine();
			//Setup the Changing Variable Name and Initial Value
			Properties.MultipleExecution.Default.Fast_Changing_Variable_Initial = Convert.ToInt32(SimLib.Properties.Simulation.Default.Properties[Properties.MultipleExecution.Default.Fast_Changing_Variable].DefaultValue as String);
			Console.WriteLine("\t[APP]\tDetected default configuration value: " + Properties.MultipleExecution.Default.Fast_Changing_Variable_Initial);
			Console.Write("Enter minimum value:\t");
			Properties.MultipleExecution.Default.Fast_Changing_Variable_Minimum = Convert.ToInt32(Console.ReadLine());
			Properties.MultipleExecution.Default.Fast_Changing_Variable_Current = Properties.MultipleExecution.Default.Fast_Changing_Variable_Minimum;
			Console.Write("Enter maximum value:\t");
			Properties.MultipleExecution.Default.Fast_Changing_Variable_Maximum = Convert.ToInt32(Console.ReadLine());
			Console.Write("Enter increment:\t");
			Properties.MultipleExecution.Default.Fast_Changing_Variable_Increment = Convert.ToInt32(Console.ReadLine());
			Console.Write("Enter repetitions:\t");
			Properties.MultipleExecution.Default.Repetitions_Per_Single_Execution = Convert.ToInt32(Console.ReadLine());
		}

		/// <summary>
		/// Gets the slow changing variable stuff
		/// </summary>
		private static void getSlowChangingVariableStuff()
		{
			Console.Write("\nEnter the SLOW changing (different curves) variable. Possible valid options are: Nodes, Field, Representatives, Sigma, Range, QoS, Data_Changes. Enter option: ");
			Properties.MultipleExecution.Default.Slow_Changing_Variable = Console.ReadLine();
			//Setup the Changing Variable Name and Initial Value
			Properties.MultipleExecution.Default.Slow_Changing_Variable_Initial = Convert.ToInt32(SimLib.Properties.Simulation.Default.Properties[Properties.MultipleExecution.Default.Slow_Changing_Variable].DefaultValue as String);
			Console.WriteLine("\t[APP]\tDetected default configuration value: " + Properties.MultipleExecution.Default.Slow_Changing_Variable_Initial);
			Console.Write("Enter values for the slow changing variable, separated by spaces: ");
			StringCollection sc = new StringCollection();
			sc.AddRange(Console.ReadLine().Trim().Split(' '));
			Properties.MultipleExecution.Default.Slow_Changing_Variable_Values = sc;
			Properties.MultipleExecution.Default.Fast_Changing_Variable_Current = Convert.ToInt32(Properties.MultipleExecution.Default.Slow_Changing_Variable_Values[0]);
		}

		/// <summary>
		/// Prints the configuration
		/// </summary>
		private static void print()
		{
			Console.WriteLine("\nConfiguration Overview:\n=======================\n");
			Console.WriteLine("Fast changing variable name:\t" + Properties.MultipleExecution.Default.Fast_Changing_Variable);
			Console.WriteLine("                       min:\t" + Properties.MultipleExecution.Default.Fast_Changing_Variable_Minimum);
			Console.WriteLine("                       max:\t" + Properties.MultipleExecution.Default.Fast_Changing_Variable_Maximum);
			Console.WriteLine("                       inc:\t" + Properties.MultipleExecution.Default.Fast_Changing_Variable_Increment);
			Console.WriteLine("                       rep:\t" + Properties.MultipleExecution.Default.Repetitions_Per_Single_Execution);
			Console.WriteLine("\nSlow changing variable name:\t" + Properties.MultipleExecution.Default.Slow_Changing_Variable);
			Console.Write("                       val:\t");
			foreach (var value in Properties.MultipleExecution.Default.Slow_Changing_Variable_Values)
			{
				Console.Write(value + " ");
			}
			Console.WriteLine();
		}

		/// <summary>
		/// Saves the configuration
		/// </summary>
		private static void saveConfig()
		{
			Console.WriteLine("Saving configuration..");
			Properties.MultipleExecution.Default.Save();
		}
		#endregion
	}
}
