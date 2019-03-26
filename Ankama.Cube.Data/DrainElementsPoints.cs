using Ankama.Cube.Fight;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.UI.Fight;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	[RelatedToEvents(new EventCategory[]
	{
		EventCategory.ElementPointsChanged
	})]
	public sealed class DrainElementsPoints : Cost
	{
		private List<CaracId> m_elements;

		public IReadOnlyList<CaracId> elements => m_elements;

		public override string ToString()
		{
			return "Consume all " + string.Join(" & ", m_elements);
		}

		public new static DrainElementsPoints FromJsonToken(JToken token)
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
			DrainElementsPoints drainElementsPoints = new DrainElementsPoints();
			drainElementsPoints.PopulateFromJson(jsonObject);
			return drainElementsPoints;
		}

		public static DrainElementsPoints FromJsonProperty(JObject jsonObject, string propertyName, DrainElementsPoints defaultValue = null)
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
			m_elements = Serialization.JsonArrayAsList<CaracId>(jsonObject, "elements");
		}

		protected override CastValidity InternalCheckValidity(PlayerStatus status, DynamicValueContext castTargetContext)
		{
			for (int i = 0; i < elements.Count; i++)
			{
				if (status.GetCarac(elements[i]) <= 0)
				{
					return CastValidity.NOT_ENOUGH_ELEMENT_POINTS;
				}
			}
			return CastValidity.SUCCESS;
		}

		public override void UpdateModifications(ref GaugesModification modifications, PlayerStatus player, DynamicValueContext context)
		{
			for (int i = 0; i < elements.Count; i++)
			{
				CaracId carac = elements[i];
				modifications.Increment(carac, -player.GetCarac(carac));
			}
		}
	}
}
