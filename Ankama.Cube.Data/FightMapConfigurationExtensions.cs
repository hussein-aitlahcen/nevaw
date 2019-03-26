using System;

namespace Ankama.Cube.Data
{
	public static class FightMapConfigurationExtensions
	{
		public static int GetRegionCount(this FightMapConfiguration value)
		{
			switch (value)
			{
			case FightMapConfiguration.PVP:
				return 1;
			case FightMapConfiguration.PVM:
				return 1;
			case FightMapConfiguration.BossFight:
				return 4;
			case FightMapConfiguration.PVP3V3:
				return 3;
			case FightMapConfiguration.PVP2V2:
				return 1;
			default:
				throw new ArgumentOutOfRangeException("value", value, null);
			}
		}

		public static int GetPlayerCountPerRegion(this FightMapConfiguration value)
		{
			switch (value)
			{
			case FightMapConfiguration.PVP:
				return 2;
			case FightMapConfiguration.PVM:
				return 1;
			case FightMapConfiguration.BossFight:
				return 1;
			case FightMapConfiguration.PVP3V3:
				return 2;
			case FightMapConfiguration.PVP2V2:
				return 4;
			default:
				throw new ArgumentOutOfRangeException("value", value, null);
			}
		}
	}
}
