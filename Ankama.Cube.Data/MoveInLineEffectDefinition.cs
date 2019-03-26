using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class MoveInLineEffectDefinition : EffectExecutionDefinition
	{
		private DynamicValue m_cellCount;

		private MoveVector m_direction;

		private MovementType m_movementType;

		private bool m_canPassThroughEntities;

		public DynamicValue cellCount => m_cellCount;

		public MoveVector direction => m_direction;

		public MovementType movementType => m_movementType;

		public bool canPassThroughEntities => m_canPassThroughEntities;

		public override string ToString()
		{
			return string.Format("Move {0} {1} of {2} cells in line {3}", m_executionTargetSelector, direction, cellCount, (m_condition == null) ? "" : $" if {m_condition}");
		}

		public new static MoveInLineEffectDefinition FromJsonToken(JToken token)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if ((int)token.get_Type() != 1)
			{
				Debug.LogWarning((object)("Malformed token : type Object expected, but " + token.get_Type() + " found"));
				return null;
			}
			JObject jsonObject = Extensions.Value<JObject>((IEnumerable<JToken>)token);
			MoveInLineEffectDefinition moveInLineEffectDefinition = new MoveInLineEffectDefinition();
			moveInLineEffectDefinition.PopulateFromJson(jsonObject);
			return moveInLineEffectDefinition;
		}

		public static MoveInLineEffectDefinition FromJsonProperty(JObject jsonObject, string propertyName, MoveInLineEffectDefinition defaultValue = null)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Invalid comparison between Unknown and I4
			JProperty val = jsonObject.Property(propertyName);
			if (val == null || (int)val.get_Value().get_Type() == 10)
			{
				return defaultValue;
			}
			return FromJsonToken(val.get_Value());
		}

		public override void PopulateFromJson(JObject jsonObject)
		{
			base.PopulateFromJson(jsonObject);
			m_cellCount = DynamicValue.FromJsonProperty(jsonObject, "cellCount");
			m_direction = MoveVector.FromJsonProperty(jsonObject, "direction");
			m_movementType = (MovementType)Serialization.JsonTokenValue<int>(jsonObject, "movementType", 6);
			m_canPassThroughEntities = Serialization.JsonTokenValue<bool>(jsonObject, "canPassThroughEntities", false);
		}
	}
}
