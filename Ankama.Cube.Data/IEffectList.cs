using DataEditor;

namespace Ankama.Cube.Data
{
	public interface IEffectList : IEditableContent
	{
		PrecomputedData precomputedData
		{
			get;
		}
	}
}
