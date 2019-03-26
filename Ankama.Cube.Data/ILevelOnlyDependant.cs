using DataEditor;

namespace Ankama.Cube.Data
{
	public interface ILevelOnlyDependant : IEditableContent, IReferenceableContent
	{
		int GetValueWithLevel(int level);
	}
}
