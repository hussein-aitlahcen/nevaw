using Ankama.Cube.Data;
using System.Linq;

namespace Ankama.Cube.UI.Debug
{
	public class DebugDropperSpell : DebugDropperDataDefinition<SpellDefinition>
	{
		private const string DisplayName = "Spell";

		private const string SearchPrefKey = "DebugSpellDropperSearch";

		protected override SpellDefinition[] dataValues => RuntimeData.spellDefinitions.Values.ToArray();

		protected override void Awake()
		{
			base.Awake();
			Initialize("Spell", "DebugSpellDropperSearch");
		}
	}
}
