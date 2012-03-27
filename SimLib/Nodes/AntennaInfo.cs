using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimLib.Nodes
{
    public class AntennaInfo
    {
        public double Range { get; set; }

        public AntennaInfo()
        {
            Range = Properties.Settings.Default.Range;
        }
    }
}
