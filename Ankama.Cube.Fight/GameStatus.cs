using Ankama.Cube.Data;
using Ankama.Cube.Fight.Entities;
using JetBrains.Annotations;
using System;

namespace Ankama.Cube.Fight
{
	public static class GameStatus
	{
		public static FightType fightType;

		public static FightDefinition fightDefinition;

		public static FightStatus[] fights;

		public static bool hasEnded;

		public static int localPlayerTeamIndex = -1;

		public static int allyTeamPoints;

		public static int opponentTeamPoints;

		public static bool isPvP
		{
			get
			{
				switch (fightType)
				{
				case FightType.None:
				case FightType.Versus:
				case FightType.TeamVersus:
					return true;
				case FightType.BossFight:
					return false;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		public static void Initialize(FightType type, FightDefinition definition, FightStatus[] fightStatusArray)
		{
			fightType = type;
			fightDefinition = definition;
			fights = fightStatusArray;
			hasEnded = false;
			localPlayerTeamIndex = -1;
			allyTeamPoints = 0;
			opponentTeamPoints = 0;
		}

		public static FightStatus GetFightStatus(int index)
		{
			return fights[index];
		}

		public static DirectionAngle GetMapRotation([NotNull] PlayerStatus localPlayerStatus)
		{
			switch (fightType)
			{
			case FightType.None:
				return DirectionAngle.None;
			case FightType.Versus:
				if (localPlayerStatus.index != 0)
				{
					return DirectionAngle.Clockwise180;
				}
				return DirectionAngle.None;
			case FightType.BossFight:
				switch (FightStatus.local.fightId)
				{
				case 0:
					return DirectionAngle.None;
				case 1:
					return DirectionAngle.Clockwise90;
				case 2:
					return DirectionAngle.Clockwise180;
				case 3:
					return DirectionAngle.CounterClockwise90;
				default:
					throw new ArgumentOutOfRangeException();
				}
			case FightType.TeamVersus:
				if (localPlayerStatus.teamIndex != 0)
				{
					return DirectionAngle.Clockwise180;
				}
				return DirectionAngle.None;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
	}
}
