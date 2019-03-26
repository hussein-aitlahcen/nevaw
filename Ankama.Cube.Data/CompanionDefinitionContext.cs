using JetBrains.Annotations;

namespace Ankama.Cube.Data
{
	public sealed class CompanionDefinitionContext : DynamicValueContext
	{
		public CompanionDefinitionContext([NotNull] CompanionDefinition definition, int level)
			: base(DynamicValueHolderType.Companion, level)
		{
		}
	}
}
