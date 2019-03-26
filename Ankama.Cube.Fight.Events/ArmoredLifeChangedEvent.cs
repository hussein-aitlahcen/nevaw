using Ankama.Cube.Data;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps;
using Ankama.Cube.Maps.Feedbacks;
using Ankama.Cube.Maps.Objects;
using Ankama.Cube.UI.Fight;
using Ankama.Cube.UI.Fight.Info;
using Ankama.Utilities;
using System;
using System.Collections;

namespace Ankama.Cube.Fight.Events
{
	public class ArmoredLifeChangedEvent : FightEvent, IRelatedToEntity
	{
		private enum LifeModificationType
		{
			Undefined,
			ArmorGain,
			Damage,
			Heal,
			Death
		}

		public int concernedEntity
		{
			get;
			private set;
		}

		public int? lifeBefore
		{
			get;
			private set;
		}

		public int? lifeAfter
		{
			get;
			private set;
		}

		public int? armorBefore
		{
			get;
			private set;
		}

		public int? armorAfter
		{
			get;
			private set;
		}

		public override void UpdateStatus(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out IEntityWithLife entityStatus))
			{
				if (armorAfter.HasValue)
				{
					entityStatus.SetCarac(CaracId.Armor, armorAfter.Value);
				}
				if (lifeAfter.HasValue)
				{
					entityStatus.SetCarac(CaracId.Life, lifeAfter.Value);
				}
			}
			else
			{
				Log.Error(FightEventErrors.EntityNotFound<IEntityWithLife>(concernedEntity), 41, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\ArmoredLifeChangedEvent.cs");
			}
			FightLogicExecutor.FireUpdateStatus(fightStatus.fightId, EventCategory.LifeArmorChanged);
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			int fightId = fightStatus.fightId;
			if (fightStatus.TryGetEntity(concernedEntity, out IEntityWithBoardPresence entityStatus))
			{
				int change = 0;
				int num = 0;
				int num2 = 0;
				LifeModificationType lifeModificationType = LifeModificationType.Undefined;
				if (armorAfter.HasValue)
				{
					num = armorAfter.Value;
					if (armorBefore.HasValue)
					{
						int value = armorBefore.Value;
						change += num - value;
						if (value > num)
						{
							lifeModificationType = LifeModificationType.Damage;
						}
						else if (value < num)
						{
							lifeModificationType = LifeModificationType.ArmorGain;
						}
					}
				}
				if (lifeAfter.HasValue)
				{
					num2 = lifeAfter.Value;
					if (lifeBefore.HasValue)
					{
						int value2 = lifeBefore.Value;
						change += num2 - value2;
						if (value2 > num2)
						{
							lifeModificationType = ((num2 > 0) ? LifeModificationType.Damage : LifeModificationType.Death);
						}
						else if (value2 < num2)
						{
							lifeModificationType = ((entityStatus.type == EntityType.ObjectMechanism) ? LifeModificationType.ArmorGain : LifeModificationType.Heal);
						}
					}
				}
				if (lifeAfter.HasValue && entityStatus.type == EntityType.Hero)
				{
					HeroStatus heroStatus = (HeroStatus)entityStatus;
					if (heroStatus.ownerId == fightStatus.localPlayerId)
					{
						FightMap current = FightMap.current;
						if (null != current)
						{
							current.SetLocalPlayerHeroLife(num2, heroStatus.baseLife);
						}
					}
					if (fightStatus.TryGetEntity(heroStatus.ownerId, out PlayerStatus entityStatus2))
					{
						AbstractPlayerUIRework view = entityStatus2.view;
						if (null != view)
						{
							view.ChangeHeroLifePoints(num2);
						}
						TryDrawLowLifeMessage(num2, entityStatus2);
					}
				}
				IsoObject isoObject = entityStatus.view;
				if (null != isoObject)
				{
					IObjectWithArmoredLife objectWithArmoredLife2;
					IObjectWithArmoredLife objectWithArmoredLife = objectWithArmoredLife2 = (isoObject as IObjectWithArmoredLife);
					if (objectWithArmoredLife2 != null)
					{
						int life = lifeAfter.HasValue ? num2 : objectWithArmoredLife.life;
						int armor = armorAfter.HasValue ? num : objectWithArmoredLife.armor;
						objectWithArmoredLife.SetArmoredLife(life, armor);
					}
					else
					{
						Log.Error(FightEventErrors.EntityHasIncompatibleView<IObjectWithArmoredLife>(entityStatus), 151, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\ArmoredLifeChangedEvent.cs");
						objectWithArmoredLife = null;
					}
					switch (lifeModificationType)
					{
					case LifeModificationType.ArmorGain:
						yield return FightSpellEffectFactory.PlayGenericEffect(SpellEffectKey.ArmorGain, fightId, parentEventId, isoObject, fightStatus.context);
						ValueChangedFeedback.Launch(change, ValueChangedFeedback.Type.Heal, isoObject.cellObject.get_transform());
						break;
					case LifeModificationType.Damage:
						yield return FightSpellEffectFactory.PlayGenericEffect(SpellEffectKey.Damage, fightId, parentEventId, isoObject, fightStatus.context);
						ValueChangedFeedback.Launch(change, ValueChangedFeedback.Type.Damage, isoObject.cellObject.get_transform());
						if (objectWithArmoredLife != null)
						{
							yield return objectWithArmoredLife.Hit(isoObject.area.refCoord);
						}
						break;
					case LifeModificationType.Heal:
						yield return FightSpellEffectFactory.PlayGenericEffect(SpellEffectKey.Heal, fightId, parentEventId, isoObject, fightStatus.context);
						ValueChangedFeedback.Launch(change, ValueChangedFeedback.Type.Heal, isoObject.cellObject.get_transform());
						break;
					case LifeModificationType.Death:
						yield return FightSpellEffectFactory.PlayGenericEffect(SpellEffectKey.Damage, fightId, parentEventId, isoObject, fightStatus.context);
						ValueChangedFeedback.Launch(change, ValueChangedFeedback.Type.Damage, isoObject.cellObject.get_transform());
						objectWithArmoredLife?.LethalHit(isoObject.area.refCoord);
						break;
					default:
						throw new ArgumentOutOfRangeException();
					case LifeModificationType.Undefined:
						break;
					}
				}
				else
				{
					Log.Error(FightEventErrors.EntityHasNoView(entityStatus), 197, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\ArmoredLifeChangedEvent.cs");
				}
			}
			else
			{
				Log.Error(FightEventErrors.EntityNotFound<IEntityWithBoardPresence>(concernedEntity), 202, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\ArmoredLifeChangedEvent.cs");
			}
			FightLogicExecutor.FireUpdateView(fightId, EventCategory.LifeArmorChanged);
		}

		private void TryDrawLowLifeMessage(int lifeAfterValue, PlayerStatus ownerStatus)
		{
			FightType fightType = GameStatus.fightType;
			if ((fightType != FightType.BossFight && fightType != FightType.TeamVersus) || lifeAfterValue <= 0)
			{
				return;
			}
			int num = (int)((float)ownerStatus.heroStatus.baseLife * 0.35f);
			if (lifeAfterValue <= num && (!lifeBefore.HasValue || lifeBefore.Value > num))
			{
				MessageInfoRibbonGroup messageGroup = (GameStatus.localPlayerTeamIndex != ownerStatus.teamIndex) ? MessageInfoRibbonGroup.OtherID : MessageInfoRibbonGroup.MyID;
				FightUIRework instance = FightUIRework.instance;
				if (null != instance)
				{
					FightInfoMessage message = FightInfoMessage.HeroLowLife(messageGroup);
					instance.DrawInfoMessage(message, ownerStatus.nickname);
				}
			}
		}

		public ArmoredLifeChangedEvent(int eventId, int? parentEventId, int concernedEntity, int? lifeBefore, int? lifeAfter, int? armorBefore, int? armorAfter)
			: base(FightEventData.Types.EventType.ArmoredLifeChanged, eventId, parentEventId)
		{
			this.concernedEntity = concernedEntity;
			this.lifeBefore = lifeBefore;
			this.lifeAfter = lifeAfter;
			this.armorBefore = armorBefore;
			this.armorAfter = armorAfter;
		}

		public ArmoredLifeChangedEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.ArmoredLifeChanged, proto)
		{
			concernedEntity = proto.Int1;
			lifeBefore = proto.OptInt1;
			lifeAfter = proto.OptInt2;
			armorBefore = proto.OptInt3;
			armorAfter = proto.OptInt4;
		}
	}
}
