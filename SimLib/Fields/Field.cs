using System;
using System.Collections.Generic;
using System.Drawing;
using SimLib.Abstractions.Networking;
using SimLib.Nodes;
using SimLib.Messages;

namespace SimLib.Fields
{
	public class Field
	{
		Rectangle field;
		NodeContainer Nodes;
		public static DataContainer Data { get; set; }
		public Dictionary<int, List<int>> Neighbors { get; set; }

		public static Com<IMessage> Communications;

		/// <summary>
		/// Wrapper for the Field Height property
		/// </summary>
		public int Height
		{
			get
			{
				return field.Height;
			}
			set
			{
				field.Height = value;
			}
		}

		/// <summary>
		/// Wrapper for the Field Width property
		/// </summary>
		public int Width
		{
			get
			{
				return field.Width;
			}
			set
			{
				field.Width = value;
			}
		}

		/// <summary>
		/// A rectangular field
		/// </summary>
		public Field()
		{
			int lenght = Properties.Simulation.Default.Field;
			field = new Rectangle(new Point(lenght, lenght), new Size(lenght, lenght));
			Nodes = new NodeContainer();
			Neighbors = new Dictionary<int, List<int>>();
		}

		/// <summary>
		/// Guess A random empty coordinate within the field
		/// </summary>
		/// <returns>The random coordinate</returns>
		public Point getEmptyRandomCoordinates()
		{
			Point ret;
			Random random = new Random();
			do
			{
				ret = new Point(random.Next(field.Width), random.Next(field.Height));
			} while (Nodes.contains(ret));
			return ret;
		}

		/// <summary>
		/// Guess A random (not necessarily empty) coordinate within the field
		/// </summary>
		/// <returns>The random coordinate</returns>
		public Point getRandomCoordinates()
		{
			Random random = new Random();
			return new Point(random.Next(field.Width), random.Next(field.Height));
		}

		/// <summary>
		/// Gets the closest to A specified reference point(non-empty) coordinates
		/// </summary>
		/// <param name="point">The reference point</param>
		/// <returns>The non-empty coordinates closest to the reference point</returns>
		public Point getCloseCoordinates(Point point)
		{
			int range = Properties.Simulation.Default.Field / 5;
			Point ret = new Point(-1, -1);
			bool found = false;
			//Loop through
			for (int i = 0; i < range; i++)
			{
				//Check if field borders
				int minX = (point.X - i >= 0) ? (point.X - i) : (0);
				int maxX = (point.X + i <= field.Width) ? (point.X + i) : (field.Width);
				int minY = (point.Y - i >= 0) ? (point.Y - i) : (0);
				int maxY = (point.Y + i <= field.Height) ? (point.Y + i) : (field.Height);

				for (int x = minX; x < maxX; x++)
				{
					for (int y = minY; y < maxY; y++)
					{
						if (Nodes.contains(new Point(x, y)))
						{
							ret = new Point(x, y);
							found = true;
							break;
						}
					}
					if (found)
						break;
				}
				if (found)
					break;
			}
			return ret;
		}

		/// <summary>
		/// Adds A node to the field
		/// </summary>
		/// <param name="node">The INode to be added</param>
		public void Add(INode node)
		{
			Nodes.Add(node);
		}

		/// <summary>
		/// Gets an INode from the field
		/// </summary>
		/// <param name="ID">The ID of the INode to get</param>
		/// <returns>The specified INode</returns>
		public INode Get(int ID)
		{
			return Nodes.Get(ID);
		}

		/// <summary>
		/// Gets an INode from the field
		/// </summary>
		/// <param name="Coordinates">The coordinates of the INode to get</param>
		/// <returns>The specified INode</returns>
		public INode Get(Point Coordinates)
		{
			return Nodes.Get(Coordinates);
		}

		/// <summary>
		/// Gets A List containing all the INodes of the field
		/// </summary>
		/// <returns>The List of all the INodes of the field</returns>
		public List<INode> Get()
		{
			return Nodes.List();
		}

		/// <summary>
		/// Finalizes the initalization of the Field
		/// </summary>
		public void Init()
		{
			//Setup Data Stuff
			Data = new DataContainer(this);
			SetupINodeNeighbors();
			foreach (var node in Nodes.List())
			{
				node.Sense();
			}

			//Enable communications
			Communications = new Com<IMessage>(Nodes);
			//Console.WriteLine("Done");
		}

		/// <summary>
		/// Sets up the neighbors of the INodes
		/// </summary>
		private void SetupINodeNeighbors()
		{
			//Console.WriteLine("Setting up node neighbors..");
			foreach (var node in Nodes.List())
			{
				Neighbors.Add(node.Info.ID, getNeighborsByDistance(node.Info.ID));
				Nodes.addNeighbor(node.Info.ID,Neighbors[node.Info.ID]);
			}
		}

		/// <summary>
		/// Gets a list of neighbors, the closest being first in the list
		/// </summary>
		/// <param name="ID"></param>
		/// <returns></returns>
		private List<int> getNeighborsByDistance(int ID)
		{
			List<int> ret = new List<int>();
			foreach (var _INode in Nodes.List())
			{
				if (SimMath.Distance.WithinRange(Nodes.Get(ID), Nodes.Get(_INode.Info.ID)) && ID != _INode.Info.ID)
					ret.Add(_INode.Info.ID);
			}
			ret.Sort( 
				delegate( int ID1, int ID2 )
				{ 
					return SimMath.Distance.Get(Nodes.Get(ID).Info.Coordinates, Nodes.Get(ID1).Info.Coordinates).CompareTo(
						SimMath.Distance.Get(Nodes.Get(ID).Info.Coordinates, Nodes.Get(ID2).Info.Coordinates));
				});
			return ret;
		}

		/// <summary>
		/// Prints stuff to the console
		/// </summary>
		/// <param name="DrawNodes">If true, it will print the nodes</param>
		/// <param name="DrawData">If true, it will print the data</param>
		public void print(bool DrawNodes, bool DrawData)
		{
			if (DrawNodes)
			{
				Console.WriteLine("\nPrinting nodes\n" +
									"==============");
				int MaxLength = this.Nodes.List().Count.ToString().Length;
				MaxLength++;
				for (int x = 0; x < field.Width; x++)
				{
					for (int y = 0; y < field.Height; y++)
					{
						if (Nodes.contains(new Point(x, y)))
						{
							Console.Write(Nodes.Get(new Point(x, y)).Info.ID.ToString().PadLeft(MaxLength));
						}
						else
						{
							Console.Write("".PadLeft(MaxLength));
						}
					}
					Console.WriteLine();
				}
			}

			if (DrawData)
			{
				Console.WriteLine("\nPrinting data\n" +
									"=============");
				int MaxLength = 0;
				for (int x = 0; x < field.Width; x++)
				{
					for (int y = 0; y < field.Height; y++)
					{
						int _len = ((int)Math.Round(Data.Get(new Point(x,y)))).ToString().Length;
						MaxLength = Math.Max(MaxLength, _len);
					}
				}
				MaxLength++;
				//Actual Printing
				for (int x = 0; x < field.Width; x++)
				{
					for (int y = 0; y < field.Height; y++)
					{
						Console.Write( ((int)Math.Round(Data.Get(new Point(x,y)))).ToString().PadLeft(MaxLength));
					}
					Console.WriteLine();
				}
			}
		}

	}
}
