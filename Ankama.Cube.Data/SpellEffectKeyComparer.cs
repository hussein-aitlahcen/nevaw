using System.Collections.Generic;

namespace Ankama.Cube.Data
{
	public class SpellEffectKeyComparer : IEqualityComparer<SpellEffectKey>
	{
		public static readonly SpellEffectKeyComparer instance;

		static SpellEffectKeyComparer()
		{
			instance = new SpellEffectKeyComparer();
		}

		public bool Equals(SpellEffectKey x, SpellEffectKey y)
		{
			return x == y;
		}

		public int GetHashCode(SpellEffectKey obj)
		{
			return (int)obj;
		}
	}
}
