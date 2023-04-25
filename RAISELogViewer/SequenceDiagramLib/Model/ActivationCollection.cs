using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SequenceDiagramLib.Model
{
	public class ActivationCollection : IEnumerable<Activation>
	{
		private Participant participant = null;

		private List<Activation> activations = new List<Activation>();
		private Dictionary<string, Activation> activationsDict = new Dictionary<string, Activation>();

		public Participant Participant
		{
			get
			{
				return this.participant;
			}
		}

		public ActivationCollection(Participant participant)
		{
			this.participant = participant;
		}

		public Activation this[string key]
		{
			get
			{
				return this.activationsDict[key];
			}
		}

		public Activation Add(ActivationInfo activationInfo, Activation parentActivation)
		{
			Activation activation = new Activation(this, activationInfo, parentActivation);
			this.activations.Add(activation);
			if (activationInfo.Id != "")
				this.activationsDict.Add(activationInfo.Id, activation);

			this.participant.Sequence.AddActivationInternal(activation);
			return activation;
		}


		public void Clear()
		{
			this.activations.Clear();
		}

		public int Count
		{
			get
			{
				return this.activations.Count;
			}
		}

		public Activation this[int index]
		{
			get
			{
				return this.activations[index];
			}
		}

		#region Enumerator
		public IEnumerator<Activation> GetEnumerator()
		{
			return this.activations.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		#endregion
	}
}
