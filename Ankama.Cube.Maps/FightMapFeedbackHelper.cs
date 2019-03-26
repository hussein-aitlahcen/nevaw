using Ankama.Cube.Maps.Feedbacks;
using JetBrains.Annotations;
using System;
using UnityEngine;

namespace Ankama.Cube.Maps
{
	public static class FightMapFeedbackHelper
	{
		public static void SetupMovementAreaHighlight([NotNull] FightMapFeedbackResources resources, [NotNull] FightMapMovementContext context, Vector2Int coords, [NotNull] CellHighlight highlight, Color color)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0527: Unknown result type (might be due to invalid IL or missing references)
			//IL_0557: Unknown result type (might be due to invalid IL or missing references)
			//IL_056a: Unknown result type (might be due to invalid IL or missing references)
			if ((context.GetCell(coords).state & (FightMapMovementContext.CellState.Reachable | FightMapMovementContext.CellState.Occupied)) != FightMapMovementContext.CellState.Reachable)
			{
				highlight.ClearSprite();
				return;
			}
			IMapStateProvider stateProvider = context.stateProvider;
			Vector2Int sizeMin = stateProvider.sizeMin;
			Vector2Int sizeMax = stateProvider.sizeMax;
			FightMapMovementContext.Cell[] grid = context.grid;
			Sprite[] areaFeedbackSprites = resources.areaFeedbackSprites;
			Vector2Int val = default(Vector2Int);
			val._002Ector(coords.get_x(), coords.get_y() + 1);
			Vector2Int val2 = default(Vector2Int);
			val2._002Ector(coords.get_x() - 1, coords.get_y());
			Vector2Int val3 = default(Vector2Int);
			val3._002Ector(coords.get_x() + 1, coords.get_y());
			Vector2Int val4 = default(Vector2Int);
			val4._002Ector(coords.get_x(), coords.get_y() - 1);
			FightMapMovementContext.CellState cellState = (val.get_x() >= sizeMin.get_x() && val.get_x() < sizeMax.get_x() && val.get_y() >= sizeMin.get_y() && val.get_y() < sizeMax.get_y()) ? grid[stateProvider.GetCellIndex(val.get_x(), val.get_y())].state : FightMapMovementContext.CellState.None;
			FightMapMovementContext.CellState cellState2 = (val2.get_x() >= sizeMin.get_x() && val2.get_x() < sizeMax.get_x() && val2.get_y() >= sizeMin.get_y() && val2.get_y() < sizeMax.get_y()) ? grid[stateProvider.GetCellIndex(val2.get_x(), val2.get_y())].state : FightMapMovementContext.CellState.None;
			FightMapMovementContext.CellState cellState3 = (val3.get_x() >= sizeMin.get_x() && val3.get_x() < sizeMax.get_x() && val3.get_y() >= sizeMin.get_y() && val3.get_y() < sizeMax.get_y()) ? grid[stateProvider.GetCellIndex(val3.get_x(), val3.get_y())].state : FightMapMovementContext.CellState.None;
			FightMapMovementContext.CellState num = (val4.get_x() >= sizeMin.get_x() && val4.get_x() < sizeMax.get_x() && val4.get_y() >= sizeMin.get_y() && val4.get_y() < sizeMax.get_y()) ? grid[stateProvider.GetCellIndex(val4.get_x(), val4.get_y())].state : FightMapMovementContext.CellState.None;
			int num2 = ((cellState & (FightMapMovementContext.CellState.Reachable | FightMapMovementContext.CellState.Occupied)) == FightMapMovementContext.CellState.Reachable) ? 1 : 0;
			int num3 = ((cellState2 & (FightMapMovementContext.CellState.Reachable | FightMapMovementContext.CellState.Occupied)) == FightMapMovementContext.CellState.Reachable) ? 1 : 0;
			int num4 = ((cellState3 & (FightMapMovementContext.CellState.Reachable | FightMapMovementContext.CellState.Occupied)) == FightMapMovementContext.CellState.Reachable) ? 1 : 0;
			int num5 = ((num & (FightMapMovementContext.CellState.Reachable | FightMapMovementContext.CellState.Occupied)) == FightMapMovementContext.CellState.Reachable) ? 1 : 0;
			switch (4 - (num2 + num3 + num4 + num5))
			{
			case 0:
			case 1:
			case 2:
			{
				Vector2Int val5 = default(Vector2Int);
				val5._002Ector(coords.get_x() - 1, coords.get_y() + 1);
				Vector2Int val6 = default(Vector2Int);
				val6._002Ector(coords.get_x() + 1, coords.get_y() + 1);
				Vector2Int val7 = default(Vector2Int);
				val7._002Ector(coords.get_x() - 1, coords.get_y() - 1);
				Vector2Int val8 = default(Vector2Int);
				val8._002Ector(coords.get_x() + 1, coords.get_y() - 1);
				FightMapMovementContext.CellState cellState4 = (val5.get_x() >= sizeMin.get_x() && val5.get_x() < sizeMax.get_x() && val5.get_y() >= sizeMin.get_y() && val5.get_y() < sizeMax.get_y()) ? grid[stateProvider.GetCellIndex(val5.get_x(), val5.get_y())].state : FightMapMovementContext.CellState.None;
				FightMapMovementContext.CellState cellState5 = (val6.get_x() >= sizeMin.get_x() && val6.get_x() < sizeMax.get_x() && val6.get_y() >= sizeMin.get_y() && val6.get_y() < sizeMax.get_y()) ? grid[stateProvider.GetCellIndex(val6.get_x(), val6.get_y())].state : FightMapMovementContext.CellState.None;
				FightMapMovementContext.CellState cellState6 = (val7.get_x() >= sizeMin.get_x() && val7.get_x() < sizeMax.get_x() && val7.get_y() >= sizeMin.get_y() && val7.get_y() < sizeMax.get_y()) ? grid[stateProvider.GetCellIndex(val7.get_x(), val7.get_y())].state : FightMapMovementContext.CellState.None;
				FightMapMovementContext.CellState num6 = (val8.get_x() >= sizeMin.get_x() && val8.get_x() < sizeMax.get_x() && val8.get_y() >= sizeMin.get_y() && val8.get_y() < sizeMax.get_y()) ? grid[stateProvider.GetCellIndex(val8.get_x(), val8.get_y())].state : FightMapMovementContext.CellState.None;
				int num7 = ((cellState4 & (FightMapMovementContext.CellState.Reachable | FightMapMovementContext.CellState.Occupied)) != FightMapMovementContext.CellState.Reachable) ? 1 : 0;
				int num8 = ((cellState5 & (FightMapMovementContext.CellState.Reachable | FightMapMovementContext.CellState.Occupied)) != FightMapMovementContext.CellState.Reachable) ? 1 : 0;
				int num9 = ((cellState6 & (FightMapMovementContext.CellState.Reachable | FightMapMovementContext.CellState.Occupied)) != FightMapMovementContext.CellState.Reachable) ? 1 : 0;
				int num10 = ((num6 & (FightMapMovementContext.CellState.Reachable | FightMapMovementContext.CellState.Occupied)) != FightMapMovementContext.CellState.Reachable) ? 1 : 0;
				int num11 = 1 - num2;
				int num12 = 1 - num3;
				int num13 = 1 - num4;
				int num14 = 1 - num5;
				int num15 = num11 | num12 | (num7 * num2 * num3);
				int num16 = num11 | num13 | (num8 * num2 * num4);
				int num17 = num14 | num12 | (num9 * num5 * num3);
				int num18 = num14 | num13 | (num10 * num5 * num4);
				Compute(num15 | (num16 << 1) | (num17 << 2) | (num18 << 3) | (num11 << 4) | (num12 << 5) | (num13 << 6) | (num14 << 7), areaFeedbackSprites, highlight, color);
				break;
			}
			case 3:
			{
				Sprite sprite2 = areaFeedbackSprites[4];
				float angle = (float)num2 * -90f + (float)num4 * 180f + (float)num5 * 90f;
				highlight.SetSprite(sprite2, color, angle);
				break;
			}
			case 4:
			{
				Sprite sprite = areaFeedbackSprites[5];
				highlight.SetSprite(sprite, color);
				break;
			}
			default:
				throw new ArgumentException();
			}
		}

