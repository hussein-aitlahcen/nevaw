using Ankama.Cube.Data.UI.Localization;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data.UI
{
	[Serializable]
	public struct CultureCode : IEquatable<CultureCode>
	{
		private enum Value
		{
			FR_FR = 1036,
			EN_US = 1033,
			ES_ES = 3082
		}

		public static readonly CultureCode FR_FR = new CultureCode(Value.FR_FR);

		public static readonly CultureCode EN_US = new CultureCode(Value.EN_US);

		public static readonly CultureCode ES_ES = new CultureCode(Value.ES_ES);

		public static readonly CultureCode Default = FR_FR;

		public static readonly CultureCode Fallback = EN_US;

		[UsedImplicitly]
		[SerializeField]
		private Value m_value;

		private CultureCode(Value value)
		{
			m_value = value;
		}

		[Pure]
		public FontLanguage GetFontLanguage()
		{
			Value value = m_value;
			if (value == Value.EN_US || value == Value.FR_FR || value == Value.ES_ES)
			{
				return FontLanguage.Latin;
			}
			throw new ArgumentOutOfRangeException("m_value", m_value, null);
		}

		[Pure]
		public IPluralRules GetPluralRules()
		{
			switch (m_value)
			{
			case Value.FR_FR:
				return new PluralRulesFR();
			case Value.EN_US:
				return new PluralRulesEN();
			case Value.ES_ES:
				return new PluralRulesES();
			default:
				throw new ArgumentOutOfRangeException("m_value", m_value, null);
			}
		}

		[Pure]
		public string GetLanguage()
		{
			switch (m_value)
			{
			case Value.FR_FR:
				return "fr";
			case Value.EN_US:
				return "en";
			case Value.ES_ES:
				return "es";
			default:
				throw new ArgumentOutOfRangeException("m_value", m_value, null);
			}
		}

		[Pure]
		public static CultureCode GetCultureCodeByLanguage(string language)
		{
			if (!(language == "fr"))
			{
				if (!(language == "en"))
				{
					if (!(language == "es"))
					{
						if (!(language == "fr-FR"))
						{
							if (!(language == "en-US"))
							{
								if (language == "es-ES")
								{
									return ES_ES;
								}
								throw new ArgumentException(language + " is not a valid language");
							}
							return EN_US;
						}
						return FR_FR;
					}
					return ES_ES;
				}
				return EN_US;
			}
			return FR_FR;
		}

		[Pure]
		public override string ToString()
		{
			switch (m_value)
			{
			case Value.FR_FR:
				return "fr-FR";
			case Value.EN_US:
				return "en-US";
			case Value.ES_ES:
				return "es-ES";
			default:
				return string.Empty;
			}
		}

		[Pure]
		public bool IsValid()
		{
			foreach (CultureCode item in EnumerateAvailableCultureCodes())
			{
				if (item.m_value == m_value)
				{
					return true;
				}
			}
			return false;
		}

		[Pure]
		public static IEnumerable<CultureCode> EnumerateAvailableCultureCodes()
		{
			yield return EN_US;
			yield return FR_FR;
			yield return ES_ES;
		}

		[Pure]
		public static bool TryParse([NotNull] string value, out CultureCode result)
		{
			foreach (CultureCode item in EnumerateAvailableCultureCodes())
			{
				if (item.ToString().Equals(value, StringComparison.OrdinalIgnoreCase))
				{
					result = item;
					return true;
				}
			}
			result = default(CultureCode);
			return false;
		}

		[Pure]
		public bool Equals(CultureCode other)
		{
			return m_value == other.m_value;
		}

		[Pure]
		public override bool Equals(object obj)
		{
			object obj2;
			if ((obj2 = obj) is CultureCode)
			{
				CultureCode cultureCode = (CultureCode)obj2;
				return cultureCode.m_value == m_value;
			}
			return false;
		}

		[Pure]
		public static bool operator ==(CultureCode a, CultureCode b)
		{
			return a.m_value == b.m_value;
		}

		[Pure]
		public static bool operator !=(CultureCode a, CultureCode b)
		{
			return a.m_value != b.m_value;
		}

		[Pure]
		public static explicit operator CultureCode(int value)
		{
			return new CultureCode((Value)value);
		}

		[Pure]
		public static explicit operator int(CultureCode value)
		{
			return (int)value.m_value;
		}

		[Pure]
		public override int GetHashCode()
		{
			return (int)m_value;
		}
	}
}
