using System.Drawing;

namespace SequenceDiagramLib.Model
{
	public class ParticipantInfo
	{
		public string Name = "";
		public Color Color = Color.LightYellow;
		public Color TextColor = Color.Black;
		public EParticipantType Type = EParticipantType.Box;
		public Box Box = null;
		public bool CreateNow = false;
	}
}
