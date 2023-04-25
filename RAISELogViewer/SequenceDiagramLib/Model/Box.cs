using System;
using System.Collections.Generic;
using System.Drawing;

namespace SequenceDiagramLib.Model
{
	public class Box
	{
		public Sequence Sequence = null;

		public string Name = "";
		public Color Color;

		public int MinIndex = -1;
		public int MaxIndex = -1;

		public Box(Sequence sequence, BoxInfo info, int index)
		{
			this.Sequence = sequence;

			this.Name = info.Name;
			this.Color = info.Color;
		}

		public void AddParticipant(Participant participant)
		{
			if (this.MinIndex == -1)
				this.MinIndex = participant.Index;
			this.MaxIndex = participant.Index;
		}
	}
}
