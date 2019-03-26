using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ankama.Cube.Demo.UI
{
	public abstract class PanelList : MonoBehaviour
	{
		protected PanelList()
			: this()
		{
		}
	}
	[RequireComponent(typeof(RectTransform))]
	public abstract class PanelList<T> : PanelList where T : Panel
	{
		private struct ElementState
		{
			public Vector3 pos;

			public int slibingIndex;

			public float visibility;

			public ElementState(Vector3 pos, int slibingIndex, float visibility)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				this.pos = pos;
				this.slibingIndex = slibingIndex;
				this.visibility = visibility;
			}
		}

		[SerializeField]
		private PanelListConfig m_config;

		[SerializeField]
		private Button m_leftArrow;

		[SerializeField]
		private Button m_rightArrow;

		private List<ElementState> m_elementsStates = new List<ElementState>();

		private List<T> m_elements = new List<T>();

		private List<Tween> m_tweens = new List<Tween>();

		private int m_selectedIndex = -1;

		private Sequence m_transitionTweenSequence;

		private SlidingAnimUI m_slidingAnim;

		public Action<int> onElementSelected;

		public int selectedIndex => m_selectedIndex;

		public Button rightButton => m_rightArrow;

		public Button leftButton => m_leftArrow;

		public int lockedLeft
		{
			get;
			set;
		}

		public int lockedright
		{
			get;
			set;
		}

		public int elementWidth
		{
			get;
			set;
		}

		protected void OnEnable()
		{
			this.StartCoroutine(DelayUpdateElements());
		}

		private unsafe void Start()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Expected O, but got Unknown
			m_leftArrow.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_rightArrow.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		}

		private void OnRectTransformDimensionsChange()
		{
			UpdateElements(tween: false);
		}

		private void OnArrowClicked(int direction)
		{
			SetSelectedIndex(m_selectedIndex + direction, tween: true, selectCallback: true);
		}

		public void SetSelectedIndex(int index, bool tween, bool selectCallback)
		{
			if (m_elements.Count == 0)
			{
				UpdateArrowState();
				return;
			}
			m_selectedIndex = index;
			UpdateArrowState();
			UpdateElements(tween);
			if (selectCallback)
			{
				onElementSelected?.Invoke(m_selectedIndex);
			}
		}

		private void UpdateArrowState()
		{
			m_leftArrow.get_gameObject().SetActive(m_selectedIndex > 0 && m_selectedIndex > lockedLeft);
			m_rightArrow.get_gameObject().SetActive(m_selectedIndex < m_elements.Count - 1 && m_selectedIndex < m_elements.Count - 1 - lockedright);
		}

		private IEnumerator DelayUpdateElements()
		{
			yield return null;
			UpdateElements(tween: false);
		}

		private unsafe void UpdateElements(bool tween)
		{
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			if (!this.get_gameObject().get_activeInHierarchy() || m_elements.Count == 0 || m_selectedIndex == -1)
			{
				return;
			}
			UpdateElementsState(m_selectedIndex);
			for (int i = 0; i < m_tweens.Count; i++)
			{
				Tween val = m_tweens[i];
				if (TweenExtensions.IsActive(val))
				{
					TweenExtensions.Kill(val, false);
				}
			}
			m_tweens.Clear();
			for (int j = 0; j < m_elements.Count; j++)
			{
				T element = m_elements[j];
				ElementState elementState = m_elementsStates[j];
				if (tween)
				{
					float selectionTweenDuration = m_config.selectionTweenDuration;
					Ease selectionTweenEase = m_config.selectionTweenEase;
					element.get_transform().SetSiblingIndex(elementState.slibingIndex);
					m_tweens.Add(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMove(element.get_transform(), elementState.pos, selectionTweenDuration, false), selectionTweenEase));
					float currentFactor = element.GetVisibilityFactor();
					_003C_003Ec__DisplayClass36_1 _003C_003Ec__DisplayClass36_;
					_003C_003Ec__DisplayClass36_0 _003C_003Ec__DisplayClass36_2;
					m_tweens.Add(TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(new DOGetter<float>((object)_003C_003Ec__DisplayClass36_, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<float>((object)_003C_003Ec__DisplayClass36_2, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), elementState.visibility, selectionTweenDuration), selectionTweenEase));
				}
				else
				{
					element.get_transform().SetSiblingIndex(elementState.slibingIndex);
					element.get_transform().set_localPosition(elementState.pos);
					element.SetVisibilityFactor(elementState.visibility, m_config);
				}
			}
		}

		private void UpdateElementsState(int selectedIndex)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			RectTransform val = this.get_transform() as RectTransform;
			Rect rect = val.get_rect();
			Vector3 val2 = default(Vector3);
			val2._002Ector(rect.get_xMin() + (float)elementWidth / 2f - (float)m_config.leftOffset, 0f, 0f);
			rect = val.get_rect();
			Vector3 val3 = default(Vector3);
			val3._002Ector(rect.get_xMax() - (float)elementWidth / 2f + (float)m_config.rightOffset, 0f, 0f);
			int count = m_elementsStates.Count;
			int num = (selectedIndex != count - 1) ? (count - 1 - selectedIndex) : 0;
			int num2 = 0;
			for (int i = 0; i < selectedIndex; i++)
			{
				m_elementsStates[i] = new ElementState(Vector3.Lerp(val2, Vector3.get_zero(), m_config.elementRepartition.Evaluate((float)(i + 1) / (float)(selectedIndex + 1))), num2, 1f - (float)(selectedIndex - i) / (float)count);
				num2++;
			}
			for (int j = 0; j < num; j++)
			{
				int index = count - 1 - j;
				m_elementsStates[index] = new ElementState(Vector3.Lerp(val3, Vector3.get_zero(), m_config.elementRepartition.Evaluate((float)(j + 1) / (float)(num + 1))), num2, 1f - (float)(num - j) / (float)count);
				num2++;
			}
			m_elementsStates[selectedIndex] = new ElementState(Vector3.get_zero(), num2, 1f);
		}

		public void Add(T element)
		{
			element.get_transform().SetParent(this.get_transform(), false);
			m_elements.Add(element);
			m_elementsStates.Add(default(ElementState));
		}

		public Sequence TransitionAnim(bool open, SlidingSide side)
		{
			if (m_slidingAnim.elements == null)
			{
				m_slidingAnim.elements = new List<CanvasGroup>();
			}
			m_slidingAnim.elements.Clear();
			m_slidingAnim.elements.Add(m_elements[m_selectedIndex].transitionCanvasGroup);
			for (int i = 1; i < m_elements.Count; i++)
			{
				int num = m_selectedIndex - i;
				if (num >= 0 && num < m_elements.Count)
				{
					m_slidingAnim.elements.Add(m_elements[num].transitionCanvasGroup);
				}
				int num2 = m_selectedIndex + i;
				if (num2 >= 0 && num2 < m_elements.Count)
				{
					m_slidingAnim.elements.Add(m_elements[num2].transitionCanvasGroup);
				}
			}
			m_slidingAnim.closeConfig = m_config.closeAnim;
			m_slidingAnim.openConfig = m_config.openAnim;
			return m_slidingAnim.PlayAnim(open, side);
		}
	}
}
