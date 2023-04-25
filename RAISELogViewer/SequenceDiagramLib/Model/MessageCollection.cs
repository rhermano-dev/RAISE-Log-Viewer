using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace SequenceDiagramLib.Model
{
	public class MessageCollection : IEnumerable<Message>
	{
		private Sequence sequence = null;

		private List<Message> messages = new List<Message>();

		public MessageCollection(Sequence sequence)
		{
			this.sequence = sequence;
		}

		public void Clear()
		{
			this.messages.Clear();
		}

		public Message Add(string name, Participant from, Participant to, string FromTo,
			Color? color = null, EArrowHead? arrowHead = null, DashStyle? dashStyle = null)
		{
			MessageInfo messageInfo = new MessageInfo();
			messageInfo.Name = name;
			messageInfo.From = from;
			messageInfo.To = to;
			messageInfo.FromTo = FromTo;
			if (color != null) messageInfo.Color = color.Value;
			if (arrowHead != null) messageInfo.ArrowHead = arrowHead.Value;
			if (dashStyle != null) messageInfo.DashStyle = dashStyle.Value;
			return Add(messageInfo);
		}

		public Message Add(string name, Participant self,
			Color? color = null, EArrowHead? arrowHead = null, DashStyle ? dashStyle = null)
		{
			MessageInfo messageInfo = new MessageInfo();
			messageInfo.Name = name;
			messageInfo.From = self;
			messageInfo.To = self;
			if (color != null) messageInfo.Color = color.Value;
			if (arrowHead != null) messageInfo.ArrowHead = arrowHead.Value;
			if (dashStyle != null) messageInfo.DashStyle = dashStyle.Value;
			return Add(messageInfo);
		}

		public Message Add(MessageInfo messageInfo)
		{
			Message message = new Message(this.sequence, messageInfo);
			this.messages.Add(message);

			this.sequence.Changed(messageInfo.From);
			this.sequence.Changed(messageInfo.To);

			return message;
		}

		#region
		public IEnumerator<Message> GetEnumerator()
		{
			return this.messages.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		#endregion
	}
}
