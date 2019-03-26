using DataEditor;

namespace Ankama.Cube.Data
{
	public interface ISingleCaracIdSelector : ICaracIdSelector, IEditableContent
	{
		CaracId GetCaracId();
	}
}
