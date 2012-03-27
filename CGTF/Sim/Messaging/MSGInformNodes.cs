using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CGTF.Sim.Messaging.Types;

namespace CGTF.Sim.Messaging
{
	class MSGInformNodes : SimLib.Messages.Message
	{
		public Dictionary<int, int> CS { get; set; }
		public int RID { get; set; }

		public MSGInformNodes(int Source, int RID, Dictionary<int, int> CS)
			: base(Source, SimLib.Messages.Types.MessageTargets.ALL_IN_RANGE, CustomMessageType.MSG_INFORM_NODES, 0)
		{
			this.CS = CS;
			this.RID = RID;
		}
	}
}
