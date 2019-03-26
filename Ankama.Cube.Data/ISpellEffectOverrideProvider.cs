namespace Ankama.Cube.Data
{
	public interface ISpellEffectOverrideProvider
	{
		bool TryGetSpellEffectOverride(SpellEffectKey key, out SpellEffect spellEffect);
	}
}
