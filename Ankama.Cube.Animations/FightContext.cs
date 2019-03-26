using UnityEngine;

namespace Ankama.Cube.Animations
{
	public sealed class FightContext : ScriptableObject
	{
		public int fightId
		{
			get;
			private set;
		}

		public static FightContext Create(int fightId)
		{
			FightContext fightContext = ScriptableObject.CreateInstance<FightContext>();
			fightContext.fightId = fightId;
			return fightContext;
		}

		public FightContext()
			: this()
		{
		}
	}
}
