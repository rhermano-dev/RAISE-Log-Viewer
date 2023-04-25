using System.Drawing;
using System.Drawing.Drawing2D;

namespace SequenceDiagramLib.Model
{
	public class Message
	{
		public string Name = "";
		public Color Color;
		public DashStyle DashStyle;

		public Participant From = null;
		public Participant To = null;
        public string FromTo = null;

        public int Timestep = 0;

		public Message(Sequence sequence, MessageInfo messageInfo)
		{
			this.Name = messageInfo.Name;
			this.Color = messageInfo.Color;
			this.DashStyle = messageInfo.DashStyle;

			this.From = messageInfo.From;
			this.To = messageInfo.To;

			this.FromTo = messageInfo.FromTo;

			this.Timestep = sequence.Timestep;
		}
	}
}
