using SequenceDiagramLib.Presenter;

namespace SequenceDiagramLib.View
{
	public interface ISequenceDiagramView
	{
		void SetPresenter(ISequenceDiagramPresenter presenter);

		void Tick();

		event TickDelegate OnTick;
	}
}
