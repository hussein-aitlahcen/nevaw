using System.Text;

namespace Ankama.Cube.Data.UI.Localization.TextFormatting
{
	public class ObjectReferenceParserRule : IParserRuleWithPrefix, IParserRule
	{
		private readonly string m_text;

		private readonly ObjectReference.Type m_type;

		public string prefix => m_text;

		public ObjectReferenceParserRule(string text, ObjectReference.Type type)
		{
			m_text = text;
			m_type = type;
		}

		public bool TryFormat(ref StringReader reader, StringBuilder output, FormatterParams formatterParams)
		{
			if (reader.Read(m_text) && reader.Read(':'))
			{
				int id = reader.ReadInt();
				output.BeginKeyWord();
				output.Append(GetText(m_type, id));
				output.EndKeyWord();
				return true;
			}
			return false;
		}

		private static string GetText(ObjectReference.Type type, int id)
		{
			IDefinitionWithTooltip @object = ObjectReference.GetObject(type, id);
			if (@object == null)
			{
				return $"{type}:{id}";
			}
			return RuntimeData.FormattedText(@object.i18nNameId);
		}
	}
}
