using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class SingleCoordWithConditionSelector : IEditableContent, ITargetSelector, ISingleCoordSelector, ICoordSelector, ISingleTargetSelector, ISingleTargetWithFiltersSelector
	{
		private ISingleCoordSelector m_from;

		private List<ICoordFilter> m_onlyIf;

		public ISingleCoordSelector from => m_from;

		public IReadOnlyList<ICoordFilter> onlyIf => m_onlyIf;

		public override string ToString()
		{
			return GetType().Name;
		}

		public static SingleCoordWithConditionSelector FromJsonToken(JToken token)
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
			SingleCoordWithConditionSelector singleCoordWithConditionSelector = new SingleCoordWithConditionSelector();
			singleCoordWithConditionSelector.PopulateFromJson(jsonObject);
			return singleCoordWithConditionSelector;
		}

		public static SingleCoordWithConditionSelector FromJsonProperty(JObject jsonObject, string propertyName, SingleCoordWithConditionSelector defaultValue = null)
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
			m_from = ISingleCoordSelectorUtils.FromJsonProperty(jsonObject, "from");
			JArray val = Serialization.JsonArray(jsonObject, "onlyIf");
			m_onlyIf = new List<ICoordFilter>((val != null) ? val.get_Count() : 0);
			if (val != null)
			{
				foreach (JToken item in val)
				{
					m_onlyIf.Add(ICoordFilterUtils.FromJsonToken(item));
				}
			}
		}

		public IEnumerable<Coord> EnumerateCoords(DynamicValueContext context)
		{
			DynamicValueFightContext dynamicValueFightContext = context as DynamicValueFightContext;
			if (dynamicValueFightContext != null)
			{
				IEnumerable<Coord> enumerable = dynamicValueFightContext.fightStatus.EnumerateCoords();
				int count = m_onlyIf.Count;
				for (int i = 0; i < count; i++)
				{
					enumerable = m_onlyIf[i].Filter(enumerable, context);
				}
				return enumerable;
			}
			return Enumerable.Empty<Coord>();
		}

		public bool TryGetCoord(DynamicValueContext context, out Coord coord)
		{
			throw new NotImplementedException();
		}
	}
}
