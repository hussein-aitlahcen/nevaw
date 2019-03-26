using JetBrains.Annotations;
using UnityEngine;

namespace Ankama.Cube.UI.Components
{
	[ExecuteInEditMode]
	public sealed class RawTextField : AbstractTextField
	{
		[UsedImplicitly]
		[SerializeField]
		private string m_formattedText = string.Empty;

		protected override string GetFormattedText()
		{
			return m_formattedText;
		}

		public void SetText(string text)
		{
			m_formattedText = text;
			RefreshText();
		}
	}
}
