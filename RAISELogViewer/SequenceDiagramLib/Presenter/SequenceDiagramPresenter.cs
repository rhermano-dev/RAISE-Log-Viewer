using SequenceDiagramLib.Model;
using SequenceDiagramLib.View;

namespace SequenceDiagramLib.Presenter
{
	public class SequenceDiagramPresenter : ISequenceDiagramPresenter
	{
		public ISequenceDiagramView View = null;
		private ISequenceDiagramModel model = null;

		public event TickDelegate OnTick;

		public SequenceDiagramPresenter(ISequenceDiagramView view)
		{
			this.model = new SequenceDiagramModel(this);
			this.View = view;
			this.View.SetPresenter(this);
		}

		public ISequenceDiagramModel GetModel()
		{
			return this.model;
		}

		public void Tick()
		{
			this.View.Tick();

			if (this.OnTick != null)
				this.OnTick();
		}
	}
}
