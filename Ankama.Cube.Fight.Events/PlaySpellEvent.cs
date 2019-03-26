using Ankama.Cube.Data;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps;
using Ankama.Cube.Maps.Objects;
using Ankama.Cube.Protocols.CommonProtocol;
using Ankama.Cube.UI.Fight;
using Ankama.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Fight.Events
{
	public class PlaySpellEvent : FightEvent, IRelatedToEntity
	{
		public int concernedEntity
		{
			get;
			private set;
		}

		public int spellInstanceId
		{
			get;
			private set;
		}

		public int spellDefId
		{
			get;
			private set;
		}

		public int spellLevel
		{
			get;
			private set;
		}

		public int actionPointsCost
		{
			get;
			private set;
		}

		public IReadOnlyList<CastTarget> targets
		{
			get;
			private set;
		}

		public PlaySpellEvent(int eventId, int? parentEventId, int concernedEntity, int spellInstanceId, int spellDefId, int spellLevel, int actionPointsCost, IReadOnlyList<CastTarget> targets)
			: base(FightEventData.Types.EventType.PlaySpell, eventId, parentEventId)
		{
			this.concernedEntity = concernedEntity;
			this.spellInstanceId = spellInstanceId;
			this.spellDefId = spellDefId;
			this.spellLevel = spellLevel;
			this.actionPointsCost = actionPointsCost;
			this.targets = targets;
		}

		public PlaySpellEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.PlaySpell, proto)
		{
			concernedEntity = proto.Int1;
			spellInstanceId = proto.Int2;
			spellDefId = proto.Int3;
			spellLevel = proto.Int4;
			actionPointsCost = proto.Int5;
			targets = (IReadOnlyList<CastTarget>)proto.CastTargetList1;
		}

		public override void UpdateStatus(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out PlayerStatus entityStatus))
			{
				if (entityStatus.TryGetSpell(spellInstanceId, out SpellStatus spellStatus))
				{
					if (null == spellStatus.definition)
					{
						if (RuntimeData.spellDefinitions.TryGetValue(spellDefId, out SpellDefinition value))
						{
							spellStatus.Upgrade(value, spellLevel);
						}
						else
						{
							Log.Error(FightEventErrors.DefinitionNotFound<SpellDefinition>(spellDefId), 31, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\PlaySpellEvent.cs");
						}
					}
				}
				else
				{
					Log.Error($"Could not find spell with instance id {spellInstanceId} for player with id {concernedEntity}.", 37, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\PlaySpellEvent.cs");
				}
			}
			else
			{
				Log.Error(FightEventErrors.PlayerNotFound(concernedEntity), 42, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\PlaySpellEvent.cs");
			}
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out PlayerStatus playerStatus))
			{
				if (playerStatus.TryGetSpell(spellInstanceId, out SpellStatus spellStatus))
				{
					SpellDefinition definition = spellStatus.definition;
					if (!(null != definition))
					{
						yield break;
					}
					yield return definition.LoadResources();
					ICastTargetDefinition castTarget = definition.castTarget;
					CastTargetContext castTargetContext = castTarget.CreateCastTargetContext(fightStatus, concernedEntity, DynamicValueHolderType.Spell, spellDefId, spellLevel, 0);
					int count = targets.Count;
					for (int j = 0; j < count; j++)
					{
						castTargetContext.SelectTarget(targets[j].ToTarget(fightStatus));
					}
					if (count > 0 && !playerStatus.isLocalPlayer)
					{
						CellObject targetedCell = GetTargetedCell(fightStatus, targets[0]);
						yield return FightUIRework.ShowPlayingSpell(spellStatus, targetedCell);
					}
					List<SpellEffectInstantiationData> spellEffectData = (List<SpellEffectInstantiationData>)definition.spellEffectData;
					int spellEffectCount = spellEffectData.Count;
					if (spellEffectCount > 0)
					{
						List<IEnumerator> routineList = ListPool<IEnumerator>.Get();
						int num;
						for (int i = 0; i < spellEffectCount; i = num)
						{
							SpellEffect spellEffect = definition.GetSpellEffect(i);
							if (!(null == spellEffect))
							{
								SpellEffectInstantiationData spellEffectInstantiationData = spellEffectData[i];
								spellEffectInstantiationData.PreComputeDelayOverDistance(castTargetContext);
								foreach (Vector2Int item3 in spellEffectInstantiationData.EnumerateInstantiationPositions(castTargetContext))
								{
									IEnumerator item = FightSpellEffectFactory.PlaySpellEffect(spellEffect, item3, spellEffectInstantiationData, castTargetContext);
									routineList.Add(item);
								}
								foreach (IsoObject item4 in spellEffectInstantiationData.EnumerateInstantiationObjectTargets(castTargetContext))
								{
									IEnumerator item2 = FightSpellEffectFactory.PlaySpellEffect(spellEffect, item4, spellEffectInstantiationData, castTargetContext);
									routineList.Add(item2);
								}
								yield return EnumeratorUtility.ParallelRecursiveImmediateSafeExecution(routineList.ToArray());
								routineList.Clear();
							}
							num = i + 1;
						}
						ListPool<IEnumerator>.Release(routineList);
					}
					FightSpellEffectFactory.SetupSpellEffectOverrides(definition, fightStatus.fightId, eventId);
				}
				else
				{
					Log.Error($"Could not find spell with instance id {spellInstanceId} for player with id {concernedEntity}.", 128, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\PlaySpellEvent.cs");
				}
			}
			else
			{
				Log.Error(FightEventErrors.PlayerNotFound(concernedEntity), 133, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\PlaySpellEvent.cs");
			}
		}

		private static CellObject GetTargetedCell(FightStatus fightStatus, CastTarget castTarget)
		{
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			FightMap current = FightMap.current;
			if (null == current)
			{
				return null;
			}
			switch (castTarget.ValueCase)
			{
			case CastTarget.ValueOneofCase.Cell:
			{
				CellCoord cell = castTarget.Cell;
				return current.GetCellObject(cell.X, cell.Y);
			}
			case CastTarget.ValueOneofCase.EntityId:
			{
				int entityId = castTarget.EntityId;
				if (fightStatus.TryGetEntity(entityId, out IEntityWithBoardPresence entityStatus))
				{
					Vector2Int refCoord = entityStatus.area.refCoord;
					return current.GetCellObject(refCoord.get_x(), refCoord.get_y());
				}
				return null;
			}
			default:
				return null;
			}
		}
	}
}
