using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class PlayEntityAnimationEffectDefinition : EffectExecutionDefinition
	{
		private EntityAnimationKey m_animation;

		private List<ISingleTargetSelector> m_additionalCoords;

		public EntityAnimationKey animation => m_animation;

		public IReadOnlyList<ISingleTargetSelector> additionalCoords => m_additionalCoords;

		public override string ToString()
		{
			return GetType().Name;
		}

		public new static PlayEntityAnimationEffectDefinition FromJsonToken(JToken token)
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
			PlayEntityAnimationEffectDefinition playEntityAnimationEffectDefinition = new PlayEntityAnimationEffectDefinition();
			playEntityAnimationEffectDefinition.PopulateFromJson(jsonObject);
			return playEntityAnimationEffectDefinition;
		}

		public static PlayEntityAnimationEffectDefinition FromJsonProperty(JObject jsonObject, string propertyName, PlayEntityAnimationEffectDefinition defaultValue = null)
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
			m_animation = (EntityAnimationKey)Serialization.JsonTokenValue<int>(jsonObject, "animation", 1);
			JArray val = Serialization.JsonArray(jsonObject, "additionalCoords");
			m_additionalCoords = new List<ISingleTargetSelector>((val != null) ? val.get_Count() : 0);
			if (val != null)
			{
				foreach (JToken item in val)
				{
					m_additionalCoords.Add(ISingleTargetSelectorUtils.FromJsonToken(item));
				}
			}
		}
	}
}
