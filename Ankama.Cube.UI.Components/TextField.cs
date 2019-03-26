using Ankama.Cube.Data.UI.Localization.TextFormatting;
using Ankama.Utilities;
using UnityEngine;

namespace Ankama.Cube.UI.Components
{
	[ExecuteInEditMode]
	public sealed class TextField : AbstractTextField
	{
		[SerializeField]
		[TextKey]
		private int m_textKeyId;

		[SerializeField]
		private bool m_requiresValueProvider;

		private IValueProvider m_valueProvider;

		protected override string GetFormattedText()
		{
			if (m_textKeyId == 0 || (m_requiresValueProvider && m_valueProvider == null))
			{
				return string.Empty;
			}
			return RuntimeData.FormattedText(m_textKeyId, m_valueProvider);
		}

		public void SetText(int textKeyId, IValueProvider valueProvider = null)
		{
			m_textKeyId = textKeyId;
			m_valueProvider = valueProvider;
			RefreshText();
		}

		public void SetText(string textKeyName, IValueProvider valueProvider = null)
		{
			if (!RuntimeData.TryGetTextKeyId(textKeyName, out int id))
			{
				Log.Warning("Could not found a text key named '" + textKeyName + "'.", 60, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\Components\\TextField.cs");
				m_textKeyId = 0;
				m_valueProvider = null;
			}
			else
			{
				SetText(id, valueProvider);
			}
		}
	}
}
