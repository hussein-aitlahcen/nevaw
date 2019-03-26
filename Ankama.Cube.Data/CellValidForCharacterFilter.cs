using Ankama.Cube.Fight;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class CellValidForCharacterFilter : IEditableContent, ICoordFilter, ITargetFilter
	{
		public override string ToString()
		{
			return GetType().Name;
		}

		public static CellValidForCharacterFilter FromJsonToken(JToken token)
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
			CellValidForCharacterFilter cellValidForCharacterFilter = new CellValidForCharacterFilter();
			cellValidForCharacterFilter.PopulateFromJson(jsonObject);
			return cellValidForCharacterFilter;
		}

		public static CellValidForCharacterFilter FromJsonProperty(JObject jsonObject, string propertyName, CellValidForCharacterFilter defaultValue = null)
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

		public void PopulateFromJson(JObject jsonObject)
		{
		}

		public IEnumerable<Coord> Filter(IEnumerable<Coord> coords, DynamicValueContext context)
		{
			DynamicValueFightContext dynamicValueFightContext = context as CastTargetContext;
			if (dynamicValueFightContext == null)
			{
				return ICoordFilterExtension.empty;
			}
			return Filter(coords, dynamicValueFightContext.fightStatus);
		}

		private static IEnumerable<Coord> Filter(IEnumerable<Coord> coords, FightStatus fightStatus)
		{
			foreach (Coord coord in coords)
			{
				if (!fightStatus.HasEntityBlockingMovementAt((Vector2Int)coord))
				{
					yield return coord;
				}
			}
		}

		public static IEnumerable<Coord> EnumerateCells(FightStatus fightStatus)
		{
			return Filter(fightStatus.EnumerateCoords(), fightStatus);
		}
	}
}
