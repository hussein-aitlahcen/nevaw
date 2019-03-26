using Ankama.Cube.Data.UI.Localization.TextFormatting.ValueProviders.FightValueProviders;
using Ankama.Cube.Utility;
using Ankama.Utilities;
using System;
using System.Text;

namespace Ankama.Cube.Data.UI.Localization.TextFormatting
{
	public class RangeParserRule : IParserRuleWithPrefix, IParserRule
	{
		private bool m_initialized;

		private SubString rawText;

		public string prefix => "range";

		private void ReloadRawText()
		{
			string text = "RANGE";
			if (!RuntimeData.TryGetText(text, out string value))
			{
				Log.Error("Text key with name " + text + " does not exist.", 59, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\UI\\Localization\\TextFormatting\\ParserRules\\TooltipParserRules\\RangeParserRule.cs");
				rawText = (SubString)text;
			}
			else
			{
				m_initialized = true;
				rawText = (SubString)value;
			}
		}

		public bool TryFormat(ref StringReader reader, StringBuilder output, FormatterParams formatterParams)
		{
			if (reader.Read("range"))
			{
				if (!m_initialized)
				{
					ReloadRawText();
				}
				IFightValueProvider fightValueProvider = formatterParams.valueProvider as IFightValueProvider;
				if (fightValueProvider == null)
				{
					Log.Error("Cannot format Range for a object without range", 80, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\UI\\Localization\\TextFormatting\\ParserRules\\TooltipParserRules\\RangeParserRule.cs");
					return false;
				}
				Tuple<int, int> range = fightValueProvider.GetRange();
				if (range == null)
				{
					Log.Error("Cannot format Range for a object without range", 87, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\UI\\Localization\\TextFormatting\\ParserRules\\TooltipParserRules\\RangeParserRule.cs");
					return false;
				}
				formatterParams.valueProvider = new IndexedValueProvider(range.Item1.ToString(), range.Item2.ToString());
				formatterParams.formatter.AppendFormat(rawText, output, formatterParams);
				return true;
			}
			return false;
		}
	}
}
