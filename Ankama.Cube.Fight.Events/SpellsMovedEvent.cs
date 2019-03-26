using Ankama.Cube.Data;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Protocols.FightCommonProtocol;
using Ankama.Cube.UI.Fight;
using Ankama.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Ankama.Cube.Fight.Events
{
	public class SpellsMovedEvent : FightEvent, IRelatedToEntity
	{
		public int concernedEntity
		{
			get;
			private set;
		}

		public IReadOnlyList<SpellMovement> moves
		{
			get;
			private set;
		}

		public SpellsMovedEvent(int eventId, int? parentEventId, int concernedEntity, IReadOnlyList<SpellMovement> moves)
			: base(FightEventData.Types.EventType.SpellsMoved, eventId, parentEventId)
		{
			this.concernedEntity = concernedEntity;
			this.moves = moves;
		}

		public SpellsMovedEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.SpellsMoved, proto)
		{
			concernedEntity = proto.Int1;
			moves = (IReadOnlyList<SpellMovement>)proto.SpellMovementList1;
		}

		public override void UpdateStatus(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out PlayerStatus entityStatus))
			{
				bool flag = fightStatus == FightStatus.local;
				int count = moves.Count;
				for (int i = 0; i < count; i++)
				{
					SpellMovement spellMovement = moves[i];
					if (flag)
					{
						FightCastManager.CheckSpellPlayed(spellMovement.Spell.SpellInstanceId);
					}
					switch (spellMovement.To)
					{
					case Ankama.Cube.Protocols.FightCommonProtocol.SpellMovementZone.Nowhere:
					case Ankama.Cube.Protocols.FightCommonProtocol.SpellMovementZone.Deck:
						if (spellMovement.From == Ankama.Cube.Protocols.FightCommonProtocol.SpellMovementZone.Hand)
						{
							int spellInstanceId = spellMovement.Spell.SpellInstanceId;
							entityStatus.RemoveSpell(spellInstanceId);
							AbstractPlayerUIRework view2 = entityStatus.view;
							if (null != view2)
							{
								view2.RemoveSpellStatus(spellInstanceId, i);
							}
						}
						break;
					case Ankama.Cube.Protocols.FightCommonProtocol.SpellMovementZone.Hand:
					{
						SpellInfo spell = spellMovement.Spell;
						SpellStatus spellStatus = SpellStatus.TryCreate(spell, entityStatus);
						if (spellStatus != null)
						{
							entityStatus.AddSpell(spellStatus);
							AbstractPlayerUIRework view = entityStatus.view;
							if (null != view)
							{
								view.AddSpellStatus(spell, i);
							}
						}
						break;
					}
					default:
						throw new ArgumentOutOfRangeException($"Spell moved to unknown zone: {spellMovement.To}");
					}
				}
			}
			else
			{
				Log.Error(FightEventErrors.PlayerNotFound(concernedEntity), 75, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\SpellsMovedEvent.cs");
			}
			FightLogicExecutor.FireUpdateStatus(fightStatus.fightId, EventCategory.SpellsMoved);
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out PlayerStatus entityStatus))
			{
				AbstractPlayerUIRework view = entityStatus.view;
				if (null != view)
				{
					int count = moves.Count;
					IEnumerator[] array = new IEnumerator[count];
					for (int i = 0; i < count; i++)
					{
						SpellMovement spellMovement = moves[i];
						if (spellMovement.From == Ankama.Cube.Protocols.FightCommonProtocol.SpellMovementZone.Hand)
						{
							int spellInstanceId = spellMovement.Spell.SpellInstanceId;
							array[i] = view.RemoveSpell(spellInstanceId, i);
						}
						else if (spellMovement.To == Ankama.Cube.Protocols.FightCommonProtocol.SpellMovementZone.Hand)
						{
							array[i] = view.AddSpell(spellMovement.Spell, i);
						}
					}
					yield return EnumeratorUtility.ParallelRecursiveImmediateSafeExecution(array);
				}
			}
			else
			{
				Log.Error(FightEventErrors.PlayerNotFound(concernedEntity), 112, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\SpellsMovedEvent.cs");
			}
			FightLogicExecutor.FireUpdateView(fightStatus.fightId, EventCategory.SpellsMoved);
		}
	}
}
