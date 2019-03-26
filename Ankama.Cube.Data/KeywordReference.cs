using Ankama.Cube.Data.UI.Localization.TextFormatting;
using DataEditor;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ankama.Cube.Data
{
	[Serializable]
	public struct KeywordReference
	{
		public readonly ObjectReference.Type type;

		public readonly int id;

		public readonly string keyword;

		private readonly KeywordCondition conditionMask;

		public KeywordReference(ObjectReference.Type type, int id, KeywordCondition conditionMask = (KeywordCondition)0)
		{
			this.type = type;
			this.id = id;
			keyword = null;
			this.conditionMask = conditionMask;
		}

		public KeywordReference(string keyword, KeywordCondition conditionMask = (KeywordCondition)0)
		{
			type = ObjectReference.Type.None;
			id = 0;
			this.keyword = keyword;
			this.conditionMask = conditionMask;
		}

		public bool IsValidFor(KeywordContext context)
		{
			return ((int)context & (int)conditionMask) == (int)conditionMask;
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (type == ObjectReference.Type.None)
			{
				stringBuilder.Append(keyword);
			}
			else
			{
				stringBuilder.Append(type.ToString().ToLower()).Append(':').Append(id);
			}
			if (conditionMask != 0)
			{
				stringBuilder.Append("<if ").Append(conditionMask.ToString()).Append("/>");
			}
			return stringBuilder.ToString();
		}

		public bool Equals(KeywordReference other)
		{
			if (type == other.type && id == other.id && string.Equals(keyword, other.keyword))
			{
				return conditionMask == other.conditionMask;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj is KeywordReference)
			{
				return Equals((KeywordReference)obj);
			}
			return false;
		}

		public override int GetHashCode()
		{
			int num = (int)conditionMask;
			if (keyword == null)
			{
				num = ((num * 397) ^ (int)type);
				return (num * 397) ^ id;
			}
			return (num * 397) ^ keyword.GetHashCode();
		}

		public static bool operator ==(KeywordReference left, KeywordReference right)
		{
			if (left.type == right.type && left.id == right.id && left.keyword == right.keyword)
			{
				return left.conditionMask == right.conditionMask;
			}
			return false;
		}

		public static bool operator !=(KeywordReference left, KeywordReference right)
		{
			if (left.type == right.type && left.id == right.id && !(left.keyword != right.keyword))
			{
				return left.conditionMask != right.conditionMask;
			}
			return true;
		}

		public static void Write(KeywordReference keywordReference, JsonTextWriter writer)
		{
			KeywordCondition keywordCondition = keywordReference.conditionMask;
			if (keywordReference.type == ObjectReference.Type.None)
			{
				if (keywordCondition == (KeywordCondition)0)
				{
					writer.WriteValue(keywordReference.keyword);
					return;
				}
				writer.WriteStartObject();
				writer.WritePropertyName("condition");
				writer.WriteValue((object)keywordCondition);
				writer.WritePropertyName("keyword");
				writer.WriteValue(keywordReference.keyword);
				writer.WriteEndObject();
				return;
			}
			writer.WriteStartObject();
			if (keywordCondition != 0)
			{
				writer.WritePropertyName("condition");
				writer.WriteValue((object)keywordCondition);
			}
			writer.WritePropertyName("type");
			writer.WriteValue((object)keywordReference.type);
			writer.WritePropertyName("id");
			writer.WriteValue(keywordReference.id);
			writer.WriteEndObject();
		}

		public static KeywordReference FromJson(JToken jToken)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected O, but got Unknown
			if ((int)jToken.get_Type() == 8)
			{
				return new KeywordReference(Extensions.Value<string>((IEnumerable<JToken>)jToken));
			}
			JObject val = jToken;
			KeywordCondition keywordCondition = (KeywordCondition)Serialization.JsonTokenValue<int>(val, "condition", 0);
			string text = Serialization.JsonTokenValue<string>(val, "keyword", (string)null);
			if (text != null)
			{
				return new KeywordReference(text, keywordCondition);
			}
			ObjectReference.Type type = (ObjectReference.Type)Serialization.JsonTokenValue<int>(val, "type", 0);
			int num = Serialization.JsonTokenValue<int>(val, "id", 0);
			return new KeywordReference(type, num, keywordCondition);
		}
	}
}
