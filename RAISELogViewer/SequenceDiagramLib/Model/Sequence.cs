using SequenceDiagramLib.Presenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDiagramLib.Model
{
	public class Sequence
	{
		public ISequenceDiagramPresenter Presenter = null;

		private int timestep = 0;

		public BoxCollection Boxes = null;
		public ParticipantCollection Participants = null;
		public MessageCollection Messages = null;

		public List<DateTime> Timesteps = null;
        public List<String> TimeLine = null;

        public readonly object Lock = new object();

		public Sequence(ISequenceDiagramPresenter presenter)
		{
			this.Presenter = presenter;

			Clear();
		}

		public void Clear()
		{
			this.timestep = 0;

			this.Boxes = new BoxCollection(this);
			this.Participants = new ParticipantCollection(this);
			this.Messages = new MessageCollection(this);

			this.activations = new Dictionary<string, Activation>();

			this.Timesteps = new List<DateTime>();
            this.TimeLine = new List<String>();
        }

		public int Timestep
		{
			get
			{
				return this.timestep;
			}
		}

		public void Changed(Participant participant)
		{
			if (participant.Break)
				this.break_ = true;
		}

		public void Tick(string time)
		{
			foreach (Participant participant in this.Participants)
				participant.Tick(this.timestep);

            //this.Timesteps.Add(DateTime.Now);
            this.TimeLine.Add(time);
            this.timestep++;

			this.Presenter.Tick();

			//Break();
			//this.break_ = false;
		}

		#region Break functionality
		public bool BreakEnabled = true;
		private bool break_ = false;

		public delegate void EnterBreakDelegate();
		public delegate void ExitBreakDelegate();

		public event EnterBreakDelegate OnEnterBreak;
		public event ExitBreakDelegate OnExitBreak;

		private volatile bool wait = false;
		private volatile bool exit = false;

		private void Break()
		{
			if ((this.BreakEnabled) && (this.break_))
			{
				if (this.OnEnterBreak != null)
					this.OnEnterBreak();

				ResponsiveWait();

				if (this.OnExitBreak != null)
					this.OnExitBreak();
			}

		}

		public void Continue()
		{
			this.wait = false;
		}

		public void Exit()
		{
			this.exit = true;
		}

		private void ResponsiveWait()
		{
			this.wait = true;

			for (;;)
			{
				if (!this.wait)
					break;

				if (this.exit)
					break;

				System.Windows.Forms.Application.DoEvents();
				System.Threading.Thread.Sleep(200);
			}
		}
		#endregion

		/*
		private void ResponsiveSleep(int millisecondsTimeout)
		{
			if (millisecondsTimeout < 1) return;

			DateTime desired = DateTime.Now.AddMilliseconds(millisecondsTimeout);
			while (DateTime.Now < desired)
			{
				Thread.Sleep(200);
				Application.DoEvents();
			}
		}
		*/

		#region A collection of all activations of the sequence
		private Dictionary<string, Activation> activations = null;

		internal Activation AddActivationInternal(Activation activation)
		{
			this.activations.Add(activation.Id, activation);
			return activation;
		}

		public Activation GetActivation(string id)
		{
			return this.activations[id];
		}
		#endregion
	}
}
