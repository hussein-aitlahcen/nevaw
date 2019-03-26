using Ankama.Cube.Data;
using Ankama.Cube.Extensions;

namespace Ankama.Cube.UI.Debug
{
	public class DebugSelectorProperty : DebugSelector<PropertyId>
	{
		protected override PropertyId[] dataValues => EnumUtility.GetValues<PropertyId>();

		public DebugSelectorProperty()
			: base("PropertyId")
		{
		}
	}
}
