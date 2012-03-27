using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimLib.Messages.Types
{
    public class MSGDataExchange : Message
    {
        public MSGDataExchange(int Source, int Target, double Data)
            : base(Source, Target, MessageType.DATA_EXCHANGE, Data)
        {
        }
    }
}
