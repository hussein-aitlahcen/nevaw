namespace Ankama.Cube.Data
{
	public abstract class DynamicValueContext
	{
		public readonly DynamicValueHolderType type;

		public readonly int level;

		protected DynamicValueContext(DynamicValueHolderType type, int level)
		{
			this.type = type;
			this.level = level;
		}
	}
}
