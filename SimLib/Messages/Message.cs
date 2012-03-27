using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimLib.Messages
{
	public class Message : IMessage
	{

		public Envelop Envelop { get; set; }

		/// <summary>
		/// Gets/Sets the ID of the transmitting node
		/// </summary>
		public int Source
		{
			get
			{
				return Envelop.Source;
			}

			set
			{
				Envelop.Source = value;
			}
		}

		/// <summary>
		/// Gets/Sets the ID of the target(s) node(s)
		/// </summary>
		public int Target
		{
			get
			{
				return this.Envelop.Target;
			}
			set
			{
				this.Envelop.Target = value;
			}
		}

		/// <summary>
		/// Gets/Sets the type of the item
		/// </summary>
		public int Type
		{
			get
			{
				return this.Envelop.Type;
			}
			set
			{
				this.Envelop.Type = value;
			}
		}

		/// <summary>
		/// Gets/Sets the data delivered with the item
		/// </summary>
		public double Data
		{
			get
			{
				return this.Envelop.Data;
			}
			set
			{
				this.Envelop.Data = value;
			}
		}

		/// <summary>
		/// Represents A generic item
		/// </summary>
		/// <param name="Source">The sender node ID</param>
		/// <param name="Target">The receiver node ID</param>
		/// <param name="Type">The type of the item</param>
		/// <param name="DrawData">The dta carried by the item</param>
		public Message(int Source, int Target, int Type, double Data)
		{
			Envelop = new Envelop(Source, Target, Type, Data);
		}

	}
}
