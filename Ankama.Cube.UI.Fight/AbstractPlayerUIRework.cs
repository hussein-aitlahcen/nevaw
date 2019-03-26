using Ankama.AssetManagement.AssetReferences;
using Ankama.Cube.Data;
using Ankama.Cube.Fight.CostModifiers;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Protocols.FightCommonProtocol;
using Ankama.Cube.UI.Components;
using Ankama.Utilities;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Fight
{
	public abstract class AbstractPlayerUIRework : MonoBehaviour
	{
		[SerializeField]
		private RawTextField m_playerName;

		[SerializeField]
		private Image m_rankIcon;

		[SerializeField]
		private ImageLoader m_heroIllustrationLoader;

		[SerializeField]
		private HeroLifeBarRework m_heroLifeBar;

		[SerializeField]
		private ActionPointCounterRework m_actionPointCounter;

		[SerializeField]
		private ElementaryPointsCounterRework m_elementaryPointsCounter;

		[SerializeField]
		protected ReservePointCounterRework m_reservePointCounter;

		private readonly Dictionary<int, SpellCostModification> m_spellCostModifiers = new Dictionary<int, SpellCostModification>();

		protected PlayerStatus m_playerStatus;

		public virtual void SetPlayerStatus(PlayerStatus playerStatus)
		{
			m_playerStatus = playerStatus;
		}

		public void SetPlayerName([NotNull] string value)
		{
			m_playerName.SetText(value);
		}

		public void SetRankIcon(int rank)
		{
			m_rankIcon.set_enabled(false);
		}

		public void SetHeroIllustration(WeaponDefinition definition, Gender gender)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			AssetReference illustrationReference = definition.GetIllustrationReference(gender);
			string bundleName = "core/ui/characters/heroes";
			m_heroIllustrationLoader.Setup(illustrationReference, bundleName);
		}

		public void SetHeroStartLifePoints(int value, PlayerType playerType)
		{
			if (null != m_heroLifeBar)
			{
				m_heroLifeBar.SetStartLife(value, playerType);
			}
		}

		public void ChangeHeroBaseLifePoints(int value)
		{
			if (null != m_heroLifeBar)
			{
				m_heroLifeBar.ChangeBaseLife(value);
			}
		}

		public void ChangeHeroLifePoints(int value)
		{
			if (null != m_heroLifeBar)
			{
				m_heroLifeBar.ChangeLife(value);
			}
		}

		public virtual void RefreshAvailableActions(bool recomputeSpellCosts)
		{
		}

		public virtual IEnumerator UpdateAvailableActions(bool recomputeSpellCosts)
		{
			yield break;
		}

		public void SetActionPoints(int value)
		{
			if (null != m_actionPointCounter)
			{
				m_actionPointCounter.SetValue(value);
			}
		}

		public void ChangeActionPoints(int value)
		{
			if (null != m_actionPointCounter)
			{
				m_actionPointCounter.ChangeValue(value);
			}
		}

		public void PreviewActionPoints(int value, ValueModifier modifier)
		{
			if (null != m_actionPointCounter)
			{
				m_actionPointCounter.ShowPreview(value, modifier);
			}
		}

		public void HidePreviewActionPoints(bool cancelled)
		{
			if (null != m_actionPointCounter)
			{
				m_actionPointCounter.HidePreview(cancelled);
			}
		}

		public void SetupReserve(HeroStatus heroStatus, ReserveDefinition definition)
		{
			if (null != m_reservePointCounter)
			{
				m_reservePointCounter.Setup(heroStatus, definition);
			}
		}

		public void SetReservePoints(int value)
		{
			if (null != m_reservePointCounter)
			{
				m_reservePointCounter.SetValue(value);
			}
		}

		public virtual void ChangeReservePoints(int value)
		{
			if (null != m_reservePointCounter)
			{
				m_reservePointCounter.ChangeValue(value);
			}
		}

		public void PreviewReservePoints(int value, ValueModifier modifier)
		{
			if (null != m_reservePointCounter)
			{
				m_reservePointCounter.ShowPreview(value, modifier);
			}
		}

		public void HidePreviewReservePoints(bool cancelled)
		{
			if (null != m_reservePointCounter)
			{
				m_reservePointCounter.HidePreview(cancelled);
			}
		}

		public void SetElementaryPoints(int air, int earth, int fire, int water)
		{
			if (null != m_elementaryPointsCounter)
			{
				m_elementaryPointsCounter.SetValues(air, earth, fire, water);
			}
		}

		public void ChangeAirElementaryPoints(int value)
		{
			if (null != m_elementaryPointsCounter)
			{
				m_elementaryPointsCounter.ChangeAirValue(value);
			}
		}

		public void ShowPreviewAir(int value, ValueModifier modifier)
		{
			if (null != m_elementaryPointsCounter)
			{
				m_elementaryPointsCounter.ShowPreviewAir(value, modifier);
			}
		}

		public void HidePreviewAir(bool cancelled)
		{
			if (null != m_elementaryPointsCounter)
			{
				m_elementaryPointsCounter.HidePreviewAir(cancelled);
			}
		}

		public void ChangeEarthElementaryPoints(int value)
		{
			if (null != m_elementaryPointsCounter)
			{
				m_elementaryPointsCounter.ChangeEarthValue(value);
			}
		}

		public void ShowPreviewEarth(int value, ValueModifier modifier)
		{
			if (null != m_elementaryPointsCounter)
			{
				m_elementaryPointsCounter.ShowPreviewEarth(value, modifier);
			}
		}

		public void HidePreviewEarth(bool cancelled)
		{
			if (null != m_elementaryPointsCounter)
			{
				m_elementaryPointsCounter.HidePreviewEarth(cancelled);
			}
		}

		public void ChangeFireElementaryPoints(int value)
		{
			if (null != m_elementaryPointsCounter)
			{
				m_elementaryPointsCounter.ChangeFireValue(value);
			}
		}

		public void ShowPreviewFire(int value, ValueModifier modifier)
		{
			if (null != m_elementaryPointsCounter)
			{
				m_elementaryPointsCounter.ShowPreviewFire(value, modifier);
			}
		}

		public void HidePreviewFire(bool cancelled)
		{
			if (null != m_elementaryPointsCounter)
			{
				m_elementaryPointsCounter.HidePreviewFire(cancelled);
			}
		}

		public void ChangeWaterElementaryPoints(int value)
		{
			if (null != m_elementaryPointsCounter)
			{
				m_elementaryPointsCounter.ChangeWaterValue(value);
			}
		}

		public void ShowPreviewWater(int value, ValueModifier modifier)
		{
			if (null != m_elementaryPointsCounter)
			{
				m_elementaryPointsCounter.ShowPreviewWater(value, modifier);
			}
		}

		public void HidePreviewWater(bool cancelled)
		{
			if (null != m_elementaryPointsCounter)
			{
				m_elementaryPointsCounter.HidePreviewWater(cancelled);
			}
		}

		public abstract void SetUIInteractable(bool interactable);

		public abstract void AddSpellStatus(SpellInfo spellInfo, int index);

		public abstract void RemoveSpellStatus(int spellInstanceId, int index);

		public abstract IEnumerator AddSpell(SpellInfo spellInfo, int index);

		public abstract IEnumerator RemoveSpell(int spellInstanceId, int index);

		public virtual IEnumerator AddSpellCostModifier(SpellCostModification spellCostModifier)
		{
			int id = spellCostModifier.id;
			if (m_spellCostModifiers.ContainsKey(id))
			{
				Log.Error($"Tried to add spell cost modifier with id {id} multiple times.", 315, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\FightRework\\PlayerUI\\AbstractPlayerUIRework.cs");
			}
			else
			{
				m_spellCostModifiers.Add(spellCostModifier.id, spellCostModifier);
			}
			yield break;
		}

		public virtual IEnumerator RemoveSpellCostModifier(int spellCostModifierId)
		{
			if (!m_spellCostModifiers.Remove(spellCostModifierId))
			{
				Log.Error($"Tried to remove spell cost modifier with id {spellCostModifierId} but it does not exist.", 326, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\FightRework\\PlayerUI\\AbstractPlayerUIRework.cs");
			}
			yield break;
		}

		public abstract void RefreshAvailableCompanions();

		public abstract IEnumerator UpdateAvailableCompanions();

		public abstract void AddCompanionStatus(int companionDefinitionId, int level, int index);

		public abstract void AddAdditionalCompanionStatus(PlayerStatus owner, int companionDefinitionId, int level);

		public abstract void RemoveAdditionalCompanionStatus(int companionDefinitionId);

		public abstract void ChangeCompanionStateStatus(int companionDefinitionId, CompanionReserveState state);

		public abstract IEnumerator AddCompanion(int companionDefinitionId, int level, int index);

		public abstract IEnumerator AddAdditionalCompanion(PlayerStatus owner, int companionDefinitionId, int level);

		public abstract IEnumerator RemoveAdditionalCompanion(int companionDefinitionId);

		public abstract IEnumerator ChangeCompanionState(int companionDefinitionId, CompanionReserveState state);

		public void HideAllPreviews(bool cancelled)
		{
			if (null != m_actionPointCounter)
			{
				m_actionPointCounter.HidePreview(cancelled);
			}
			if (null != m_reservePointCounter)
			{
				m_reservePointCounter.HidePreview(cancelled);
			}
			if (null != m_elementaryPointsCounter)
			{
				m_elementaryPointsCounter.HidePreviewAir(cancelled);
				m_elementaryPointsCounter.HidePreviewFire(cancelled);
				m_elementaryPointsCounter.HidePreviewEarth(cancelled);
				m_elementaryPointsCounter.HidePreviewWater(cancelled);
			}
		}

		protected AbstractPlayerUIRework()
			: this()
		{
		}
	}
}
