using Ankama.Cube.UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace Ankama.Cube.Demo.UI
{
	[RequireComponent(typeof(RectTransform))]
	public class Panel : MonoBehaviour
	{
		[SerializeField]
		protected CanvasGroup m_transitionCanvasGroup;

		[SerializeField]
		protected Image m_illu;

		[SerializeField]
		private Image[] m_shadows;

		[SerializeField]
		private AbstractTextField[] m_texts;

		private float m_visibilityFactor = 1f;

		public CanvasGroup transitionCanvasGroup => m_transitionCanvasGroup;

		public float GetVisibilityFactor()
		{
			return m_visibilityFactor;
		}

		public void SetVisibilityFactor(float value, PanelListConfig config)
		{
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			m_visibilityFactor = Mathf.Clamp01(value);
			float num = config.depthRepartition.Evaluate(m_visibilityFactor);
			float num2 = config.shadowDepthRepartition.Evaluate(m_visibilityFactor);
			m_illu.set_color(new Color(Mathf.Lerp(config.imageDepthDarken, 1f, num), Mathf.Lerp(config.imageDepthDesaturation, 1f, num), 1f, 1f));
			Color color = Color.Lerp(config.textDepthTint, Color.get_white(), num);
			for (int i = 0; i < m_texts.Length; i++)
			{
				m_texts[i].color = color;
			}
			float num3 = Mathf.Lerp(config.shadowDepthAlpha, 1f, num2);
			for (int j = 0; j < m_shadows.Length; j++)
			{
				Image obj = m_shadows[j];
				obj.WithAlpha<Image>(num3);
				obj.get_gameObject().SetActive(num3 > 0.01f);
			}
		}

		public Panel()
			: this()
		{
		}
	}
}
