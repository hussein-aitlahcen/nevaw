using Ankama.Cube.Data;
using Ankama.Cube.Fight.Entities;
using JetBrains.Annotations;
using System.Collections.Generic;

namespace Ankama.Cube
{
	public static class RuntimeDataHelper
	{
		[CanBeNull]
		public static ReserveDefinition GetReserveDefinitionFrom([NotNull] PlayerStatus playerStatus)
		{
			HeroStatus heroStatus = playerStatus.heroStatus;
			if (heroStatus == null)
			{
				return null;
			}
			Dictionary<God, ReserveDefinition> reserveDefinitions = RuntimeData.reserveDefinitions;
			God god = ((WeaponDefinition)heroStatus.definition).god;
			if (reserveDefinitions.TryGetValue(god, out ReserveDefinition value))
			{
				return value;
			}
			return null;
		}
	}
}
