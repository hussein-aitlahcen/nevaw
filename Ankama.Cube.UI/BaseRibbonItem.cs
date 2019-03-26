using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ankama.Cube.UI
{
	public class BaseRibbonItem : MonoBehaviour
	{
		protected Toggle m_toggle;

		protected RectTransform m_tickRectTransform;

		protected Vector2 m_defaultTickDelta;

		[SerializeField]
		protected Image m_visual;

		[SerializeField]
		protected GameObject m_equippedSquare;

		[SerializeField]
		protected CanvasGroup m_selectedTicks;

		private bool m_selected;

		protected IEnumerator m_loadingRoutine;

		public unsafe void Initialise()
		{
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			m_toggle = this.GetComponent<Toggle>();
			m_toggle.onValueChanged.AddListener(new UnityAction<bool>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_tickRectTransform = m_selectedTicks.GetComponent<RectTransform>();
			m_defaultTickDelta = m_tickRectTransform.get_sizeDelta();
			m_selectedTicks.get_gameObject().SetActive(false);
		}

		protected virtual IEnumerator LoadAssets()
		{
			yield return null;
		}

		public void ForceSelect()
		{
			if (m_toggle.get_isOn())
			{
				ApplySelect();
			}
			else
			{
				m_toggle.set_isOn(true);
			}
		}

		public void OnClic(bool b)
		{
			if (b)
			{
				ApplySelect();
			}
		}

		protected virtual void ApplySelect()
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			if (!m_selected)
			{
				m_selectedTicks.get_gameObject().SetActive(true);
				m_tickRectTransform.set_sizeDelta(m_defaultTickDelta + new Vector2(0f, 100f));
				m_selectedTicks.set_alpha(0f);
				Sequence obj = DOTween.Sequence();
				TweenSettingsExtensions.Insert(obj, 0f, DOTweenModuleUI.DOFade(m_selectedTicks, 1f, 0.1f));
				TweenSettingsExtensions.Insert(obj, 0f, DOTweenModuleUI.DOSizeDelta(m_tickRectTransform, m_defaultTickDelta, 0.1f, false));
				TweenExtensions.Play<Sequence>(obj);
				m_selected = true;
			}
		}

		public virtual void SetEquiped(bool selected)
		{
			m_equippedSquare.SetActive(selected);
		}

		protected void OnUnselect()
		{
			m_selected = false;
		}

		public IEnumerator GetLoadingRoutine()
		{
			return LoadAssets();
		}

		public BaseRibbonItem()
			: this()
		{
		}
	}
}
