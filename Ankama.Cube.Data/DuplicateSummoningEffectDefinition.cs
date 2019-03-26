using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class DuplicateSummoningEffectDefinition : EffectExecutionDefinition
	{
		private ISingleEntitySelector m_invocationOwner;

		private ISingleCoordSelector m_destination;

		private bool? m_copyNonHealableLifeValue;

		public ISingleEntitySelector invocationOwner => m_invocationOwner;

		public ISingleCoordSelector destination => m_destination;

		public bool? copyNonHealableLifeValue => m_copyNonHealableLifeValue;

		public override string ToString()
		{
			return GetType().Name;
		}

		public new static DuplicateSummoningEffectDefinition FromJsonToken(JToken token)
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
			DuplicateSummoningEffectDefinition duplicateSummoningEffectDefinition = new DuplicateSummoningEffectDefinition();
			duplicateSummoningEffectDefinition.PopulateFromJson(jsonObject);
			return duplicateSummoningEffectDefinition;
		}

		public static DuplicateSummoningEffectDefinition FromJsonProperty(JObject jsonObject, string propertyName, DuplicateSummoningEffectDefinition defaultValue = null)
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
			m_invocationOwner = ISingleEntitySelectorUtils.FromJsonProperty(jsonObject, "invocationOwner");
			m_destination = ISingleCoordSelectorUtils.FromJsonProperty(jsonObject, "destination");
			m_copyNonHealableLifeValue = Serialization.JsonTokenValue<bool?>(jsonObject, "copyNonHealableLifeValue", (bool?)null);
		}
	}
}
