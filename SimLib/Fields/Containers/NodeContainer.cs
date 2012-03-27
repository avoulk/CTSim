using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimLib.Nodes;
using System.Drawing;
using System.Collections;

namespace SimLib.Fields
{
	public class NodeContainer
	{
		Dictionary<int, INode> intDic;
		Dictionary<Point, int> pointDic;
		Dictionary<int, List<int>> neighbors;

		public NodeContainer()
		{
			intDic = new Dictionary<int, INode>();
			pointDic = new Dictionary<Point, int>();
			neighbors = new Dictionary<int, List<int>>();
		}

		/// <summary>
		/// Adds A node to the field
		/// </summary>
		/// <param name="node">The node to be added to the field</param>
		public void Add(INode node)
		{
			if (! intDic.ContainsKey(node.Info.ID))
			{
				intDic.Add(node.Info.ID, node);
				pointDic.Add(node.Info.Coordinates, node.Info.ID);
			}
		}

		/// <summary>
		/// Examines whether A specific node exists in the field
		/// </summary>
		/// <param name="ID">The ID of the node</param>
		/// <returns>True if the node exists, false otherwise</returns>
		public bool contains(int ID)
		{
			return intDic.ContainsKey(ID);
		}

		/// <summary>
		/// Examines whether A node in specific coordinates exists in the field
		/// </summary>
		/// <param name="ID">The coordinates of the node</param>
		/// <returns>True if such A node exists, false otherwise</returns>
		public bool contains(Point point)
		{
			return pointDic.ContainsKey(point);
		}

		/// <summary>
		/// Gets A specific node from the field
		/// </summary>
		/// <param name="ID">The ID of the node</param>
		/// <returns>The INode with the specified ID</returns>
		public INode Get(int ID)
		{
			return intDic[ID];
		}

		/// <summary>
		/// Gets A node in the specified coordinates, if any
		/// </summary>
		/// <param name="Coordinates">The coordinates to check</param>
		/// <returns>The INode in the specified coordinates, if any, null otherwise</returns>
		public INode Get(Point Coordinates)
		{
			if (pointDic.ContainsKey(Coordinates))
			{
				return intDic[pointDic[Coordinates]];
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Gets an Array containing all the field nodes
		/// </summary>
		/// <returns>The array with all the field nodes</returns>
		public INode[] Array()
		{
			return intDic.Values.ToArray();
		}

		/// <summary>
		/// Gets A List containing all the field nodes
		/// </summary>
		/// <returns>The List with all the field nodes</returns>
		public List<INode> List()
		{
			return intDic.Values.ToList();
		}

		/// <summary>
		/// Adds A List of new neighbors to the known neighbors of the node
		/// </summary>
		/// <param name="ID">The ID of the reference node</param>
		/// <param name="newNeighbors">The List with the neighboring nodes IDs</param>
		public void addNeighbor(int ID, List<int> newNeighbors)
		{
            if (neighbors.Keys.Contains(ID))
            {
                foreach (var item in newNeighbors)
                {
                    if (!neighbors[ID].Contains(item))
                    {
                        neighbors[ID].Add(item);
                    }
                }
            }
            else
            {
                neighbors.Add(ID, newNeighbors);
            }
		}

		/// <summary>
		/// Adds A new neighbor to the known neighbors of the node
		/// </summary>
		/// <param name="ID">The ID of the reference node</param>
		/// <param name="newNeighbors">The ID of the new neighbor</param>
		public void addNeighbor(int ID, int neighborID)
		{
			if (!neighbors[ID].Contains(neighborID))
			neighbors[ID].Add(neighborID);
		}

		/// <summary>
		/// Gets A List containing the neighbors of the specified node
		/// </summary>
		/// <param name="ID">The ID of the node</param>
		/// <returns>The neighbors List</returns>
		public List<int> Neighbors(int ID)
		{
			return neighbors[ID];
		}
	}
}
