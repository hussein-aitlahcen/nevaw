using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public abstract class AITargets : IEditableContent
	{
		[Serializable]
		public sealed class All : AITargets, IAIActionTargets, IEditableContent
		{
			public override string ToString()
			{
				return GetType().Name;
			}

			public new static All FromJsonToken(JToken token)
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
				All all = new All();
				all.PopulateFromJson(jsonObject);
				return all;
			}

			public static All FromJsonProperty(JObject jsonObject, string propertyName, All defaultValue = null)
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
		public sealed class Allies : AITargets, IAIActionTargets, IEditableContent
		{
			public override string ToString()
			{
				return GetType().Name;
			}

			public new static Allies FromJsonToken(JToken token)
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
				Allies allies = new Allies();
				allies.PopulateFromJson(jsonObject);
				return allies;
			}

			public static Allies FromJsonProperty(JObject jsonObject, string propertyName, Allies defaultValue = null)
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
		public sealed class AlliesWounded : AITargets, IAIActionTargets, IEditableContent
		{
			public override string ToString()
			{
				return GetType().Name;
			}

			public new static AlliesWounded FromJsonToken(JToken token)
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
				AlliesWounded alliesWounded = new AlliesWounded();
				alliesWounded.PopulateFromJson(jsonObject);
				return alliesWounded;
			}

			public static AlliesWounded FromJsonProperty(JObject jsonObject, string propertyName, AlliesWounded defaultValue = null)
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
		public sealed class Nothing : AITargets, IAIActionTargets, IEditableContent
		{
			public override string ToString()
			{
				return GetType().Name;
			}

			public new static Nothing FromJsonToken(JToken token)
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
				Nothing nothing = new Nothing();
				nothing.PopulateFromJson(jsonObject);
				return nothing;
			}

			public static Nothing FromJsonProperty(JObject jsonObject, string propertyName, Nothing defaultValue = null)
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
		public sealed class Opponents : AITargets, IAIActionTargets, IEditableContent
		{
			public override string ToString()
			{
				return GetType().Name;
			}

			public new static Opponents FromJsonToken(JToken token)
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
				Opponents opponents = new Opponents();
				opponents.PopulateFromJson(jsonObject);
				return opponents;
			}

			public static Opponents FromJsonProperty(JObject jsonObject, string propertyName, Opponents defaultValue = null)
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
		public sealed class SameAsActionTargets : AITargets
		{
			public override string ToString()
			{
				return GetType().Name;
			}

			public new static SameAsActionTargets FromJsonToken(JToken token)
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
				SameAsActionTargets sameAsActionTargets = new SameAsActionTargets();
				sameAsActionTargets.PopulateFromJson(jsonObject);
				return sameAsActionTargets;
			}

			public static SameAsActionTargets FromJsonProperty(JObject jsonObject, string propertyName, SameAsActionTargets defaultValue = null)
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

		public override string ToString()
		{
			return GetType().Name;
		}

		public static AITargets FromJsonToken(JToken token)
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
				Debug.LogWarning((object)"Malformed json: no 'type' property in object of class AITargets");
				return null;
			}
			string text = Extensions.Value<string>((IEnumerable<JToken>)val2);
			AITargets aITargets;
			switch (text)
			{
			case "Nothing":
				aITargets = new Nothing();
				break;
			case "All":
				aITargets = new All();
				break;
			case "Allies":
				aITargets = new Allies();
				break;
			case "AlliesWounded":
				aITargets = new AlliesWounded();
				break;
			case "Opponents":
				aITargets = new Opponents();
				break;
			case "SameAsActionTargets":
				aITargets = new SameAsActionTargets();
				break;
			default:
				Debug.LogWarning((object)("Unknown type: " + text));
				return null;
			}
			aITargets.PopulateFromJson(val);
			return aITargets;
		}

		public static AITargets FromJsonProperty(JObject jsonObject, string propertyName, AITargets defaultValue = null)
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
