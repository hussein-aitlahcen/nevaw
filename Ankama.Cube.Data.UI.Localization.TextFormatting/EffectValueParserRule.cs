using Ankama.Cube.Data.UI.Localization.TextFormatting.ValueProviders.FightValueProviders;
using Ankama.Cube.Utility;
using Ankama.Utilities;
using System;
using System.Text;

namespace Ankama.Cube.Data.UI.Localization.TextFormatting
{
	public class EffectValueParserRule : IParserRuleWithPrefix, IParserRule
	{
		protected const string BonusColorPrefix = "<color=#008c00ff>";

		protected const string MalusColorPrefix = "<color=#d90000ff>";

		protected const string ColorPostfix = "</color>";

		private readonly string text;

		private bool m_initialized;

		private SubString rawText;

		public Func<IFightValueProvider, int> getModificationValue;

		public string prefix => text;

		public EffectValueParserRule(string text)
		{
			this.text = text;
			RuntimeData.CultureCodeChanged += delegate
			{
				ReloadRawText();
			};
		}

		private void ReloadRawText()
		{
			string text = this.text.ToUpper();
			if (!RuntimeData.TryGetText(text, out string value))
			{
				Log.Error("Text key with name " + text + " does not exist.", 114, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\UI\\Localization\\TextFormatting\\ParserRules\\TooltipParserRules\\EffectValueParserRule.cs");
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
			if (reader.Read(text) && reader.Read(':'))
			{
				if (!m_initialized)
				{
					ReloadRawText();
				}
				string varName = reader.ReadWord().ToString();
				FormatValue(varName, output, formatterParams);
				return true;
			}
			return false;
		}

		private void FormatValue(string varName, StringBuilder output, FormatterParams formatterParams)
		{
			IFightValueProvider fightValueProvider = formatterParams.valueProvider as IFightValueProvider;
			if (fightValueProvider != null && getModificationValue != null)
			{
				int num = getModificationValue(fightValueProvider);
				if (num != 0)
				{
					int num2 = fightValueProvider.GetValueInt(varName) + num;
					output.Append((num > 0) ? "<color=#008c00ff>" : "<color=#d90000ff>");
					formatterParams.valueProvider = new IndexedValueProvider(num2.ToString());
					formatterParams.formatter.AppendFormat(rawText, output, formatterParams);
					output.Append("</color>");
					return;
				}
			}
			string value = formatterParams.valueProvider.GetValue(varName);
			formatterParams.valueProvider = new IndexedValueProvider(value);
			formatterParams.formatter.AppendFormat(rawText, output, formatterParams);
		}
	}
}
