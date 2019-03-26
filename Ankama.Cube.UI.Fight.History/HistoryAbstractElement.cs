using Ankama.Cube.Fight.Entities;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Fight.History
{
	public abstract class HistoryAbstractElement : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		[SerializeField]
		private Image m_back;

		[SerializeField]
		private Image m_illu;

		[SerializeField]
		private RectTransform m_animDummy;

		[SerializeField]
		private CanvasGroup m_canvasGroup;

		[SerializeField]
		private Graphic[] m_tintedGraphics;

		[SerializeField]
		private HistoryData m_data;

		private Coroutine m_loadIlluCoroutine;

		private bool m_illuLoaded;

		private Tweener m_overTween;

		private float m_overFactor;

		private float m_depthFactor = 1f;

		public Action<HistoryAbstractElement> onPointerEnter;

		public Action<HistoryAbstractElement> onPointerExit;

		public CanvasGroup canvasGroup => m_canvasGroup;

		public float depthFactor
		{
			set
			{
				m_depthFactor = value;
				UpdateOverFactor(m_overFactor);
			}
		}

		public abstract HistoryElementType type
		{
			get;
		}

		public abstract ITooltipDataProvider tooltipProvider
		{
			get;
		}

		protected virtual void OnEnable()
		{
			UpdateIllu();
		}

		protected virtual void OnDisable()
		{
			if (m_loadIlluCoroutine != null)
			{
				this.StopCoroutine(m_loadIlluCoroutine);
				m_loadIlluCoroutine = null;
			}
		}

		protected void ApplyIllu(bool isLocalPlayer)
		{
			m_back.set_sprite(isLocalPlayer ? m_data.playerBg : m_data.opponentBg);
			m_illuLoaded = false;
			UpdateIllu();
		}

		private void UpdateIllu()
		{
			if (m_illuLoaded || !this.get_isActiveAndEnabled())
			{
				return;
			}
			if (!HasIllu())
			{
				m_illu.set_sprite(null);
				return;
			}
			if (m_loadIlluCoroutine != null)
			{
				this.StopCoroutine(m_loadIlluCoroutine);
				m_loadIlluCoroutine = null;
				m_illu.set_sprite(null);
			}
			m_loadIlluCoroutine = this.StartCoroutine(LoadIllu(LoadIlluCallback));
		}

		private void LoadIlluCallback(Sprite sprite, string loadedBundleName)
		{
			if (null != m_illu)
			{
				m_illu.set_sprite(sprite);
				m_illuLoaded = true;
			}
			m_loadIlluCoroutine = null;
		}

		public unsafe virtual void OnPointerEnter(PointerEventData eventData)
		{
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			if (m_overTween != null)
			{
				TweenExtensions.Kill(m_overTween, false);
			}
			m_overTween = TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(new DOGetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), 1f, m_data.elementsOverDuration), m_data.elementsOverEase);
			onPointerEnter?.Invoke(this);
		}

		public unsafe virtual void OnPointerExit(PointerEventData eventData)
		{
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			if (m_overTween != null)
			{
				TweenExtensions.Kill(m_overTween, false);
			}
			m_overTween = TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(new DOGetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), 0f, m_data.elementsOverDuration), m_data.elementsOverEase);
			onPointerExit?.Invoke(this);
		}

		private void UpdateOverFactor(float factor)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			m_overFactor = factor;
			m_animDummy.set_anchoredPosition(Vector2.Lerp(Vector2.get_zero(), new Vector2(m_data.elementsOverOffset, 0f), factor));
			Color color = Color.Lerp(Color.Lerp(m_data.elementsDepthColor, Color.get_white(), m_depthFactor), Color.get_white(), factor);
			for (int i = 0; i < m_tintedGraphics.Length; i++)
			{
				m_tintedGraphics[i].set_color(color);
			}
		}

		protected abstract bool HasIllu();

		protected abstract IEnumerator LoadIllu(Action<Sprite, string> loadEndCallback);

		protected HistoryAbstractElement()
			: this()
		{
		}
	}
}
