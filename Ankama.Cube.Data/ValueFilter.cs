using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public abstract class ValueFilter : IEditableContent
	{
		[Serializable]
		public sealed class Between : ValueFilter
		{
			private DynamicValue m_minIncluded;

			private DynamicValue m_maxIncluded;

			public DynamicValue minIncluded => m_minIncluded;

			public DynamicValue maxIncluded => m_maxIncluded;

			public override string ToString()
			{
				return $"in [{minIncluded}; {maxIncluded}] ";
			}

			public new static Between FromJsonToken(JToken token)
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
				Between between = new Between();
				between.PopulateFromJson(jsonObject);
				return between;
			}

			public static Between FromJsonProperty(JObject jsonObject, string propertyName, Between defaultValue = null)
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
				m_minIncluded = DynamicValue.FromJsonProperty(jsonObject, "minIncluded");
				m_maxIncluded = DynamicValue.FromJsonProperty(jsonObject, "maxIncluded");
			}

			public override bool Matches(int value, DynamicValueContext context)
			{
				minIncluded.GetValue(context, out int value2);
				maxIncluded.GetValue(context, out int value3);
				if (value2 <= value)
				{
					return value3 >= value;
				}
				return false;
			}
		}

		[Serializable]
		public sealed class EqualsTo : SingleValueFilter
		{
			public override string ToString()
			{
				return $"== {base.dynamicValue}";
			}

			public new static EqualsTo FromJsonToken(JToken token)
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
				EqualsTo equalsTo = new EqualsTo();
				equalsTo.PopulateFromJson(jsonObject);
				return equalsTo;
			}

			public static EqualsTo FromJsonProperty(JObject jsonObject, string propertyName, EqualsTo defaultValue = null)
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

			public override bool Matches(int value, DynamicValueContext context)
			{
				base.dynamicValue.GetValue(context, out int value2);
				return value == value2;
			}
		}

		[Serializable]
		public sealed class GreaterEqualThan : SingleValueFilter
		{
			public override string ToString()
			{
				return $">= {base.dynamicValue}";
			}

			public new static GreaterEqualThan FromJsonToken(JToken token)
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
				GreaterEqualThan greaterEqualThan = new GreaterEqualThan();
				greaterEqualThan.PopulateFromJson(jsonObject);
				return greaterEqualThan;
			}

			public static GreaterEqualThan FromJsonProperty(JObject jsonObject, string propertyName, GreaterEqualThan defaultValue = null)
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

			public override bool Matches(int value, DynamicValueContext context)
			{
				base.dynamicValue.GetValue(context, out int value2);
				return value >= value2;
			}
		}

		[Serializable]
		public sealed class GreaterThan : SingleValueFilter
		{
			public override string ToString()
			{
				return $"> {base.dynamicValue}";
			}

			public new static GreaterThan FromJsonToken(JToken token)
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
				GreaterThan greaterThan = new GreaterThan();
				greaterThan.PopulateFromJson(jsonObject);
				return greaterThan;
			}

			public static GreaterThan FromJsonProperty(JObject jsonObject, string propertyName, GreaterThan defaultValue = null)
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

			public override bool Matches(int value, DynamicValueContext context)
			{
				base.dynamicValue.GetValue(context, out int value2);
				return value > value2;
			}
		}

		[Serializable]
		public sealed class IsEven : ValueFilter
		{
			public override string ToString()
			{
				return GetType().Name;
			}

			public new static IsEven FromJsonToken(JToken token)
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
				IsEven isEven = new IsEven();
				isEven.PopulateFromJson(jsonObject);
				return isEven;
			}

			public static IsEven FromJsonProperty(JObject jsonObject, string propertyName, IsEven defaultValue = null)
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

			public override bool Matches(int value, DynamicValueContext context)
			{
				return value % 2 == 0;
			}
		}

		[Serializable]
		public sealed class IsOdd : ValueFilter
		{
			public override string ToString()
			{
				return GetType().Name;
			}

			public new static IsOdd FromJsonToken(JToken token)
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
				IsOdd isOdd = new IsOdd();
				isOdd.PopulateFromJson(jsonObject);
				return isOdd;
			}

			public static IsOdd FromJsonProperty(JObject jsonObject, string propertyName, IsOdd defaultValue = null)
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

			public override bool Matches(int value, DynamicValueContext context)
			{
				return value % 2 == 1;
			}
		}

		[Serializable]
		public sealed class LowerEqualThan : SingleValueFilter
		{
			public override string ToString()
			{
				return $"<= {base.dynamicValue}";
			}

			public new static LowerEqualThan FromJsonToken(JToken token)
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
				LowerEqualThan lowerEqualThan = new LowerEqualThan();
				lowerEqualThan.PopulateFromJson(jsonObject);
				return lowerEqualThan;
			}

			public static LowerEqualThan FromJsonProperty(JObject jsonObject, string propertyName, LowerEqualThan defaultValue = null)
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

			public override bool Matches(int value, DynamicValueContext context)
			{
				base.dynamicValue.GetValue(context, out int value2);
				return value <= value2;
			}
		}

		[Serializable]
		public sealed class LowerThan : SingleValueFilter
		{
			public override string ToString()
			{
				return $"< {base.dynamicValue}";
			}

			public new static LowerThan FromJsonToken(JToken token)
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
				LowerThan lowerThan = new LowerThan();
				lowerThan.PopulateFromJson(jsonObject);
				return lowerThan;
			}

			public static LowerThan FromJsonProperty(JObject jsonObject, string propertyName, LowerThan defaultValue = null)
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

			public override bool Matches(int value, DynamicValueContext context)
			{
				base.dynamicValue.GetValue(context, out int value2);
				return value < value2;
			}
		}

		[Serializable]
		public sealed class NotEqualsTo : SingleValueFilter
		{
			public override string ToString()
			{
				return $"!= {base.dynamicValue}";
			}

			public new static NotEqualsTo FromJsonToken(JToken token)
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
				NotEqualsTo notEqualsTo = new NotEqualsTo();
				notEqualsTo.PopulateFromJson(jsonObject);
				return notEqualsTo;
			}

			public static NotEqualsTo FromJsonProperty(JObject jsonObject, string propertyName, NotEqualsTo defaultValue = null)
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

			public override bool Matches(int value, DynamicValueContext context)
			{
				base.dynamicValue.GetValue(context, out int value2);
				return value != value2;
			}
		}

		[Serializable]
		public abstract class SingleValueFilter : ValueFilter
		{
			protected DynamicValue m_dynamicValue;

			public DynamicValue dynamicValue => m_dynamicValue;

			public override string ToString()
			{
				return GetType().Name;
			}

			public new static SingleValueFilter FromJsonToken(JToken token)
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
					Debug.LogWarning((object)"Malformed json: no 'type' property in object of class SingleValueFilter");
					return null;
				}
				string text = Extensions.Value<string>((IEnumerable<JToken>)val2);
				SingleValueFilter singleValueFilter;
				switch (text)
				{
				case "EqualsTo":
					singleValueFilter = new EqualsTo();
					break;
				case "LowerThan":
					singleValueFilter = new LowerThan();
					break;
				case "GreaterThan":
					singleValueFilter = new GreaterThan();
					break;
				case "NotEqualsTo":
					singleValueFilter = new NotEqualsTo();
					break;
				case "LowerEqualThan":
					singleValueFilter = new LowerEqualThan();
					break;
				case "GreaterEqualThan":
					singleValueFilter = new GreaterEqualThan();
					break;
				default:
					Debug.LogWarning((object)("Unknown type: " + text));
					return null;
				}
				singleValueFilter.PopulateFromJson(val);
				return singleValueFilter;
			}

			public static SingleValueFilter FromJsonProperty(JObject jsonObject, string propertyName, SingleValueFilter defaultValue = null)
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
				m_dynamicValue = DynamicValue.FromJsonProperty(jsonObject, "dynamicValue");
			}
		}

		public override string ToString()
		{
			return GetType().Name;
		}

		public static ValueFilter FromJsonToken(JToken token)
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
				Debug.LogWarning((object)"Malformed json: no 'type' property in object of class ValueFilter");
				return null;
			}
			string text = Extensions.Value<string>((IEnumerable<JToken>)val2);
			ValueFilter valueFilter;
			switch (text)
			{
			case "IsEven":
				valueFilter = new IsEven();
				break;
			case "IsOdd":
				valueFilter = new IsOdd();
				break;
			case "Between":
				valueFilter = new Between();
				break;
			case "EqualsTo":
				valueFilter = new EqualsTo();
				break;
			case "LowerThan":
				valueFilter = new LowerThan();
				break;
			case "GreaterThan":
				valueFilter = new GreaterThan();
				break;
			case "NotEqualsTo":
				valueFilter = new NotEqualsTo();
				break;
			case "LowerEqualThan":
				valueFilter = new LowerEqualThan();
				break;
			case "GreaterEqualThan":
				valueFilter = new GreaterEqualThan();
				break;
			default:
				Debug.LogWarning((object)("Unknown type: " + text));
				return null;
			}
			valueFilter.PopulateFromJson(val);
			return valueFilter;
		}

		public static ValueFilter FromJsonProperty(JObject jsonObject, string propertyName, ValueFilter defaultValue = null)
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

		public abstract bool Matches(int value, DynamicValueContext context);
	}
}
