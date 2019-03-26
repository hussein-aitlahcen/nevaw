using Ankama.Cube.Data;
using Ankama.Cube.Extensions;
using Ankama.Cube.Maps.Objects;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Fight
{
	[UsedImplicitly]
	public static class CharacterFightMovementSequencer
	{
		public static IEnumerable<CharacterAnimationInfo> ComputeMovement(Vector2Int[] movementCells, DirectionAngle mapRotation)
		{
			int cellCount = movementCells.Length;
			if (cellCount <= 1)
			{
				yield break;
			}
			Vector2Int val = movementCells[0];
			int i = 1;
			Vector2Int cell;
			Direction directionTo;
			while (true)
			{
				if (i < cellCount)
				{
					cell = movementCells[i];
					directionTo = val.GetDirectionTo(cell);
					if (!directionTo.IsAxisAligned())
					{
						break;
					}
					yield return new CharacterAnimationInfo(Vector2Int.op_Implicit(cell), "run", "run", loops: true, directionTo, mapRotation);
					val = cell;
					int num = i + 1;
					i = num;
					continue;
				}
				yield break;
			}
			throw new Exception($"[ExecuteMovementToAttack] Invalid direction {directionTo} going from {val} to {cell}");
		}

		public static IEnumerable<CharacterAnimationInfo> ComputeMovementToAction(Vector2Int[] movementCells, Direction actionDirection, DirectionAngle mapRotation)
		{
			int num = movementCells.Length;
			if (num <= 1)
			{
				yield break;
			}
			Direction[] directions = new Direction[num - 1];
			Vector2Int startCell = movementCells[0];
			Vector2Int val = startCell;
			for (int j = 1; j < num; j++)
			{
				Vector2Int val2 = movementCells[j];
				Direction directionTo = val.GetDirectionTo(val2);
				if (!directionTo.IsAxisAligned())
				{
					throw new Exception(string.Format("[{0}] Invalid direction {1} going from {2} to {3}", "ComputeMovementToAction", directionTo, val, val2));
				}
				directions[j - 1] = directionTo;
				val = val2;
			}
			int consecutiveStraight = 0;
			int lastCellIndex = num - 1;
			Direction currentDirection = directions[0];
			yield return new CharacterAnimationInfo(Vector2Int.op_Implicit(startCell), "dashantic", "dashantic", loops: false, directions[0], mapRotation);
			int num2;
			for (int i = 1; i < lastCellIndex; i = num2)
			{
				Direction nextDirection = directions[i];
				if (nextDirection != currentDirection)
				{
					Vector2Int cell = movementCells[i];
					if (consecutiveStraight > 0)
					{
						Vector2Int val3 = (i == consecutiveStraight) ? startCell : movementCells[i - 1 - consecutiveStraight];
						Vector2 position = Vector2Int.op_Implicit(val3) + 0.5f * Vector2Int.op_Implicit(cell - val3);
						yield return new CharacterAnimationInfo(position, "dash", "dash", loops: false, currentDirection, mapRotation);
					}
					if (consecutiveStraight > 1)
					{
						yield return new CharacterAnimationInfo(Vector2Int.op_Implicit(cell), "dashturn", "dashturn", loops: false, currentDirection, nextDirection, mapRotation);
					}
					else
					{
						yield return new CharacterAnimationInfo(Vector2Int.op_Implicit(cell), "dashzig", "dashzig", loops: false, currentDirection, nextDirection, mapRotation);
					}
					consecutiveStraight = 0;
				}
				else
				{
					num2 = consecutiveStraight + 1;
					consecutiveStraight = num2;
				}
				currentDirection = nextDirection;
				num2 = i + 1;
			}
			if (consecutiveStraight > 0)
			{
				Vector2Int cell = movementCells[lastCellIndex];
				Vector2Int val4 = (lastCellIndex == consecutiveStraight) ? startCell : movementCells[lastCellIndex - 1 - consecutiveStraight];
				Vector2 position2 = Vector2Int.op_Implicit(val4) + 0.5f * Vector2Int.op_Implicit(cell - val4);
				yield return new CharacterAnimationInfo(position2, "dash", "dash", loops: false, currentDirection, mapRotation);
				if (!actionDirection.IsAxisAligned())
				{
					actionDirection = actionDirection.GetAxisAligned(currentDirection);
				}
				if (actionDirection != currentDirection)
				{
					yield return new CharacterAnimationInfo(Vector2Int.op_Implicit(cell), "dashturn", "dashturn", loops: false, currentDirection, actionDirection, mapRotation);
				}
			}
		}
	}
}
