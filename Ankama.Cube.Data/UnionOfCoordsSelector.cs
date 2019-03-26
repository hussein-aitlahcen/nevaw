using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class UnionOfCoordsSelector : CoordSelectorForCast
	{
		private ICoordSelector m_first;

		private ICoordSelector m_second;

		public ICoordSelector first => m_first;

		public ICoordSelector second => m_second;

		public override string ToString()
		{
			return $"({m_first} OR {m_second})";
		}

		public new static UnionOfCoordsSelector FromJsonToken(JToken token)
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
			UnionOfCoordsSelector unionOfCoordsSelector = new UnionOfCoordsSelector();
			unionOfCoordsSelector.PopulateFromJson(jsonObject);
			return unionOfCoordsSelector;
		}

		public static UnionOfCoordsSelector FromJsonProperty(JObject jsonObject, string propertyName, UnionOfCoordsSelector defaultValue = null)
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
			m_first = ICoordSelectorUtils.FromJsonProperty(jsonObject, "first");
			m_second = ICoordSelectorUtils.FromJsonProperty(jsonObject, "second");
		}

		public override IEnumerable<Coord> EnumerateCoords(DynamicValueContext context)
		{
			HashSet<Coord> coordsSet = new HashSet<Coord>();
			foreach (Coord c in m_first.EnumerateCoords(context))
			{
				yield return c;
				coordsSet.Add(c);
			}
			foreach (Coord item in m_second.EnumerateCoords(context))
			{
				if (coordsSet.Add(item))
				{
					yield return item;
				}
			}
		}
	}
}
