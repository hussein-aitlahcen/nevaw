using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Components
{
	public abstract class RotativeList<T, U, W> : MonoBehaviour where T : CellRenderer<U, W>, ListHighlightable where W : ICellRendererConfigurator
	{
		protected struct ElementState
		{
			public Vector3 pos;

			public int siblingIndex;

			public float depth;

			public static ElementState Create(Vector3 pos, int siblingIndex, float depth)
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				ElementState result = default(ElementState);
				result.pos = pos;
				result.siblingIndex = siblingIndex;
				result.depth = depth;
				return result;
			}
		}

		[Header("List config")]
		[SerializeField]
		private T m_prefab;

		[SerializeField]
		private CanvasGroup m_buttonsCanvasGroup;

		[SerializeField]
		private Button m_leftArrowButton;

		[SerializeField]
		private Button m_rightArrowButton;

		[SerializeField]
		private RectTransform m_container;

		[SerializeField]
		private RectTransform m_leftLimit;

		[SerializeField]
		private RectTransform m_rightLimit;

		[SerializeField]
		protected RectTransform m_outPosition;

		[SerializeField]
		protected RotativeListConfig m_config;

		protected readonly List<T> m_elements = new List<T>();

		protected readonly List<U> m_values = new List<U>();

		protected int m_previousSelectedIndex = -1;

		protected int m_selectedIndex = -1;

		protected Sequence m_currentTweenSequence;

		protected ElementState[] m_currentTweenElementStates;

		private ICellRendererConfigurator m_cellRendererConfigurator;

		private bool m_enableScrollButtons;

		public U selectedValue
		{
			get
			{
				if (m_selectedIndex < 0 || m_selectedIndex >= m_values.Count)
				{
					return default(U);
				}
				if (m_selectedIndex < m_elements.Count)
				{
					return (U)m_elements[m_selectedIndex].value;
				}
				return m_values[m_selectedIndex];
			}
		}

		public int selectedIndex => m_selectedIndex;

		public int count => m_values.Count;

		public unsafe bool enableScrollButtons
		{
			get
			{
				return m_enableScrollButtons;
			}
			set
			{
				//IL_0046: Unknown result type (might be due to invalid IL or missing references)
				//IL_0073: Unknown result type (might be due to invalid IL or missing references)
				//IL_0084: Unknown result type (might be due to invalid IL or missing references)
				//IL_008e: Expected O, but got Unknown
				if (m_enableScrollButtons != value)
				{
					m_enableScrollButtons = value;
					m_buttonsCanvasGroup.get_gameObject().SetActive(true);
					if (value)
					{
						TweenSettingsExtensions.SetEase<Tweener>(DOTweenModuleUI.DOFade(m_buttonsCanvasGroup, 1f, m_config.inTweenDuration), m_config.inTweenEase);
					}
					else
					{
						TweenSettingsExtensions.OnKill<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(DOTweenModuleUI.DOFade(m_buttonsCanvasGroup, 0f, m_config.outTweenDuration), m_config.outTweenEase), new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
					}
				}
			}
		}

		public event Action<U> OnSelectionChanged;

		protected unsafe virtual void Start()
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Expected O, but got Unknown
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Expected O, but got Unknown
			m_prefab.get_gameObject().SetActive(false);
			m_buttonsCanvasGroup.set_alpha(0f);
			m_leftArrowButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_rightArrowButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		}

		public void SetCellRendererConfigurator(ICellRendererConfigurator configurator)
		{
			m_cellRendererConfigurator = configurator;
			foreach (T element in m_elements)
			{
				element.SetConfigurator(configurator);
			}
		}

		public void UpdateAllConfigurators(bool instant = false)
		{
			foreach (T element in m_elements)
			{
				element.OnConfiguratorUpdate(instant);
			}
		}

		public void UpdateConfiguratorWithValue(object value, bool instant = false)
		{
			foreach (T element in m_elements)
			{
				element.OnConfiguratorUpdate(instant);
				if (element.value == value)
				{
					element.OnConfiguratorUpdate(instant);
				}
			}
		}

		public virtual void SetValues(IList<U> values, bool scroll = true)
		{
			int i = 0;
			for (int count = values.Count; i < count; i++)
			{
				m_values.Add(values[i]);
			}
			int j = 0;
			for (int count2 = m_values.Count; j < count2; j++)
			{
				T val = CreateElement();
				SetElementValue(m_values[j], val);
				m_elements.Add(val);
			}
			SetSelectedIndex(0, tween: false, force: false, scroll);
		}

		public virtual void SetValueAt(int index, U value)
		{
			if (index >= 0 && index <= m_values.Count)
			{
				m_values[index] = value;
				T element = m_elements[index];
				SetElementValue(value, element);
			}
		}

		public virtual void Insert(int index, U value, bool selectNew = false)
		{
			if (index >= 0 && index <= m_values.Count)
			{
				m_values.Insert(index, value);
				T val = CreateElement();
				SetElementValue(m_values[index], val);
				m_elements.Insert(index, val);
				int index2 = selectNew ? index : m_selectedIndex;
				SetSelectedIndex(index2, tween: true, force: true);
			}
		}

		public virtual void RemoveAt(int index)
		{
			if (index >= 0 && index < m_values.Count)
			{
				DestroyElement(m_elements[index]);
				m_values.RemoveAt(index);
				m_elements.RemoveAt(index);
				index = Mathf.Clamp(index, 0, m_values.Count - 1);
				SetSelectedIndex(index, tween: true, force: true);
			}
		}

		protected abstract void SetElementValue(U value, T element);

		public void SetSelectedValue(U value, bool tween = true, bool scroll = true)
		{
			SetSelectedIndex(IndexByValue(value), tween, force: false, scroll);
		}

		public void SetSelectedIndex(int index, bool tween = true, bool force = false, bool scroll = true)
		{
			if (index >= 0 && index < m_values.Count && (force || index != m_selectedIndex))
			{
				m_previousSelectedIndex = m_selectedIndex;
				m_selectedIndex = index;
				this.OnSelectionChanged?.Invoke(m_values[m_selectedIndex]);
				if (scroll)
				{
					ScrollToIndex(tween);
				}
			}
		}

		private int IndexByValue(U value)
		{
			int i = 0;
			for (int count = m_values.Count; i < count; i++)
			{
				if (m_values[i].Equals(value))
				{
					return i;
				}
			}
			return -1;
		}

		private T CreateElement()
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			T val = Object.Instantiate<T>(m_prefab);
			val.get_transform().SetParent(m_container, false);
			val.get_transform().set_localPosition(m_outPosition.get_localPosition());
			val.get_gameObject().SetActive(true);
			val.SetConfigurator(m_cellRendererConfigurator, andUpdate: false);
			return val;
		}

		private void DestroyElement(T element)
		{
			Object.Destroy(element.get_gameObject());
		}

		private void OnArrowClicked(int direction)
		{
			SetSelectedIndex(m_selectedIndex + direction);
		}

		private void UpdateArrowState()
		{
			if (m_values.Count == 0)
			{
				m_leftArrowButton.get_gameObject().SetActive(false);
				m_rightArrowButton.get_gameObject().SetActive(false);
				return;
			}
			bool active = m_selectedIndex > 0 && (m_config.emptyCellsAreSelectable || m_values[m_selectedIndex - 1] != null);
			bool active2 = m_selectedIndex < m_values.Count - 1 && (m_config.emptyCellsAreSelectable || m_values[m_selectedIndex + 1] != null);
			m_leftArrowButton.get_gameObject().SetActive(active);
			m_rightArrowButton.get_gameObject().SetActive(active2);
		}

		private unsafe void ScrollToIndex(bool useTween)
		{
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			ElementState[] array = ComputeElementStates();
			if (useTween)
			{
				Sequence currentTweenSequence = m_currentTweenSequence;
				if (currentTweenSequence != null)
				{
					TweenExtensions.Kill(currentTweenSequence, false);
				}
				Sequence val = m_currentTweenSequence = DOTween.Sequence();
				m_currentTweenElementStates = array;
				for (int i = 0; i < m_elements.Count; i++)
				{
					T element = m_elements[i];
					ElementState elementState = array[i];
					element.SetHighlightFactor((i == m_selectedIndex) ? 1f : 0f);
					TweenSettingsExtensions.Insert(val, 0f, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMove(element.get_transform(), elementState.pos, m_config.moveTweenDuration, false), m_config.moveTweenEase));
					if (i == m_selectedIndex)
					{
						element.SetVisibilityFactor(m_config.cellVisibilityCurve.Evaluate(elementState.depth));
						continue;
					}
					Sequence obj = val;
					T val2 = element;
					_003C_003Ec__DisplayClass45_0 _003C_003Ec__DisplayClass45_;
					TweenSettingsExtensions.Insert(obj, 0f, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(new DOGetter<float>((object)val2, (IntPtr)(void*)/*OpCode not supported: LdVirtFtn*/), new DOSetter<float>((object)_003C_003Ec__DisplayClass45_, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), elementState.depth, m_config.moveTweenDuration), m_config.moveTweenEase));
				}
				UpdateSiblingIndexes();
			}
			else
			{
				for (int j = 0; j < m_elements.Count; j++)
				{
					T val3 = m_elements[j];
					ElementState elementState2 = array[j];
					val3.get_transform().set_localPosition(elementState2.pos);
					val3.get_transform().SetSiblingIndex(elementState2.siblingIndex);
					val3.SetVisibilityFactor(m_config.cellVisibilityCurve.Evaluate(elementState2.depth));
					val3.SetHighlightFactor(m_config.cellHighlightCurve.Evaluate((j == m_selectedIndex) ? 1f : 0f));
				}
			}
			UpdateArrowState();
		}

		protected void OnTweenComplete()
		{
			m_currentTweenSequence = null;
			UpdateSiblingIndexes();
		}

		private void UpdateSiblingIndexes()
		{
			if (m_currentTweenElementStates != null)
			{
				ElementState[] currentTweenElementStates = m_currentTweenElementStates;
				for (int i = 0; i < m_elements.Count; i++)
				{
					T val = m_elements[i];
					ElementState elementState = currentTweenElementStates[i];
					val.get_transform().SetSiblingIndex(elementState.siblingIndex);
				}
				m_currentTweenElementStates = null;
			}
		}

		public IEnumerator TweenOut()
		{
			Sequence currentTweenSequence = m_currentTweenSequence;
			if (currentTweenSequence != null)
			{
				TweenExtensions.Kill(currentTweenSequence, false);
			}
			Sequence val = TweenOutSequence();
			yield return TweenExtensions.WaitForKill(val);
		}

		protected unsafe virtual Sequence TweenOutSequence()
		{
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Expected O, but got Unknown
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Expected O, but got Unknown
			Sequence val = m_currentTweenSequence = DOTween.Sequence();
			enableScrollButtons = false;
			float num = (m_config.outTweenDelayByElement >= 0f) ? m_config.outTweenDelayByElement : ((0f - m_config.outTweenDelayByElement) * (float)(m_elements.Count - 1));
			for (int i = 0; i < m_elements.Count; i++)
			{
				T element = m_elements[i];
				TweenSettingsExtensions.Insert(val, num, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMove(element.get_transform(), m_outPosition.get_localPosition(), m_config.outTweenDuration, false), m_config.outTweenEase));
				_003C_003Ec__DisplayClass49_0 _003C_003Ec__DisplayClass49_;
				TweenSettingsExtensions.Insert(val, num, TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScaleX(element.get_transform(), m_config.outScale, m_config.outTweenDuration), m_config.outTweenEase), new TweenCallback((object)_003C_003Ec__DisplayClass49_, (IntPtr)(void*)/*OpCode not supported: LdFtn*/)));
				Sequence obj = val;
				float num2 = num;
				T val2 = element;
				TweenSettingsExtensions.Insert(obj, num2, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(new DOGetter<float>((object)val2, (IntPtr)(void*)/*OpCode not supported: LdVirtFtn*/), new DOSetter<float>((object)_003C_003Ec__DisplayClass49_, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), 0f, m_config.outTweenDuration), m_config.outTweenEase));
				num += m_config.outTweenDelayByElement;
			}
			TweenSettingsExtensions.OnKill<Sequence>(val, new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			return val;
		}

		protected unsafe virtual Sequence TweenInSequence()
		{
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			Sequence val = m_currentTweenSequence = DOTween.Sequence();
			enableScrollButtons = true;
			ElementState[] array = m_currentTweenElementStates = ComputeElementStates();
			UpdateSiblingIndexes();
			float num = (m_config.inTweenDelayByElement >= 0f) ? m_config.inTweenDelayByElement : ((0f - m_config.inTweenDelayByElement) * (float)(m_elements.Count - 1));
			for (int i = 0; i < m_elements.Count; i++)
			{
				T element = m_elements[i];
				ElementState elementState = array[i];
				element.get_gameObject().SetActive(true);
				TweenSettingsExtensions.Insert(val, num, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMove(element.get_transform(), elementState.pos, m_config.inTweenDuration, false), m_config.inTweenEase));
				TweenSettingsExtensions.Insert(val, num, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScaleX(element.get_transform(), 1f, m_config.inTweenDuration), m_config.inTweenEase));
				Sequence obj = val;
				float num2 = num;
				T val2 = element;
				_003C_003Ec__DisplayClass50_0 _003C_003Ec__DisplayClass50_;
				TweenSettingsExtensions.Insert(obj, num2, TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(new DOGetter<float>((object)val2, (IntPtr)(void*)/*OpCode not supported: LdVirtFtn*/), new DOSetter<float>((object)_003C_003Ec__DisplayClass50_, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), (i == m_selectedIndex) ? 1f : 0f, m_config.inTweenDuration), m_config.inTweenEase));
				num += m_config.inTweenDelayByElement;
			}
			return val;
		}

		public IEnumerator TweenIn()
		{
			Sequence currentTweenSequence = m_currentTweenSequence;
			if (currentTweenSequence != null)
			{
				TweenExtensions.Kill(currentTweenSequence, false);
			}
			Sequence val = TweenInSequence();
			yield return TweenExtensions.WaitForKill(val);
		}

		private ElementState[] ComputeElementStates()
		{
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			int count = m_elements.Count;
			ElementState[] array = new ElementState[count];
			int selectedIndex = this.selectedIndex;
			int num = (this.selectedIndex != count - 1) ? (count - 1 - this.selectedIndex) : 0;
			int num2 = 0;
			for (int i = 0; i < selectedIndex; i++)
			{
				array[i] = ElementState.Create(Vector3.Lerp(m_leftLimit.get_localPosition(), Vector3.get_zero(), m_config.cellPositionCurve.Evaluate((float)(i + 1) / (float)(selectedIndex + 1))), num2, m_config.cellVisibilityCurve.Evaluate(1f - (float)(selectedIndex - i) / (float)count));
				num2++;
			}
			for (int j = 0; j < num; j++)
			{
				int num3 = count - 1 - j;
				array[num3] = ElementState.Create(Vector3.Lerp(m_rightLimit.get_localPosition(), Vector3.get_zero(), m_config.cellPositionCurve.Evaluate((float)(j + 1) / (float)(num + 1))), num2, m_config.cellVisibilityCurve.Evaluate(1f - (float)(num - j) / (float)count));
				num2++;
			}
			array[this.selectedIndex] = ElementState.Create(Vector3.get_zero(), num2, 1f);
			return array;
		}

		protected RotativeList()
			: this()
		{
		}
	}
}
