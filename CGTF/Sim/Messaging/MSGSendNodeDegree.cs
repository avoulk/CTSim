using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimLib.Messages;
using CGTF.Sim.Messaging.Types;

namespace CGTF.Sim.Messaging
{
	class MSGSendNodeDegree : Message
	{
		public bool Reply { get; set; }

		public MSGSendNodeDegree(int Source, int Target, double Data, bool isReply)
			: base(Source, Target, CustomMessageType.MSG_SEND_NODE_DEGREE, Data)
		{
			this.Reply = isReply;
		}
	}
}