		public static void SetupSpellTargetHighlight([NotNull] FightMapFeedbackResources resources, [NotNull] FightMapTargetContext context, Vector2Int coords, [NotNull] CellHighlight highlight, Color color)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			if (!context.HasNonEntityTargetAt(coords))
			{
				highlight.ClearSprite();
				return;
			}
			Sprite sprite = resources.areaFeedbackSprites[15];
			highlight.SetSprite(sprite, color);
		}

		private static void Compute(int bitSet, Sprite[] sprites, CellHighlight highlight, Color color)
		{
			//IL_042f: Unknown result type (might be due to invalid IL or missing references)
			Sprite sprite;
			float angle;
			switch (bitSet)
			{
			case 0:
				sprite = sprites[0];
				angle = 0f;
				break;
			case 1:
				sprite = sprites[6];
				angle = 0f;
				break;
			case 2:
				sprite = sprites[6];
				angle = -90f;
				break;
			case 3:
				sprite = sprites[7];
				angle = 0f;
				break;
			case 4:
				sprite = sprites[6];
				angle = 90f;
				break;
			case 5:
				sprite = sprites[7];
				angle = 90f;
				break;
			case 6:
				sprite = sprites[8];
				angle = 0f;
				break;
			case 7:
				sprite = sprites[9];
				angle = 0f;
				break;
			case 8:
				sprite = sprites[6];
				angle = 180f;
				break;
			case 9:
				sprite = sprites[8];
				angle = 90f;
				break;
			case 10:
				sprite = sprites[7];
				angle = -90f;
				break;
			case 11:
				sprite = sprites[9];
				angle = -90f;
				break;
			case 12:
				sprite = sprites[7];
				angle = 180f;
				break;
			case 13:
				sprite = sprites[9];
				angle = 90f;
				break;
			case 14:
				sprite = sprites[9];
				angle = 180f;
				break;
			case 15:
				sprite = sprites[10];
				angle = 0f;
				break;
			case 19:
				sprite = sprites[1];
				angle = 0f;
				break;
			case 23:
				sprite = sprites[12];
				angle = 0f;
				break;
			case 27:
				sprite = sprites[11];
				angle = 0f;
				break;
			case 31:
				sprite = sprites[13];
				angle = 0f;
				break;
			case 37:
				sprite = sprites[1];
				angle = 90f;
				break;
			case 39:
				sprite = sprites[11];
				angle = 90f;
				break;
			case 45:
				sprite = sprites[12];
				angle = 90f;
				break;
			case 47:
				sprite = sprites[13];
				angle = 90f;
				break;
			case 55:
				sprite = sprites[3];
				angle = 0f;
				break;
			case 63:
				sprite = sprites[14];
				angle = 0f;
				break;
			case 74:
				sprite = sprites[1];
				angle = -90f;
				break;
			case 75:
				sprite = sprites[12];
				angle = -90f;
				break;
			case 78:
				sprite = sprites[11];
				angle = -90f;
				break;
			case 79:
				sprite = sprites[13];
				angle = -90f;
				break;
			case 91:
				sprite = sprites[3];
				angle = -90f;
				break;
			case 95:
				sprite = sprites[14];
				angle = -90f;
				break;
			case 111:
				sprite = sprites[2];
				angle = 0f;
				break;
			case 140:
				sprite = sprites[1];
				angle = 180f;
				break;
			case 141:
				sprite = sprites[11];
				angle = 180f;
				break;
			case 142:
				sprite = sprites[12];
				angle = 180f;
				break;
			case 143:
				sprite = sprites[13];
				angle = 180f;
				break;
			case 159:
				sprite = sprites[2];
				angle = 90f;
				break;
			case 173:
				sprite = sprites[3];
				angle = 90f;
				break;
			case 175:
				sprite = sprites[14];
				angle = 90f;
				break;
			case 206:
				sprite = sprites[3];
				angle = 180f;
				break;
			case 207:
				sprite = sprites[14];
				angle = 180f;
				break;
			default:
				throw new ArgumentException(string.Format("[{0}] Impossible configuration: {1}", "SetupMovementAreaHighlight", bitSet));
			}
			highlight.SetSprite(sprite, color, angle);
		}
	}
}
