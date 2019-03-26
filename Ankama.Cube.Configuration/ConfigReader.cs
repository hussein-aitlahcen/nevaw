using Ankama.Utilities;
using Newtonsoft.Json.Linq;
using System;
using System.Net;

namespace Ankama.Cube.Configuration
{
	public class ConfigReader
	{
		private readonly JObject m_json;

		private readonly string m_context;

		public ConfigReader(string text, string context = null)
		{
			m_json = JObject.Parse(text);
			m_context = (context ?? text);
		}

		public ConfigReader GetConfig(string propertyName)
		{
			JToken val = default(JToken);
			if (!m_json.TryGetValue(propertyName, ref val))
			{
				LogPropertyNotFound(propertyName);
				return null;
			}
			return new ConfigReader(((object)val).ToString(), propertyName + " in " + m_context);
		}

		public bool HasProperty(string propertyName)
		{
			return m_json.Property(propertyName) != null;
		}

		public string GetString(string propertyName, string defaultValue = "")
		{
			JToken val = default(JToken);
			if (!m_json.TryGetValue(propertyName, ref val))
			{
				LogPropertyNotFound(propertyName);
				return defaultValue;
			}
			return (string)val;
		}

		public int GetInt(string propertyName, int defaultValue = 0)
		{
			JToken val = default(JToken);
			if (!m_json.TryGetValue(propertyName, ref val))
			{
				LogPropertyNotFound(propertyName);
				return defaultValue;
			}
			if (!int.TryParse((string)val, out int result))
			{
				LogWrongType<int>(val);
				return defaultValue;
			}
			return result;
		}

		public bool GetBool(string propertyName, bool defaultValue = false)
		{
			JToken val = default(JToken);
			if (!m_json.TryGetValue(propertyName, ref val))
			{
				LogPropertyNotFound(propertyName);
				return defaultValue;
			}
			if (!bool.TryParse((string)val, out bool result))
			{
				LogWrongType<bool>(val);
				return defaultValue;
			}
			return result;
		}

		public float GetFloat(string propertyName, float defaultValue = 0f)
		{
			JToken val = default(JToken);
			if (!m_json.TryGetValue(propertyName, ref val))
			{
				LogPropertyNotFound(propertyName);
				return defaultValue;
			}
			if (!float.TryParse((string)val, out float result))
			{
				LogWrongType<float>(val);
				return defaultValue;
			}
			return result;
		}

		public string GetIPAddress(string propertyName, string defaultValue = "")
		{
			JToken val = default(JToken);
			if (!m_json.TryGetValue(propertyName, ref val))
			{
				LogPropertyNotFound(propertyName);
				return defaultValue;
			}
			if (!IPAddress.TryParse((string)val, out IPAddress address))
			{
				LogWrongType<IPAddress>(val);
				return defaultValue;
			}
			return address.ToString();
		}

		public string GetUrl(string propertyName, string defaultValue = "")
		{
			JToken val = default(JToken);
			if (!m_json.TryGetValue(propertyName, ref val))
			{
				LogPropertyNotFound(propertyName);
				return defaultValue;
			}
			if (!Uri.TryCreate((string)val, UriKind.Absolute, out Uri result) || (result.Scheme != Uri.UriSchemeHttp && result.Scheme != Uri.UriSchemeHttps))
			{
				LogWrongType<Uri>(val);
				return defaultValue;
			}
			return result.AbsoluteUri;
		}

		public T GetEnum<T>(string propertyName, T defaultValue) where T : struct
		{
			JToken val = default(JToken);
			if (!m_json.TryGetValue(propertyName, ref val))
			{
				LogPropertyNotFound(propertyName);
				return defaultValue;
			}
			if (!Enum.TryParse((string)val, ignoreCase: true, out T result))
			{
				LogWrongType<T>(val);
				return defaultValue;
			}
			return result;
		}

		private void LogPropertyNotFound(string propertyName)
		{
			Log.Warning("'" + propertyName + "' not found in " + m_context + ".", 152, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Configuration\\ConfigReader.cs");
		}

		private void LogWrongType<T>(JToken value)
		{
			Log.Warning($"'{value}' is not a valid {typeof(T)} in {m_context}", 157, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Configuration\\ConfigReader.cs");
		}
	}
}
