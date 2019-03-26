using Ankama.Cube.Data;
using Ankama.Cube.Fight.CostModifiers;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Protocols.FightCommonProtocol;
using Ankama.Cube.States;
using Ankama.Utilities;
using System.Collections;
using UnityEngine;

namespace Ankama.Cube.UI.Fight
{
	public sealed class LocalPlayerUIRework : AbstractPlayerUIRework
	{
		[SerializeField]
		private SpellBarRework m_spellBar;

		[SerializeField]
		private CompanionBarRework m_companionBar;

		private void Awake()
		{
			if (null != m_reservePointCounter)
			{
				m_reservePointCounter.OnReserveActivation += OnReserveActivation;
			}
		}

		private void OnDestroy()
		{
			if (null != m_reservePointCounter)
			{
				m_reservePointCounter.OnReserveActivation -= OnReserveActivation;
			}
		}

		public override void SetPlayerStatus(PlayerStatus playerStatus)
		{
			base.SetPlayerStatus(playerStatus);
			if (null != m_spellBar)
			{
				m_spellBar.SetPlayerStatus(playerStatus);
			}
			if (null != m_companionBar)
			{
				m_companionBar.SetPlayerStatus(playerStatus);
			}
		}

		public override void RefreshAvailableActions(bool recomputeSpellCosts)
		{
			if (null != m_spellBar)
			{
				m_spellBar.RefreshUsability(m_playerStatus, recomputeSpellCosts);
			}
		}

		public override IEnumerator UpdateAvailableActions(bool recomputeSpellCosts)
		{
			if (null != m_spellBar)
			{
				yield return m_spellBar.UpdateUsability(recomputeSpellCosts);
			}
		}

		public override void SetUIInteractable(bool interactable)
		{
			if (null != m_spellBar)
			{
				m_spellBar.SetInteractable(interactable);
			}
			if (null != m_companionBar)
			{
				m_companionBar.SetInteractable(interactable);
			}
			if (null != m_reservePointCounter)
			{
				m_reservePointCounter.SetInteractable(interactable);
			}
		}

		public override void AddSpellStatus(SpellInfo spellInfo, int index)
		{
			if (!(null == m_spellBar))
			{
				SpellStatus spellStatus = SpellStatus.TryCreate(spellInfo, m_playerStatus);
				if (spellStatus != null)
				{
					m_spellBar.AddSpellStatus(spellStatus);
				}
			}
		}

		public override void RemoveSpellStatus(int spellInstanceId, int index)
		{
			if (!(null == m_spellBar))
			{
				m_spellBar.RemoveSpellStatus(spellInstanceId);
			}
		}

		public override IEnumerator AddSpell(SpellInfo spellInfo, int index)
		{
			if (!(null == m_spellBar))
			{
				float num = (float)index * 0.1f;
				if (num > 0f)
				{
					yield return (object)new WaitForTime(num);
				}
				SpellStatus spellStatus = SpellStatus.TryCreate(spellInfo, m_playerStatus);
				if (spellStatus != null)
				{
					yield return m_spellBar.AddSpell(spellStatus);
				}
			}
		}

		public override IEnumerator RemoveSpell(int spellInstanceId, int index)
		{
			if (!(null == m_spellBar))
			{
				float num = (float)index * 0.1f;
				if (num > 0f)
				{
					yield return (object)new WaitForTime(num);
				}
				yield return m_spellBar.RemoveSpell(spellInstanceId);
			}
		}

		public override IEnumerator AddSpellCostModifier(SpellCostModification spellCostModifier)
		{
			yield return base.AddSpellCostModifier(spellCostModifier);
		}

		public override IEnumerator RemoveSpellCostModifier(int spellCostModifierId)
		{
			yield return base.RemoveSpellCostModifier(spellCostModifierId);
		}

		public override void RefreshAvailableCompanions()
		{
			if (null != m_companionBar)
			{
				m_companionBar.RefreshUsability(m_playerStatus);
			}
		}

		public override void AddCompanionStatus(int companionDefinitionId, int level, int index)
		{
			if (!(null == m_companionBar) && RuntimeData.companionDefinitions.TryGetValue(companionDefinitionId, out CompanionDefinition value))
			{
				ReserveCompanionStatus companion = new ReserveCompanionStatus(m_playerStatus, value, level);
				m_companionBar.AddCompanionStatus(m_playerStatus, companion);
			}
		}

		public override void AddAdditionalCompanionStatus(PlayerStatus owner, int companionDefinitionId, int level)
		{
			if (!(null == m_companionBar) && RuntimeData.companionDefinitions.TryGetValue(companionDefinitionId, out CompanionDefinition value))
			{
				ReserveCompanionStatus companion = new ReserveCompanionStatus(owner, value, level);
				m_companionBar.AddCompanionStatus(m_playerStatus, companion);
			}
		}

		public override void ChangeCompanionStateStatus(int companionDefinitionId, CompanionReserveState state)
		{
			if (!(null == m_companionBar))
			{
				m_companionBar.ChangeCompanionStateStatus(m_playerStatus, companionDefinitionId, state);
			}
		}

		public override void RemoveAdditionalCompanionStatus(int companionDefinitionId)
		{
			if (!(null == m_companionBar))
			{
				m_companionBar.RemoveCompanionStatus(companionDefinitionId);
			}
		}

		public override IEnumerator UpdateAvailableCompanions()
		{
			if (!(null == m_companionBar))
			{
				yield return m_companionBar.UpdateAvailableCompanions();
			}
		}

		public override IEnumerator AddCompanion(int companionDefinitionId, int level, int index)
		{
			if (!(null == m_companionBar))
			{
				float num = (float)index * 0.1f;
				if (num > 0f)
				{
					yield return (object)new WaitForTime(num);
				}
				if (RuntimeData.companionDefinitions.TryGetValue(companionDefinitionId, out CompanionDefinition value))
				{
					ReserveCompanionStatus companion = new ReserveCompanionStatus(m_playerStatus, value, level);
					yield return m_companionBar.AddCompanion(companion);
				}
			}
		}

		public override IEnumerator AddAdditionalCompanion(PlayerStatus owner, int companionDefinitionId, int level)
		{
			if (!(null == m_companionBar) && RuntimeData.companionDefinitions.TryGetValue(companionDefinitionId, out CompanionDefinition value))
			{
				ReserveCompanionStatus companion = new ReserveCompanionStatus(owner, value, level);
				yield return m_companionBar.AddCompanion(companion);
			}
		}

		public override IEnumerator ChangeCompanionState(int companionDefinitionId, CompanionReserveState state)
		{
			if (!(null == m_companionBar))
			{
				yield return m_companionBar.ChangeCompanionState(m_playerStatus, companionDefinitionId);
			}
		}

		public override IEnumerator RemoveAdditionalCompanion(int companionDefinitionId)
		{
			if (!(null == m_companionBar))
			{
				yield return m_companionBar.RemoveCompanion(companionDefinitionId);
			}
		}

		private static void OnReserveActivation()
		{
			FightState.instance?.frame?.SendUseReserve();
		}
	}
}
