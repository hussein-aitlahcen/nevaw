using System.Collections;

namespace Ankama.Cube.Maps.Objects
{
	public interface IBossSpell
	{
		IEnumerator PlaySpellAnim(int spellId);
	}
}
