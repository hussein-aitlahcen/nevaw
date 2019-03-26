using Ankama.Cube.Data;
using Ankama.Cube.Fight.CostModifiers;
using Ankama.Cube.Protocols.FightCommonProtocol;
using Ankama.Cube.UI.Fight;
using Ankama.Utilities;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;

namespace Ankama.Cube.Fight.Entities
{
	public class PlayerStatus : EntityStatus, IEntityWithActionPoints, IEntity, IEntityWithOwner, IEntityWithTeam
	{
		public readonly int fightId;

		public readonly int index;

		public readonly PlayerType playerType;

		public readonly string nickname;

		private CastTargetContext m_currentCastTargetContext;

		private ICastTargetDefinition m_currentCastTargetDef;

		private readonly List<SpellCostModification> m_spellCostModifiers = new List<SpellCostModification>();

		private readonly Dictionary<int, SpellStatus> m_availableSpells = new Dictionary<int, SpellStatus>(5);

		private readonly List<int> m_dirtySpells = new List<int>();

		private readonly List<ReserveCompanionStatus> m_availableCompanions = new List<ReserveCompanionStatus>(4);

		private readonly List<ReserveCompanionStatus> m_additionalCompanions = new List<ReserveCompanionStatus>(4);

		public int teamId
		{
			get;
		}

		public int teamIndex
		{
			get;
		}

		public int ownerId
		{
			get;
		}

		public int actionPoints => GetCarac(CaracId.ActionPoints);

		public int reservePoints => GetCarac(CaracId.ReservePoints);

		public HeroStatus heroStatus
		{
			get;
			set;
		}

		public AbstractPlayerUIRework view
		{
			get;
			set;
		}

		public override EntityType type => EntityType.Player;

		public bool isLocalPlayer => playerType == PlayerType.Player;

		public int spellCount => m_availableSpells.Count;

		public List<SpellCostModification> spellCostModifiers => m_spellCostModifiers;

		public bool reachedMaxNumberOfAdditionalCompanions => m_additionalCompanions.Count >= 4;

		public PlayerStatus(int id, int fightId, int index, int teamId, int teamIndex, string nickname, PlayerType playerType)
			: base(id)
		{
			ownerId = id;
			this.fightId = fightId;
			this.index = index;
			this.teamId = teamId;
			this.teamIndex = teamIndex;
			this.nickname = nickname;
			this.playerType = playerType;
		}

		public IEnumerator<SpellStatus> GetSpellStatusEnumerator()
		{
			foreach (KeyValuePair<int, SpellStatus> availableSpell in m_availableSpells)
			{
				if (!m_dirtySpells.Contains(availableSpell.Key))
				{
					yield return availableSpell.Value;
				}
			}
		}

		public void AddSpell(SpellStatus spellStatus)
		{
			if (m_dirtySpells.Remove(spellStatus.instanceId))
			{
				FightLogicExecutor.NotifySpellReAddedForPlayer(fightId, this);
			}
			else
			{
				m_availableSpells.Add(spellStatus.instanceId, spellStatus);
			}
		}

		public bool TryGetSpell(int spellSpellInstanceId, out SpellStatus spellStatus)
		{
			return m_availableSpells.TryGetValue(spellSpellInstanceId, out spellStatus);
		}

		public void RemoveSpell(int spellInstanceId)
		{
			m_dirtySpells.Add(spellInstanceId);
			FightLogicExecutor.NotifySpellRemovedForPlayer(fightId, this);
		}

		public IEnumerator CleanupDirtySpells(int counter)
		{
			for (int i = 0; i < counter; i++)
			{
				m_availableSpells.Remove(m_dirtySpells[i]);
			}
			m_dirtySpells.RemoveRange(0, counter);
			yield break;
		}

		public IEnumerable<ReserveCompanionStatus> GetAvailableCompanionStatusEnumerator()
		{
			return m_availableCompanions;
		}

		public IEnumerable<ReserveCompanionStatus> GetAdditionalCompanionStatusEnumerator()
		{
			return m_additionalCompanions;
		}

