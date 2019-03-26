using Ankama.Cube.UI.Components;
using System.Collections;
using UnityEngine;

namespace Ankama.Cube.Render
{
	[ExecuteInEditMode]
	public class SpriteRendererGroup : MonoBehaviour, IUIResourceConsumer
	{
		[SerializeField]
		private float m_apha;

		private SpriteRenderer[] m_renderers;

		private float m_currentAlpha;

		private Coroutine m_appearRoutine;

		private void OnEnable()
		{
			RebuildRenderersArray();
		}

		public void RebuildRenderersArray()
		{
			m_apha = 0f;
			m_renderers = this.GetComponentsInChildren<SpriteRenderer>();
			SetAlpha();
		}

		private void Update()
		{
			if (m_currentAlpha != m_apha)
			{
				SetAlpha();
			}
		}

		private void SetAlpha()
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			m_currentAlpha = m_apha;
			for (int num = m_renderers.Length - 1; num >= 0; num--)
			{
				SpriteRenderer val = m_renderers[num];
				if (!(val == null))
				{
					Color color = val.get_color();
					color.a = m_currentAlpha;
					val.set_color(color);
				}
			}
		}

		public UIResourceDisplayMode Register(IUIResourceProvider provider)
		{
			return UIResourceDisplayMode.Immediate;
		}

		public void UnRegister(IUIResourceProvider provider)
		{
			if (m_appearRoutine != null)
			{
				this.StopCoroutine(m_appearRoutine);
			}
			RebuildRenderersArray();
		}

		public void PlayAppear()
		{
			m_appearRoutine = this.StartCoroutine(Alphatween());
		}

		private IEnumerator Alphatween()
		{
			float step = 4f;
			while (m_apha < 1f)
			{
				m_apha += step * Time.get_deltaTime();
				yield return null;
			}
		}

		public SpriteRendererGroup()
			: this()
		{
		}
	}
}
