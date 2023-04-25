using System;
using System.Collections.Generic;
using System.Drawing;

namespace SequenceDiagramLib.Model
{
	public class Participant
	{
		private Sequence sequence = null;

		public List<int> DestructionTimesteps = new List<int>();

		public ActivationCollection Activations = null;
		public Activation ActiveActivation = null;

		public string Name = "";
		public bool Underlined = false;
		public Color Color;
		public Color TextColor;

		public EParticipantType Type;
		private Box box = null;
		public int CreationTimestep = 0;

		public int Index = 0;
		public bool Break = true;

		public Participant(Sequence sequence, ParticipantInfo info, int index)
		{
			this.sequence = sequence;
			this.Activations = new ActivationCollection(this);

			this.Name = info.Name;
			this.Color = info.Color;
			this.TextColor = info.TextColor;
			this.Type = info.Type;

			if (info.CreateNow)
				this.CreationTimestep = sequence.Timestep;

			this.Index = index;

			if (info.Box != null)
			{
				if (info.Box.MaxIndex > -1)
				{
					foreach (Participant participant in sequence.Participants)
					{
						if (participant.Index > info.Box.MaxIndex)
							participant.Index++;
					}

					this.Index = info.Box.MaxIndex + 1;
				}

				this.box = info.Box;
				this.box.AddParticipant(this);
			}
		}

		public Sequence Sequence
		{
			get
			{
				return this.sequence;
			}
		}

		public Box Box
		{
			get
			{
				return this.box;
			}
		}

		public Activation Activate(ActivationInfo activationInfo)
		{
			Activation activation = this.Activations.Add(activationInfo, this.ActiveActivation);
			this.ActiveActivation = activation;

			this.sequence.Changed(this);

			return activation;
		}

		public Activation Activate(string name = null, Color? color = null)
		{
			ActivationInfo activationInfo = new ActivationInfo();
			if (name != null) activationInfo.Name = name;
			if (color != null) activationInfo.Color = color.Value;

			return Activate(activationInfo);
		}

		public void Deactivate()
		{
			if (this.ActiveActivation == null)
				throw new Exception("No active activation.");

			this.ActiveActivation.Deactivate();
			this.ActiveActivation = this.ActiveActivation.Parent;
		}

		public void Destroy()
		{
			this.DestructionTimesteps.Add(this.sequence.Timestep);
		}

		public Dictionary<int, int> BeginDepth = new Dictionary<int, int>();
		public Dictionary<int, int> EndDepth = new Dictionary<int, int>();

		public void Tick(int timestep)
		{
			int depth = 0;
			if (this.ActiveActivation != null)
				depth = this.ActiveActivation.Index + 1;

			this.EndDepth.Add(timestep, depth);
			this.BeginDepth.Add(timestep + 1, depth);
		}
	}
}
