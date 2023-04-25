using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDiagramLib.Model
{
	public class Log
	{
		public int TimeStep = 0;
		public string Event = "";

		public Log(int timeStep, string event_)
		{
			this.TimeStep = timeStep;
			this.Event = event_;
		}
	}
}
