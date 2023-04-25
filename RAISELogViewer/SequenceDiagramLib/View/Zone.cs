using SequenceDiagramLib.Model;
using System.Drawing;

namespace SequenceDiagramLib.View
{
	public class Zone
	{
		public Zone(Rectangle rectangle, string description, string fromTo)
		{
			this.Location = rectangle;
			this.Description = description;
			this.FromTo = fromTo;
		}

		public Rectangle Location = new Rectangle();
		public string Description = "";
        public string FromTo = "";

        public Activation ActivationBox = null;
		public Participant Participant = null;
	}
}
