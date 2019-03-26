using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Protocols.FightCommonProtocol;
using Ankama.Cube.UI.Components;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Fight
{
	public sealed class PlayerUIRework : AbstractPlayerUIRework
	{
		[SerializeField]
		private CardNumberCounterRework m_cardNumberCounterRework;

		[SerializeField]
		private ContainerDrawer m_container;

		[SerializeField]
		private Button m_switchContainerInfoButton;

		private unsafe void Awake()
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Expected O, but got Unknown
			if (Object.op_Implicit(m_switchContainerInfoButton))
			{
				m_switchContainerInfoButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			}
		}

		private void OnSwitchContainer()
		{
			if (Object.op_Implicit(m_container))
			{
				m_container.Switch();
			}
		}

		public override void SetUIInteractable(bool interactable)
		{
			if (null != m_reservePointCounter)
			{
				m_reservePointCounter.SetInteractable(interactable: true);
			}
		}

		public override IEnumerator AddSpell(SpellInfo spellInfo, int index)
		{
			if (null != m_cardNumberCounterRework)
			{
				yield return m_cardNumberCounterRework.Increment();
			}
		}

		public override IEnumerator RemoveSpell(int spellInstanceId, int index)
		{
			if (null != m_cardNumberCounterRework)
			{
				yield return m_cardNumberCounterRework.Decrement();
			}
		}

		public override void AddSpellStatus(SpellInfo spellInfo, int index)
		{
		}

		public override void RemoveSpellStatus(int spellInstanceId, int index)
		{
		}

		public override void RefreshAvailableCompanions()
		{
		}

		public override IEnumerator UpdateAvailableCompanions()
		{
			yield break;
		}

		public override void AddCompanionStatus(int companionDefinitionId, int level, int index)
		{
		}

		public override void AddAdditionalCompanionStatus(PlayerStatus owner, int companionDefinitionId, int level)
		{
		}

		public override void ChangeCompanionStateStatus(int companionDefinitionId, CompanionReserveState state)
		{
		}

		public override void RemoveAdditionalCompanionStatus(int companionDefinitionId)
		{
		}

		public override IEnumerator AddCompanion(int companionDefinitionId, int level, int index)
		{
			yield break;
		}

		public override IEnumerator AddAdditionalCompanion(PlayerStatus owner, int companionDefinitionId, int level)
		{
			yield break;
		}

		public override IEnumerator ChangeCompanionState(int companionDefinitionId, CompanionReserveState state)
		{
			yield break;
		}

		public override IEnumerator RemoveAdditionalCompanion(int companionDefinitionId)
		{
			yield break;
		}
	}
}
