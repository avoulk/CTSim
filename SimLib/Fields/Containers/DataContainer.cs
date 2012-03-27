using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using SimLib.Properties;
using System.Diagnostics;
using MathNet.Numerics.Distributions;

namespace SimLib.Fields
{
	public class DataContainer
	{
		Field field;
		double[][] Data { get; set; }
		Random random;

		/// <summary>
		/// Creates A data container for the field
		/// </summary>
		/// <param name="field">The field with the data</param>
		public DataContainer(Field field)
		{
			this.field = field;
			Data = new double[this.field.Width][];
			random = new Random();
			for (int i = 0; i < Data.Length; i++)
			{
				Data[i] = new double[this.field.Height];
			}
			Init();
		}

		/// <summary>
		/// Gets the data in the specified coordinates
		/// </summary>
		/// <param name="Coordinates">The coordinates to fetch the data for</param>
		/// <returns>The data of the undelying data network</returns>
		public double Get(Point Coordinates)
		{
			return Data[Coordinates.X][Coordinates.Y];
		}

		/// <summary>
		/// Sets new data to the specified Coordinates
		/// </summary>
		/// <param name="Coordinates">The coordinates of the data change</param>
		/// <param name="data">The new data</param>
		public void Set(Point Coordinates, double data)
		{
			GenerateChange(Coordinates, data / Data[Coordinates.X][Coordinates.Y] - 1);
		}

		/// <summary>
		/// Initializes the Data of the field
		/// </summary>
		private void Init()
		{
			//Generate random data to the network. These are partially correlated.
			//Console.WriteLine("Initiating Data network..");
			var Gaussian = new Normal(Simulation.Default.Data_Initial_Mean, Simulation.Default.Data_Initial_Sigma);
			for (int i = 0; i < Data.Length; i++)
			{
				for (int j = 0; j < Data[i].Length; j++)
				{
					Data[i][j] = Gaussian.Density(new Random(i * Data[i].Length + j).NextDouble()) / Gaussian.Density(0) * Simulation.Default.Data_Initial_Mean;
				}
			}
			//Setup data correlation changes
			//Console.WriteLine("Setting up " + Simulation.Default.Data_Changes + " data changes..");
			for (int i = 0; i < Simulation.Default.Data_Changes; i++)
			{
				GenerateChange();
			}
		}

		/// <summary>
		/// Generates A random change in the field. Both the coordinates and the 
		/// </summary>
		public void GenerateChange()
		{
			Bernoulli rBernoulli = new Bernoulli(0.5); // Generate A random sign
			double percentage = (rBernoulli.Sample() > 0.5 ? -1 : 1) * Simulation.Default.Data_Change_Maximum_Percentage * random.NextDouble();
			GenerateChange(new Point(random.Next(field.Width), random.Next(field.Height)), percentage);
		}

		/// <summary>
		/// Generates A random change to the specified point
		/// </summary>
		/// <param name="reference">The reference point</param>
		public void GenerateChange(Point reference)
		{
			Bernoulli rBernoulli = new Bernoulli(0.5);
			double percentage = (rBernoulli.Sample() > 0.5 ? -1 : 1) * Simulation.Default.Data_Change_Maximum_Percentage * random.NextDouble();
			GenerateChange(reference, percentage);
		}

		/// <summary>
		/// Generates A random change with A specified percentage. The Point of the change is randomly generated
		/// </summary>
		/// <param name="percentage">The percentage of the data change</param>
		public void GenerateChange(double percentage)
		{
			GenerateChange(new Point(random.Next(field.Width), random.Next(field.Height)), percentage);
		}

		/// <summary>
		/// Generate A data change to the specified point
		/// </summary>
		/// <param name="reference">The reference point where the data change is being made</param>
		/// <param name="percentage">The data change percentage</param>
		public void GenerateChange(Point reference, double percentage)
		{
			//Generate the change to the own point
			//Console.Write(reference.ToString() + "\tPercentage = " + (int)System.SimMath.Round(percentage*100) + "%\tData " + (int)System.SimMath.Round(Data[reference.X][reference.Y]) + "\item=> ");
			for (int x = 0; x < Data.Length; x++)
			{
				for (int y = 0; y < Data[x].Length; y++)
				{
					Point other = new Point(x, y);
					Data[x][y] = NewRelativeChangeData(reference, other, percentage);
				}
			}
			//Console.WriteLine((int)System.SimMath.Round(Data[reference.X][reference.Y]));
		}

		/// <summary>
		/// Calculates the new data after A point data change
		/// </summary>
		/// <param name="reference">The reference point where the data change was made</param>
		/// <param name="other">The point to calculate the effect of the data change to</param>
		/// <returns>The new data of the Point "other"</returns>
		private double NewRelativeChangeData(Point reference, Point other, double percentage)
		{
			double similarity = SimilarityPDF(SimMath.Distance.Get(reference, other));
			return Data[other.X][other.Y] * (1 + similarity * percentage);
		}

		/// <summary>
		/// Sets up A similarity function to indicate correlation
		/// </summary>
		/// <param name="distance">The distance between two nodes</param>
		/// <returns>The data similarity "PDF"</returns>
		private double SimilarityPDF(double distance)
		{
			var Gaussian = new Normal(0, Simulation.Default.Sigma);
			double normalizer = Gaussian.Density(0);
			return Gaussian.Density(distance) / normalizer;
		}
	}
}
