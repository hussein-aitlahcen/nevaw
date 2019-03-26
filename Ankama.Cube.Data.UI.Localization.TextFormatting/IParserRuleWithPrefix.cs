namespace Ankama.Cube.Data.UI.Localization.TextFormatting
{
	public interface IParserRuleWithPrefix : IParserRule
	{
		string prefix
		{
			get;
		}
	}
}
