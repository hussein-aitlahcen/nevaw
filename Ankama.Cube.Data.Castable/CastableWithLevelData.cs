using DataEditor;

namespace Ankama.Cube.Data.Castable
{
	public class CastableWithLevelData<T> where T : EditableData, IDefinitionWithTooltip
	{
		public readonly T definition;

		public int level
		{
			get;
		}

		protected CastableWithLevelData(T definition, int level)
		{
			this.definition = definition;
			this.level = level;
		}
	}
}
