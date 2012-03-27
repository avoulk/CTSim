using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SimLib.Nodes
{
    public class NodeInfo
    {
        /// <summary>
        /// Gets/Sets the node ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets/Sets the node coordinates
        /// </summary>
        public Point Coordinates { get; set; }

        /// <summary>
        /// Info for A node
        /// </summary>
        /// <param name="ID">The ID of the node</param>
        /// <param name="Coordinates">The coordinates of the node</param>
        public NodeInfo(int id, Point Coordinates)
        {
            ID = id;
            this.Coordinates = Coordinates;
        }
    }
}
