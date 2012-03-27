using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CGTF.Sim.Messaging.Types
{
    public class CustomMessageType
    {
        public const int MSG_SEND_NODE_DEGREE = -4;
        public const int MSG_CLUSTER_QUERY = -6;
        public const int MSG_INFORM_NODES = -7;
        public const int MSG_NEIGHBORING_CLUSER_QUERY = -8;
        public const int MSG_REPORT_CLUSTER = -9;
        public const int MSG_REPRESENTATIVE_EXPAND = -10;
    }
}
