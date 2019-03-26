using Ankama.Cube.Data;
using Ankama.Cube.Data.UI.Localization.TextFormatting.ValueProviders.FightValueProviders;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.UI.Components;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Fight
{
	public sealed class ReservePointCounterRework : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, ITextTooltipDataProvider, ITooltipDataProvider
	{
		[SerializeField]
		private PointCounterRework m_counter;

		[SerializeField]
		private TooltipPosition m_tooltipPosition;

		[SerializeField]
		private Button m_button;

		[SerializeField]
		private ParticleSystem m_paUpFx;

		[SerializeField]
		private ParticleSystem m_paDownFx;

		private ReserveDefinition m_reserveDefinition;

		private DynamicFightValueProvider m_fightValueProvider;

		private bool m_uiIsInteractable;

		public TooltipDataType tooltipDataType => TooltipDataType.Text;

		public KeywordReference[] keywordReferences
		{
			get
			{
				if (!(null != m_reserveDefinition))
				{
					return new KeywordReference[0];
				}
				return m_reserveDefinition.precomputedData.keywordReferences;
			}
		}

		public event Action OnReserveActivation;

		public void Setup(HeroStatus heroStatus, ReserveDefinition reserveDefinition)
		{
			m_reserveDefinition = reserveDefinition;
			m_fightValueProvider = new DynamicFightValueProvider(heroStatus, heroStatus.level);
		}

		public void SetInteractable(bool interactable)
		{
			m_uiIsInteractable = interactable;
			RefreshUsability();
		}

		public void SetValue(int value)
		{
			if (null != m_counter)
			{
				m_counter.SetValue(value);
			}
			RefreshUsability();
		}

		public void ChangeValue(int value)
		{
			if (null != m_counter)
			{
				int targetValue = m_counter.targetValue;
				if (targetValue < value)
				{
					PlayActionPointsUp();
				}
				else if (targetValue > value)
				{
					PlayActionPointsDown();
				}
				m_counter.ChangeValue(value);
			}
			RefreshUsability();
		}

		private unsafe void Awake()
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected O, but got Unknown
			if (null != m_button)
			{
				m_button.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			}
			RefreshUsability();
		}

		private void OnClick()
		{
			if (!DragNDropListener.instance.dragging)
			{
				UIManager instance = UIManager.instance;
				if (!(null != instance) || !instance.userInteractionLocked)
				{
					this.OnReserveActivation?.Invoke();
				}
			}
		}

		private void PlayActionPointsUp()
		{
			if (null != m_paUpFx)
			{
				m_paUpFx.get_gameObject().SetActive(true);
				m_paUpFx.Stop();
				m_paUpFx.Play();
			}
		}

		private void PlayActionPointsDown()
		{
			if (null != m_paDownFx)
			{
				m_paDownFx.get_gameObject().SetActive(true);
				m_paDownFx.Stop();
				m_paDownFx.Play();
			}
		}

		private void RefreshUsability()
		{
			if (null != m_button)
			{
				if (null != m_counter)
				{
					m_button.set_interactable(m_uiIsInteractable && m_counter.targetValue > 0);
				}
				else
				{
					m_button.set_interactable(m_uiIsInteractable);
				}
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			FightUIRework.ShowTooltip(this, m_tooltipPosition, this.GetComponent<RectTransform>());
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			FightUIRework.HideTooltip();
		}

		public void ShowPreview(int value, ValueModifier modifier)
		{
		}

		public void HidePreview(bool cancelled)
		{
		}

		public int GetTitleKey()
		{
			if (!(null != m_reserveDefinition))
			{
				return 0;
			}
			return m_reserveDefinition.i18nNameId;
		}

		public int GetDescriptionKey()
		{
			if (!(null != m_reserveDefinition))
			{
				return 0;
			}
			return m_reserveDefinition.i18nDescriptionId;
		}

		public IFightValueProvider GetValueProvider()
		{
			return m_fightValueProvider;
		}

		public ReservePointCounterRework()
			: this()
		{
		}
	}
}
