using UnityEngine;

namespace Ankama.Cube.Maps
{
	public sealed class AreaFeedbackResources : ScriptableObject
	{
		[SerializeField]
		private Color m_localColor = Color.get_white();

		[SerializeField]
		private Color[] m_colors = (Color[])new Color[0];

		public Color localColor => m_localColor;

		public Color[] colors => m_colors;

		public AreaFeedbackResources()
			: this()
		{
		}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)

	}
}
