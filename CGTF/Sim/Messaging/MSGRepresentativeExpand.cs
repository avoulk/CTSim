using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimLib.Messages;
using SimLib.Messages.Types;
using CGTF.Sim.Messaging.Types;

namespace CGTF.Sim.Messaging
{
	class MSGRepresentativeExpand : Message
	{
		public int RID { get; set; }
		public int HC { get; set; }

		public MSGRepresentativeExpand(int Source, int RID, int HC)
			: base(Source, MessageTargets.ALL_IN_RANGE, CustomMessageType.MSG_REPRESENTATIVE_EXPAND, 0)
		{
			this.RID = RID;
			this.HC = HC;
		}
	}
}
