using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using SimLib.Nodes;

namespace SimLib.SimMath
{
    class Distance
    {
		/// <summary>
		/// Gets the distance between two points
		/// </summary>
		/// <param name="A">The first point</param>
		/// <param name="B">The second point</param>
		/// <returns>The distance of the two points</returns>
        public static double Get(Point A, Point B)
        {
            return System.Math.Sqrt( System.Math.Pow(A.X - B.X, 2) + System.Math.Pow(A.Y - B.Y,2) );
        }

		/// <summary>
		/// Checks if two nodes are within range
		/// </summary>
		/// <param name="A">The first node</param>
		/// <param name="B">The second node</param>
		/// <returns>True if the two nodes are within range, false otherwise</returns>
		public static bool WithinRange(INode A, INode B)
		{
			bool ret = false;
			double minRange = System.Math.Min(A.Antenna.Range, B.Antenna.Range);
			if (Distance.Get(A.Info.Coordinates, B.Info.Coordinates) <= minRange)
				ret = true;
			return ret;
		}
    }
}
