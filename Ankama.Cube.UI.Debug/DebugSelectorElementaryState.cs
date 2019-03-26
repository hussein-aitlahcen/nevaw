using Ankama.Cube.Data;
using Ankama.Cube.Extensions;

namespace Ankama.Cube.UI.Debug
{
	public class DebugSelectorElementaryState : DebugSelector<ElementaryStates>
	{
		protected override ElementaryStates[] dataValues => EnumUtility.GetValues<ElementaryStates>();

		public DebugSelectorElementaryState()
			: base("ElementaryStates")
		{
		}
	}
}
