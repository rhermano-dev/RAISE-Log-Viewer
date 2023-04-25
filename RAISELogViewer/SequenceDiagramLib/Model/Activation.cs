using System;
using System.Collections.Generic;
using System.Drawing;

namespace SequenceDiagramLib.Model
{
	public class Activation
	{
		private Participant participant = null;

		public Activation Parent = null;

		public string Id = "";
		public string Name = "";
		public Color Color;

		public int BeginTimestep = 0;
		public int EndTimestep = 0;

		public List<Tag> Tags = new List<Tag>();

		public int Index = 0;
		public bool Closed = false;

		public Activation(ActivationCollection coll, ActivationInfo activationInfo, Activation parentActivation)
		{
			this.participant = coll.Participant;

			if (activationInfo.Id == "")
				this.Id = Guid.NewGuid().ToString();
			else
				this.Id = activationInfo.Id;
			this.Name = activationInfo.Name;
			this.Color = activationInfo.Color;

			this.BeginTimestep = this.participant.Sequence.Timestep;

			this.Parent = parentActivation;
			if ((parentActivation != null) && (parentActivation.Participant == this.Participant))
				this.Index = this.Parent.Index + 1;
		}

		public Participant Participant
		{
			get
			{
				return this.participant;
			}
		}


		internal void Deactivate()
		{
			this.Closed =  true;
			this.EndTimestep = this.participant.Sequence.Timestep;
		}
	}
}
