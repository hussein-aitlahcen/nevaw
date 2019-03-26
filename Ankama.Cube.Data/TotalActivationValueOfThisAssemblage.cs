using Ankama.Cube.Fight;
using Ankama.Cube.Fight.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class TotalActivationValueOfThisAssemblage : DynamicValue
	{
		public override string ToString()
		{
			return GetType().Name;
		}

		public new static TotalActivationValueOfThisAssemblage FromJsonToken(JToken token)
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
			TotalActivationValueOfThisAssemblage totalActivationValueOfThisAssemblage = new TotalActivationValueOfThisAssemblage();
			totalActivationValueOfThisAssemblage.PopulateFromJson(jsonObject);
			return totalActivationValueOfThisAssemblage;
		}

		public static TotalActivationValueOfThisAssemblage FromJsonProperty(JObject jsonObject, string propertyName, TotalActivationValueOfThisAssemblage defaultValue = null)
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
		}

		public override bool GetValue(DynamicValueContext context, out int value)
		{
			AssembledEntityContext assembledEntityContext = context as AssembledEntityContext;
			if (assembledEntityContext == null)
			{
				value = 0;
				return false;
			}
			int num = 0;
			IReadOnlyList<int> assemblingIds = assembledEntityContext.assembling.assemblingIds;
			FightStatus fightStatus = assembledEntityContext.fightStatus;
			int i = 0;
			for (int count = assemblingIds.Count; i < count; i++)
			{
				if (fightStatus.TryGetEntity(assemblingIds[i], out FloorMechanismStatus entityStatus))
				{
					int? activationValue = entityStatus.activationValue;
					if (activationValue.HasValue)
					{
						num += activationValue.Value;
					}
				}
			}
			value = num;
			return true;
		}
	}
}
