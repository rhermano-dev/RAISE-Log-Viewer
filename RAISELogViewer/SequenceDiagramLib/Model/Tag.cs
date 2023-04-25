using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDiagramLib.Model
{
	public class Tag
	{
		public int TimeStep = 0;
		public string Key = "";
		public string Value = "";

		public Tag(int timeStep, string key, string value)
		{
			this.TimeStep = timeStep;
			this.Key = key;
			this.Value = value;
		}
	}
}
