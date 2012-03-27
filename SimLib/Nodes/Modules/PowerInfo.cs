using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimLib.Nodes
{
    public class PowerInfo
    {
        /// <summary>
        /// Gets/Sets the Node energy
        /// </summary>
        public double Energy { get; set; }

        /// <summary>
        /// Gets/Sets the Energy required for A single transmission
        /// </summary>
        private double _Energy_Send { get; set; }

        /// <summary>
        /// Gets/Sets the Energy required for A single item receipt
        /// </summary>
        private double _Energy_Receive { get; set; }

        /// <summary>
        /// Holds the Power information of A node
        /// </summary>
        public PowerInfo()
        {
            Energy = Properties.Simulation.Default.Energy;
            _Energy_Send = Properties.Simulation.Default.Energy_To_Send;
            _Energy_Receive = Properties.Simulation.Default.Energy_To_Receive;
        }

        /// <summary>
        /// Returns the REMAINING energy after the receipt
        /// </summary>
        /// <returns>The remaining energy of the node</returns>
        public double receive()
        {
            Energy -= _Energy_Receive;
            return Energy;
        }

        /// <summary>
        /// Returns the REMAINING energy after the item transmission
        /// </summary>
        /// <returns>The remaining energy of the node</returns>
        public double send()
        {
            Energy -= _Energy_Send;
            return Energy;
        }
    }
}
