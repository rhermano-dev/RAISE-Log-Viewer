using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SequenceDiagramLib.Model
{
	public class ParticipantCollection : IEnumerable<Participant>
	{
		private Sequence sequence = null;

		private List<Participant> participants = new List<Participant>();
		private Dictionary<string, Participant> participantsDict = new Dictionary<string, Participant>();

		public ParticipantCollection(Sequence sequence)
		{
			this.sequence = sequence;
		}

		public int Count
		{
			get
			{
				return this.participants.Count;
			}
		}

		public Participant this[int index]
		{
			get
			{
				return this.participants[index];
			}
		}

		public Participant this[string key]
		{
			get
			{
				return this.participantsDict[key];
			}
		}

		public Participant Create(ParticipantInfo participantInfo)
		{
			Participant participant = new Participant(this.sequence, participantInfo, this.participants.Count);
			this.participants.Add(participant);
			this.participantsDict.Add(participantInfo.Name, participant);

			return participant;
		}

		public Participant Create(string name, bool underlined = false,
			Color? color = null, Color? textColor = null,
			EParticipantType? type = null, Box box = null, bool createNow = false)
		{
			ParticipantInfo participantInfo = new ParticipantInfo();
			participantInfo.Name = name;
			if (color != null) participantInfo.Color = color.Value;
			if (textColor != null) participantInfo.TextColor = textColor.Value;
			if (type != null) participantInfo.Type = type.Value;
			if (box != null) participantInfo.Box = box;
			participantInfo.CreateNow = createNow;
			return Create(participantInfo);
		}

		public Participant CreateOrGet(ParticipantInfo participantInfo)
		{
			if (!this.participantsDict.ContainsKey(participantInfo.Name))
				Create(participantInfo);

			return this.participantsDict[participantInfo.Name];
		}

		public Participant CreateOrGet(
			string name, bool underlined = false,
			Color? color = null, Color? textColor = null,
			EParticipantType? type = null, Box box = null, bool createNow = false)
		{
			if (!this.participantsDict.ContainsKey(name))
				Create(name, underlined, color, textColor, type, box, createNow);

			return this.participantsDict[name];
		}

		#region Enumerator
		public IEnumerator<Participant> GetEnumerator()
		{
			return this.participants.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		#endregion
	}
}
