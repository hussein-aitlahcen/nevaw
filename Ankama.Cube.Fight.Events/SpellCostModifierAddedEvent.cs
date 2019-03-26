using Ankama.Cube.Data;
using Ankama.Cube.Fight.CostModifiers;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.UI.Fight;
using Ankama.Utilities;
using System.Collections;

namespace Ankama.Cube.Fight.Events
{
	public class SpellCostModifierAddedEvent : FightEvent, IRelatedToEntity
	{
		public int concernedEntity
		{
			get;
			private set;
		}

		public int modifierId
		{
			get;
			private set;
		}

		public int modificationValue
		{
			get;
			private set;
		}

		public string spellFiltersJson
		{
			get;
			private set;
		}

		public SpellCostModifierAddedEvent(int eventId, int? parentEventId, int concernedEntity, int modifierId, int modificationValue, string spellFiltersJson)
			: base(FightEventData.Types.EventType.SpellCostModifierAdded, eventId, parentEventId)
		{
			this.concernedEntity = concernedEntity;
			this.modifierId = modifierId;
			this.modificationValue = modificationValue;
			this.spellFiltersJson = spellFiltersJson;
		}

		public SpellCostModifierAddedEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.SpellCostModifierAdded, proto)
		{
			concernedEntity = proto.Int1;
			modifierId = proto.Int2;
			modificationValue = proto.Int3;
			spellFiltersJson = proto.String1;
		}

		public override void UpdateStatus(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out PlayerStatus entityStatus))
			{
				SpellCostModification spellCostModifier = new SpellCostModification(modifierId, modificationValue, spellFiltersJson);
				entityStatus.AddSpellCostModifier(spellCostModifier);
				AbstractPlayerUIRework view = entityStatus.view;
				if (null != view)
				{
					view.RefreshAvailableActions(recomputeSpellCosts: true);
				}
			}
			else
			{
				Log.Error(FightEventErrors.PlayerNotFound(concernedEntity), 27, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\SpellCostModifierAddedEvent.cs");
			}
			FightLogicExecutor.FireUpdateStatus(fightStatus.fightId, EventCategory.SpellCostModification);
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out PlayerStatus entityStatus))
			{
				AbstractPlayerUIRework playerUI = entityStatus.view;
				if (null != playerUI)
				{
					SpellCostModification spellCostModifier = new SpellCostModification(modifierId, modificationValue, spellFiltersJson);
					yield return playerUI.AddSpellCostModifier(spellCostModifier);
					yield return playerUI.UpdateAvailableActions(recomputeSpellCosts: true);
				}
			}
			else
			{
				Log.Error(FightEventErrors.PlayerNotFound(concernedEntity), 48, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\SpellCostModifierAddedEvent.cs");
			}
			FightLogicExecutor.FireUpdateView(fightStatus.fightId, EventCategory.SpellCostModification);
		}
	}
}
