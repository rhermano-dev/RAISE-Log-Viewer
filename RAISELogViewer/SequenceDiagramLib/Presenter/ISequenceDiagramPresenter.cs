using SequenceDiagramLib.Model;

namespace SequenceDiagramLib.Presenter
{
	public delegate void TickDelegate();

	public interface ISequenceDiagramPresenter
	{
		ISequenceDiagramModel GetModel();

		void Tick();

		event TickDelegate OnTick;
	}
}
