using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CGTF.Sim.Messaging.Types;

namespace CGTF.Sim.Messaging
{
	public class MSGClusterQuery : SimLib.Messages.Message
	{
		public int CID { get; set; }
		public int RID { get; set; }
		public int HC { get; set; }
		public double DEG { get; set; }

		public MSGClusterQuery(int Source, int Target, double Data, int RID, int CID, int HC, double DEG)
			: base(Source, Target, CustomMessageType.MSG_CLUSTER_QUERY, Data)
		{
			this.RID = RID;
			this.CID = CID;
			this.HC = HC;
			this.DEG = DEG;
		}
	}
}
