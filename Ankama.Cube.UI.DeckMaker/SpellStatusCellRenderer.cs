using Ankama.Cube.Fight.Entities;
using Ankama.Cube.UI.Components;
using UnityEngine;
using UnityEngine.Events;

namespace Ankama.Cube.UI.DeckMaker
{
	public class SpellStatusCellRenderer : SpellCellRenderer<SpellStatus, ISpellStatusCellRendererConfigurator>
	{
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

		private IDragNDropValidator m_dragNDropValidator;

		private SpellStatusData? m_spellStatusData;

		protected override bool IsAvailable()
		{
			if (m_spellStatusData?.hasEnoughAp ?? true)
			{
				return m_configurator?.IsParentInteractable() ?? true;
			}
			return false;
		}

		protected override int? GetAPCost()
		{
			return m_spellStatusData?.apCost;
		}

		protected override int? GetBaseAPCost()
		{
			return m_spellStatusData?.baseCost;
		}

		private void Awake()
		{
			if (Object.op_Implicit(m_castableDnd))
			{
				m_castableDnd.SkipEndDragEvent = true;
				m_castableDnd.OnDragBeginRequest += IsDragValid;
				m_castableDnd.OnDragBegin += OnDragBegin;
				m_castableDnd.OnDragEnd += OnDragEnd;
				m_castableDnd.castBehaviour = DndCastBehaviour.Stay;
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

		protected override void SetValue(SpellStatus val)
		{
			SetValue(val?.definition, val?.level ?? 0);
			m_castableDnd.enableDnd = IsAvailable();
			m_castableDnd.ResetContentPosition();
		}

		public override void OnConfiguratorUpdate(bool instant)
		{
			if (m_configurator != null)
			{
				m_spellStatusData = m_configurator.GetSpellStatusData(m_value);
				m_dragNDropValidator = m_configurator.GetDragNDropValidator();
			}
			else
			{
				m_spellStatusData = null;
				m_dragNDropValidator = null;
			}
			base.OnConfiguratorUpdate(instant);
			m_castableDnd.enableDnd = IsAvailable();
		}

		private void OnDragBegin(bool click)
		{
			m_configurator?.GetEventListener()?.CastSpellDragBegin(this, click);
			m_onBeginDrag.Invoke();
		}

		private void OnDragEnd(bool onTarget)
		{
			m_configurator?.GetEventListener()?.CastSpellDragEnd(this, onTarget);
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
