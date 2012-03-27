using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimLib.Messages
{
    public interface IMessage
    {
		Envelop Envelop { get; set; }
    }
}