		public void SetAvailableCompanions(IReadOnlyList<int> definitionIds, IReadOnlyList<int> levels)
		{
			int count = definitionIds.Count;
			for (int i = 0; i < count; i++)
			{
				int num = definitionIds[i];
				int level = levels[i];
				if (!RuntimeData.companionDefinitions.TryGetValue(num, out CompanionDefinition value))
				{
					Log.Error(string.Format("Could not find {0} with id {1}.", "CompanionDefinition", num), 168, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Entities\\Status\\PlayerStatus.cs");
					continue;
				}
				ReserveCompanionStatus item = new ReserveCompanionStatus(this, value, level);
				m_availableCompanions.Add(item);
			}
		}

		public bool HasCompanion(int definitionId)
		{
			List<ReserveCompanionStatus> availableCompanions = m_availableCompanions;
			int count = availableCompanions.Count;
			for (int i = 0; i < count; i++)
			{
				if (availableCompanions[i].definition.get_id() == definitionId)
				{
					return true;
				}
			}
			List<ReserveCompanionStatus> additionalCompanions = m_additionalCompanions;
			int count2 = additionalCompanions.Count;
			for (int j = 0; j < count2; j++)
			{
				if (additionalCompanions[j].definition.get_id() == definitionId)
				{
					return true;
				}
			}
			return false;
		}

		public bool TryGetCompanion(int definitionId, out ReserveCompanionStatus companionStatus)
		{
			List<ReserveCompanionStatus> availableCompanions = m_availableCompanions;
			int count = availableCompanions.Count;
			for (int i = 0; i < count; i++)
			{
				ReserveCompanionStatus reserveCompanionStatus = availableCompanions[i];
				if (reserveCompanionStatus.definition.get_id() == definitionId)
				{
					companionStatus = reserveCompanionStatus;
					return true;
				}
			}
			List<ReserveCompanionStatus> additionalCompanions = m_additionalCompanions;
			int count2 = additionalCompanions.Count;
			for (int j = 0; j < count2; j++)
			{
				ReserveCompanionStatus reserveCompanionStatus2 = additionalCompanions[j];
				if (reserveCompanionStatus2.definition.get_id() == definitionId)
				{
					companionStatus = reserveCompanionStatus2;
					return true;
				}
			}
			companionStatus = null;
			return false;
		}

		public void AddAdditionalCompanion([NotNull] ReserveCompanionStatus companion)
		{
			companion.SetCurrentPlayer(this);
			m_additionalCompanions.Add(companion);
		}

		public void SetAdditionalCompanionState(int companionDefinitionId, CompanionReserveState state)
		{
			int count = m_additionalCompanions.Count;
			for (int i = 0; i < count; i++)
			{
				ReserveCompanionStatus reserveCompanionStatus = m_additionalCompanions[i];
				if (reserveCompanionStatus.definition.get_id() == companionDefinitionId)
				{
					reserveCompanionStatus.SetState(state);
					return;
				}
			}
			Log.Error($"Could not change state of an additional companion from player with id {base.id} because it was not in its secondary reserve.", 256, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Entities\\Status\\PlayerStatus.cs");
		}

		public void RemoveAdditionalCompanion(int companionDefinitionId)
		{
			int count = m_additionalCompanions.Count;
			for (int i = 0; i < count; i++)
			{
				if (m_additionalCompanions[i].definition.get_id() == companionDefinitionId)
				{
					m_additionalCompanions.RemoveAt(i);
					return;
				}
			}
			Log.Error($"Tried to remove a companion from player with id {base.id} but it was not in its secondary reserve.", 272, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Entities\\Status\\PlayerStatus.cs");
		}

		public void AddSpellCostModifier(SpellCostModification spellCostModifier)
		{
			m_spellCostModifiers.Add(spellCostModifier);
		}

		public void RemoveSpellCostModifier(int spellCostModifierId)
		{
			int num = 0;
			while (true)
			{
				if (num < m_spellCostModifiers.Count)
				{
					if (m_spellCostModifiers[num].id == spellCostModifierId)
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			m_spellCostModifiers.RemoveAt(num);
		}
	}
}
