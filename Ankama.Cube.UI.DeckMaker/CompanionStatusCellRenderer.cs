using Ankama.Cube.Data;
using Ankama.Cube.Protocols.FightCommonProtocol;
using Ankama.Cube.UI.Components;
using Ankama.Cube.UI.Fight;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ankama.Cube.UI.DeckMaker
{
	public class CompanionStatusCellRenderer : CompanionCellRenderer<ReserveCompanionStatus, ICompanionStatusCellRendererConfigurator>
	{
		[SerializeField]
		private GameObject m_highlight;

		[SerializeField]
		private Image m_deadIcon;

		[SerializeField]
		private Image m_givenIcon;

		[SerializeField]
		private Image m_inFightIcon;

		[SerializeField]
		protected CastableDnd m_castableDnd;

		[Header("Audio Events")]
		[SerializeField]
		private UnityEvent m_onBeginDrag;

		[SerializeField]
		private UnityEvent m_onCancelDrag;

		[SerializeField]
		private UnityEvent m_onEnterTarget;

		[SerializeField]
		private UnityEvent m_onExitTarget;

		[SerializeField]
		private UnityEvent m_onBecameAvailable;

		private CompanionStatusData? m_statusData;

		private IDragNDropValidator m_dragNDropValidator;

		protected override IReadOnlyList<Cost> GetCosts()
		{
			return m_statusData?.cost;
		}

		protected override bool IsAvailable()
		{
			if ((m_statusData?.hasResources ?? true) && (m_statusData?.state ?? CompanionReserveState.Idle) == CompanionReserveState.Idle)
			{
				return m_configurator?.IsParentInteractable() ?? true;
			}
			return false;
		}

		private void Awake()
		{
			if (Object.op_Implicit(m_castableDnd))
			{
				m_castableDnd.SkipEndDragEvent = true;
				m_castableDnd.OnDragBegin += OnDragBegin;
				m_castableDnd.OnDragEnd += OnDragEnd;
				m_castableDnd.OnDragBeginRequest += IsDragValid;
				m_castableDnd.castBehaviour = DndCastBehaviour.MoveBack;
			}
		}

		private bool IsDragValid()
		{
			if (m_dragNDropValidator != null)
			{
				return m_dragNDropValidator.IsValidDrag(m_value);
			}
			return true;
		}

		protected override void SetValue(ReserveCompanionStatus val)
		{
			SetValue(val?.definition, val?.level ?? 0);
			SetStateIcon();
			bool flag = IsAvailable();
			m_highlight.SetActive(flag);
			m_castableDnd.enableDnd = flag;
			m_castableDnd.ResetContentPosition();
		}

		private void SetStateIcon()
		{
			CompanionReserveState companionReserveState = m_statusData?.state ?? CompanionReserveState.Idle;
			m_deadIcon.get_gameObject().SetActive(companionReserveState == CompanionReserveState.Dead);
			m_givenIcon.get_gameObject().SetActive(companionReserveState == CompanionReserveState.Given);
			m_inFightIcon.get_gameObject().SetActive(companionReserveState == CompanionReserveState.InFight);
		}

		public override void OnConfiguratorUpdate(bool instant)
		{
			if (m_configurator != null)
			{
				m_statusData = m_configurator.GetCompanionStatusData(m_value);
				m_dragNDropValidator = m_configurator.GetDragNDropValidator();
			}
			else
			{
				m_statusData = null;
				m_dragNDropValidator = null;
			}
			base.OnConfiguratorUpdate(instant);
			SetStateIcon();
			bool flag = IsAvailable();
			if (m_highlight.get_activeSelf() != flag)
			{
				m_highlight.SetActive(flag);
				if (flag)
				{
					m_onBecameAvailable.Invoke();
				}
			}
			m_castableDnd.enableDnd = flag;
		}

		private void OnDragBegin(bool click)
		{
			m_configurator?.GetEventListener()?.CastCompanionDragBegin(this, click);
			m_onBeginDrag.Invoke();
		}

		private void OnDragEnd(bool onTarget)
		{
			m_configurator?.GetEventListener()?.CastCompanionDragEnd(this, onTarget);
		}

		public void OnEnterTarget()
		{
			m_castableDnd.OnEnterTarget();
			m_onEnterTarget.Invoke();
		}

		public void OnExitTarget()
		{
			m_castableDnd.OnExitTarget();
			m_onExitTarget.Invoke();
		}

		public void StartCast()
		{
			m_castableDnd.StartCast();
		}

		public void CancelCast()
		{
			m_castableDnd.CancelCast();
			m_onCancelDrag.Invoke();
			m_onExitTarget.Invoke();
		}

		public void DoneCasting()
		{
			m_castableDnd.DoneCasting();
			m_onExitTarget.Invoke();
		}
	}
}
