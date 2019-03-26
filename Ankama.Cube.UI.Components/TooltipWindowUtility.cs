using Ankama.Cube.Data;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps;
using Ankama.Cube.UI.Fight;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.UI.Components
{
	public static class TooltipWindowUtility
	{
		public unsafe static void ShowFightCharacterTooltip(ITooltipDataProvider dataProvider, Vector3 worldPosition)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			Transform transform = CameraHandler.current.camera.get_transform();
			TooltipPosition position;
			if (((IntPtr)(void*)transform.InverseTransformPoint(worldPosition)).x < 0f)
			{
				worldPosition += 0.7071f * transform.get_right();
				position = TooltipPosition.Right;
			}
			else
			{
				worldPosition -= 0.7071f * transform.get_right();
				position = TooltipPosition.Left;
			}
			Vector3 worldPosition2 = FightUIRework.WorldToUIWorld(worldPosition);
			FightUIRework.ShowTooltip(dataProvider, position, worldPosition2);
		}

		public static TooltipElementValues GetTooltipElementValues(SpellDefinition definition, DynamicValueContext context)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			IReadOnlyList<GaugeValue> gaugeToModifyOnSpellPlay = definition.gaugeToModifyOnSpellPlay;
			int count = gaugeToModifyOnSpellPlay.Count;
			for (int i = 0; i < count; i++)
			{
				GaugeValue gaugeValue = gaugeToModifyOnSpellPlay[i];
				if (gaugeValue.value.GetValue(context, out int value))
				{
					switch (gaugeValue.element)
					{
					case CaracId.FirePoints:
						num3 += value;
						break;
					case CaracId.WaterPoints:
						num4 += value;
						break;
					case CaracId.EarthPoints:
						num2 += value;
						break;
					case CaracId.AirPoints:
						num += value;
						break;
					case CaracId.ReservePoints:
						num5 += value;
						break;
					}
				}
			}
			return new TooltipElementValues(num, num2, num3, num4, num5);
		}

		public static TooltipActionIcon GetActionIcon(CharacterDefinition definition)
		{
			return GetActionIcon(definition.actionType, definition.actionRange != null);
		}

		public static TooltipActionIcon GetActionIcon(ActionType actionType, bool hasRange)
		{
			switch (actionType)
			{
			case ActionType.Attack:
				if (!hasRange)
				{
					return TooltipActionIcon.AttackCloseCombat;
				}
				return TooltipActionIcon.AttackRanged;
			case ActionType.Heal:
				return TooltipActionIcon.Heal;
			case ActionType.None:
				return TooltipActionIcon.None;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		public static Vector3 ScreenOffsetInWorld(Vector3 screenOffset, Canvas parentCanvas)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			if ((int)parentCanvas.get_renderMode() == 0)
			{
				return screenOffset * parentCanvas.get_scaleFactor();
			}
			return parentCanvas.get_transform().TransformVector(screenOffset);
		}
	}
}
