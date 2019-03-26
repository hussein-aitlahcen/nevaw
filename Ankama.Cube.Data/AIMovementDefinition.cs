using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public abstract class AIMovementDefinition : IEditableContent
	{
		[Serializable]
		public sealed class DoNothing : AIMovementDefinition
		{
			public override string ToString()
			{
				return GetType().Name;
			}

			public new static DoNothing FromJsonToken(JToken token)
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
				DoNothing doNothing = new DoNothing();
				doNothing.PopulateFromJson(jsonObject);
				return doNothing;
			}

			public static DoNothing FromJsonProperty(JObject jsonObject, string propertyName, DoNothing defaultValue = null)
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
		}

		[Serializable]
		public sealed class GetCloserTo : AIMovementDefinition
		{
			private AITargets m_targets;

			public AITargets targets => m_targets;

			public override string ToString()
			{
				return $"GetCloserTo {m_targets}";
			}

			public new static GetCloserTo FromJsonToken(JToken token)
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
				GetCloserTo getCloserTo = new GetCloserTo();
				getCloserTo.PopulateFromJson(jsonObject);
				return getCloserTo;
			}

			public static GetCloserTo FromJsonProperty(JObject jsonObject, string propertyName, GetCloserTo defaultValue = null)
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
				m_targets = AITargets.FromJsonProperty(jsonObject, "targets");
			}
		}

		[Serializable]
		public sealed class RunawayFrom : AIMovementDefinition
		{
			private AITargets m_targets;

			public AITargets targets => m_targets;

			public override string ToString()
			{
				return $"RunawayFrom {m_targets}";
			}

			public new static RunawayFrom FromJsonToken(JToken token)
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
				RunawayFrom runawayFrom = new RunawayFrom();
				runawayFrom.PopulateFromJson(jsonObject);
				return runawayFrom;
			}

			public static RunawayFrom FromJsonProperty(JObject jsonObject, string propertyName, RunawayFrom defaultValue = null)
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
				m_targets = AITargets.FromJsonProperty(jsonObject, "targets");
			}
		}

		[Serializable]
		public sealed class StayOutOfAttack : AIMovementDefinition
		{
			private AITargets m_targets;

			public AITargets targets => m_targets;

			public override string ToString()
			{
				return $"StayOutOfAttack {m_targets}";
			}

			public new static StayOutOfAttack FromJsonToken(JToken token)
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
				StayOutOfAttack stayOutOfAttack = new StayOutOfAttack();
				stayOutOfAttack.PopulateFromJson(jsonObject);
				return stayOutOfAttack;
			}

			public static StayOutOfAttack FromJsonProperty(JObject jsonObject, string propertyName, StayOutOfAttack defaultValue = null)
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
				m_targets = AITargets.FromJsonProperty(jsonObject, "targets");
			}
		}

		public override string ToString()
		{
			return GetType().Name;
		}

		public static AIMovementDefinition FromJsonToken(JToken token)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if ((int)token.get_Type() != 1)
			{
				Debug.LogWarning((object)("Malformed token : type Object expected, but " + token.get_Type() + " found"));
				return null;
			}
			JObject val = Extensions.Value<JObject>((IEnumerable<JToken>)token);
			JToken val2 = default(JToken);
			if (!val.TryGetValue("type", ref val2))
			{
				Debug.LogWarning((object)"Malformed json: no 'type' property in object of class AIMovementDefinition");
				return null;
			}
			string text = Extensions.Value<string>((IEnumerable<JToken>)val2);
			AIMovementDefinition aIMovementDefinition;
			switch (text)
			{
			case "DoNothing":
				aIMovementDefinition = new DoNothing();
				break;
			case "GetCloserTo":
				aIMovementDefinition = new GetCloserTo();
				break;
			case "StayOutOfAttack":
				aIMovementDefinition = new StayOutOfAttack();
				break;
			case "RunawayFrom":
				aIMovementDefinition = new RunawayFrom();
				break;
			default:
				Debug.LogWarning((object)("Unknown type: " + text));
				return null;
			}
			aIMovementDefinition.PopulateFromJson(val);
			return aIMovementDefinition;
		}

		public static AIMovementDefinition FromJsonProperty(JObject jsonObject, string propertyName, AIMovementDefinition defaultValue = null)
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

		public virtual void PopulateFromJson(JObject jsonObject)
		{
		}
	}
}
