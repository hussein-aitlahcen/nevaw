using Ankama.Cube.Data;
using Ankama.Cube.Fight;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.UI.Components;
using Ankama.Cube.UI.Fight.History;
using Ankama.Utilities;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.UI.Fight
{
	public class HistoryUI : MonoBehaviour
	{
		[SerializeField]
		private RectTransform m_window;

		[SerializeField]
		private RectTransform m_container;

		[SerializeField]
		private RectTransform m_enterDummyPos;

		[SerializeField]
		private HistoryAbstractElement[] m_elementPrefabs;

		[SerializeField]
		private HistoryData m_datas;

		[SerializeField]
		private TooltipPosition m_tooltipPosition;

		private readonly List<HistoryAbstractElement> m_elements = new List<HistoryAbstractElement>();

		private readonly List<HistoryAbstractElement> m_displayedElements = new List<HistoryAbstractElement>();

		private unsafe void Start()
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			for (int i = 0; i < m_elementPrefabs.Length; i++)
			{
				m_elementPrefabs[i].get_gameObject().SetActive(false);
			}
			Rect rect = (m_elementPrefabs[0].get_transform() as RectTransform).get_rect();
			float width = rect.get_width();
			float y = ((IntPtr)(void*)m_container.get_sizeDelta()).y;
			m_window.SetSizeWithCurrentAnchors(1, 0f - y + (float)m_datas.maxElements * width + (float)(m_datas.maxElements - 1) * m_datas.spacing);
		}

		public void AddReserveEvent(PlayerStatus status, int valueBefore)
		{
			(EnQueueVisibleElement(HistoryElementType.Reserve) as HistoryReserveElement).Set(status, valueBefore);
		}

		public void AddSpellEvent(SpellStatus status, DynamicValueContext context, int cost)
		{
			(EnQueueVisibleElement(HistoryElementType.Spell) as HistorySpellElement).Set(status, context, cost);
		}

		public void AddCompanionEvent(ReserveCompanionStatus companion)
		{
			(EnQueueVisibleElement(HistoryElementType.Companion) as HistoryCompanionElement).Set(companion);
		}

		private HistoryAbstractElement GetFreeElement(HistoryElementType type)
		{
			for (int i = 0; i < m_elements.Count; i++)
			{
				if (m_elements[i].type == type && !m_elements[i].get_gameObject().get_activeSelf())
				{
					return m_elements[i];
				}
			}
			HistoryAbstractElement historyAbstractElement = null;
			for (int j = 0; j < m_elementPrefabs.Length; j++)
			{
				if (m_elementPrefabs[j].type == type)
				{
					historyAbstractElement = m_elementPrefabs[j];
					break;
				}
			}
			if (historyAbstractElement == null)
			{
				Log.Error($"cannot find prefab for type {type}", 77, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\Fight\\History\\HistoryUI.cs");
			}
			HistoryAbstractElement historyAbstractElement2 = Object.Instantiate<HistoryAbstractElement>(historyAbstractElement);
			HistoryAbstractElement historyAbstractElement3 = historyAbstractElement2;
			historyAbstractElement3.onPointerEnter = (Action<HistoryAbstractElement>)Delegate.Combine(historyAbstractElement3.onPointerEnter, new Action<HistoryAbstractElement>(OnElementPointerEnter));
			HistoryAbstractElement historyAbstractElement4 = historyAbstractElement2;
			historyAbstractElement4.onPointerExit = (Action<HistoryAbstractElement>)Delegate.Combine(historyAbstractElement4.onPointerExit, new Action<HistoryAbstractElement>(OnElementPointerExit));
			historyAbstractElement2.get_transform().SetParent(m_container, false);
			m_elements.Add(historyAbstractElement2);
			return historyAbstractElement2;
		}

		private void OnElementPointerEnter(HistoryAbstractElement e)
		{
			if (FightCastManager.currentCastType == FightCastManager.CurrentCastType.None)
			{
				FightUIRework.ShowTooltip(e.tooltipProvider, m_tooltipPosition, e.GetComponent<RectTransform>());
			}
		}

		private void OnElementPointerExit(HistoryAbstractElement e)
		{
			if (FightCastManager.currentCastType == FightCastManager.CurrentCastType.None)
			{
				FightUIRework.HideTooltip();
			}
		}

		private HistoryAbstractElement EnQueueVisibleElement(HistoryElementType type)
		{
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			HistoryAbstractElement freeElement = GetFreeElement(type);
			freeElement.canvasGroup.set_alpha(1f);
			freeElement.get_transform().SetAsLastSibling();
			freeElement.get_gameObject().SetActive(true);
			(freeElement.get_transform() as RectTransform).set_anchoredPosition(m_enterDummyPos.get_anchoredPosition());
			m_displayedElements.Add(freeElement);
			UpdateElements();
			for (int i = 0; i < m_displayedElements.Count; i++)
			{
				m_displayedElements[i].depthFactor = (float)(i + 1) / (float)m_displayedElements.Count;
			}
			return freeElement;
		}

		private void DeQueueVisibleElement()
		{
			HistoryAbstractElement historyAbstractElement = m_displayedElements[0];
			historyAbstractElement.get_gameObject().SetActive(false);
			historyAbstractElement.get_transform().SetAsFirstSibling();
			m_displayedElements.RemoveAt(0);
		}

		private unsafe void UpdateElements()
		{
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Expected O, but got Unknown
			int num = m_displayedElements.Count - m_datas.maxElements;
			if (num > m_datas.maxHiddableElements)
			{
				int num2 = num - m_datas.maxHiddableElements;
				for (int i = 0; i < num2; i++)
				{
					DeQueueVisibleElement();
				}
			}
			Tweener val = null;
			Vector2 zero = Vector2.get_zero();
			for (int j = 0; j < m_displayedElements.Count; j++)
			{
				HistoryAbstractElement historyAbstractElement = m_displayedElements[m_displayedElements.Count - 1 - j];
				CanvasGroup canvasGroup = historyAbstractElement.canvasGroup;
				RectTransform val2 = historyAbstractElement.get_transform() as RectTransform;
				Rect rect = val2.get_rect();
				float height = rect.get_height();
				ShortcutExtensions.DOKill(canvasGroup, false);
				ShortcutExtensions.DOKill(val2, false);
				val = TweenSettingsExtensions.SetEase<Tweener>(DOTweenModuleUI.DOAnchorPos(val2, zero, m_datas.positionTweenDuration, false), m_datas.postitionTweenEase);
				if (j == 0)
				{
					ShortcutExtensions.DOPunchScale(val2, m_datas.inScalePunchValue, m_datas.inScalePunchDuration, 0, 0f);
				}
				else
				{
					val2.set_localScale(Vector3.get_one());
				}
				if (j < m_datas.maxElements - 1)
				{
					zero.y -= height + m_datas.spacing;
				}
				if (j >= m_datas.maxElements)
				{
					DOTweenModuleUI.DOFade(canvasGroup, 0f, m_datas.outAlphaTweenDuration);
				}
			}
			if (val != null)
			{
				TweenSettingsExtensions.OnComplete<Tweener>(val, new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			}
		}

		private void OnUpdateElementsComplete()
		{
			if (m_displayedElements.Count > m_datas.maxElements)
			{
				int num = m_displayedElements.Count - m_datas.maxElements;
				for (int i = 0; i < num; i++)
				{
					DeQueueVisibleElement();
				}
			}
		}

		public HistoryUI()
			: this()
		{
		}
	}
}
