namespace Ankama.Cube.Data
{
	public sealed class SpellDefinitionContext : DynamicValueContext
	{
		public SpellDefinitionContext(SpellDefinition definition, int level)
			: base(DynamicValueHolderType.Spell, level)
		{
		}
	}
}
