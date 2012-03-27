using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimLib.Messages;
using CGTF.Sim.Messaging.Types;

namespace CGTF.Sim.Messaging
{
	class MSGNeighboringClusterQuery : Message
	{
		public int CID { get; set; }
		public bool Reply { get; set; }

		public MSGNeighboringClusterQuery(int Source, int Target, int CID, bool Reply)
			: base(Source, Target, CustomMessageType.MSG_NEIGHBORING_CLUSER_QUERY, 0)
		{
			this.CID = CID;
			this.Reply = Reply;
		}
	}
}
