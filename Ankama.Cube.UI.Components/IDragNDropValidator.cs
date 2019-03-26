namespace Ankama.Cube.UI.Components
{
	public interface IDragNDropValidator
	{
		bool IsValidDrag(object value);

		bool IsValidDrop(object value);
	}
}
