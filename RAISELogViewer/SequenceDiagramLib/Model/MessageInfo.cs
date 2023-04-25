using System.Drawing;
using System.Drawing.Drawing2D;

namespace SequenceDiagramLib.Model
{
	public class MessageInfo
	{
		public string Name = "";

		public Participant From = null;
		public Participant To = null;

        public string FromTo = null;

        public Color Color = Color.Black;
		public EArrowHead ArrowHead = EArrowHead.Open;
		public DashStyle DashStyle = DashStyle.Solid;
	}
}
