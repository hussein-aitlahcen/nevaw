using System;

namespace Ankama.Cube.Data
{
	[Serializable]
	public enum FightStatType
	{
		Mvp,
		TotalDamageDealt,
		TurnDamageDealt,
		TotalDamageSustained,
		TurnDamageSustained,
		TotalKills,
		CompanionGiven,
		SpellCasts,
		APUsed,
		MPUsed,
		PlayTime,
		BudgetPointsWon,
		HeroKillsSuffered,
		BudgetPointsDiff
	}
}
