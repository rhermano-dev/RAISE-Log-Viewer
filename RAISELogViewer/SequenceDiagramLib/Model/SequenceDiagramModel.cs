using SequenceDiagramLib.Presenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceDiagramLib.Model
{
	public class SequenceDiagramModel : ISequenceDiagramModel
	{
		public Sequence sequence = null;

		public SequenceDiagramModel(ISequenceDiagramPresenter presenter)
		{
			this.sequence = new Sequence(presenter);
		}

		public Sequence GetSequence()
		{
			return this.sequence;
		}
	}
}
