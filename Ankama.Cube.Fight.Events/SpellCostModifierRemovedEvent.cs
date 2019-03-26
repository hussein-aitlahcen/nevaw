using Ankama.Cube.Data;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.UI.Fight;
using Ankama.Utilities;
using System.Collections;

namespace Ankama.Cube.Fight.Events
{
	public class SpellCostModifierRemovedEvent : FightEvent, IRelatedToEntity
	{
		public int concernedEntity
		{
			get;
			private set;
		}

		public int stoppedModifierId
		{
			get;
			private set;
		}

		public SpellCostModifierRemovedEvent(int eventId, int? parentEventId, int concernedEntity, int stoppedModifierId)
			: base(FightEventData.Types.EventType.SpellCostModifierRemoved, eventId, parentEventId)
		{
			this.concernedEntity = concernedEntity;
			this.stoppedModifierId = stoppedModifierId;
		}

		public SpellCostModifierRemovedEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.SpellCostModifierRemoved, proto)
		{
			concernedEntity = proto.Int1;
			stoppedModifierId = proto.Int2;
		}

		public override void UpdateStatus(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out PlayerStatus entityStatus))
			{
				entityStatus.RemoveSpellCostModifier(stoppedModifierId);
				AbstractPlayerUIRework view = entityStatus.view;
				if (null != view)
				{
					view.RefreshAvailableActions(recomputeSpellCosts: true);
				}
			}
			else
			{
				Log.Error(FightEventErrors.PlayerNotFound(concernedEntity), 25, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\SpellCostModifierRemovedEvent.cs");
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
					yield return playerUI.RemoveSpellCostModifier(stoppedModifierId);
					yield return playerUI.UpdateAvailableActions(recomputeSpellCosts: true);
				}
			}
			else
			{
				Log.Error(FightEventErrors.PlayerNotFound(concernedEntity), 45, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\SpellCostModifierRemovedEvent.cs");
			}
			FightLogicExecutor.FireUpdateView(fightStatus.fightId, EventCategory.SpellCostModification);
		}
	}
}
