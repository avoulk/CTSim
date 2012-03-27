using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimLib.Messages;
using CGTF.Sim.Messaging.Types;

namespace CGTF.Sim.Messaging
{
	public class MSGReportCluster : Message
	{
		public int RID { get; set; }
		public WSNode OriginalNode { get; set; }

		public MSGReportCluster(int Source, int Target, WSNode OriginalNode, int RID)
			: base(Source, Target, CustomMessageType.MSG_REPORT_CLUSTER, 0)
		{
			this.RID = RID;
			this.OriginalNode = OriginalNode;
		}
	}
}
