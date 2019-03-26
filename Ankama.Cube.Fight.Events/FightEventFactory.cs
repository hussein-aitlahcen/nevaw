using System.ComponentModel;

namespace Ankama.Cube.Fight.Events
{
	public static class FightEventFactory
	{
		public static FightEvent FromProto(FightEventData proto)
		{
			switch (proto.EventType)
			{
			case FightEventData.Types.EventType.EffectStopped:
				return new EffectStoppedEvent(proto);
			case FightEventData.Types.EventType.TurnStarted:
				return new TurnStartedEvent(proto);
			case FightEventData.Types.EventType.EntityAreaMoved:
				return new EntityAreaMovedEvent(proto);
			case FightEventData.Types.EventType.PlayerAdded:
				return new PlayerAddedEvent(proto);
			case FightEventData.Types.EventType.HeroAdded:
				return new HeroAddedEvent(proto);
			case FightEventData.Types.EventType.CompanionAdded:
				return new CompanionAddedEvent(proto);
			case FightEventData.Types.EventType.EntityActioned:
				return new EntityActionedEvent(proto);
			case FightEventData.Types.EventType.SpellsMoved:
				return new SpellsMovedEvent(proto);
			case FightEventData.Types.EventType.TeamTurnEnded:
				return new TeamTurnEndedEvent(proto);
			case FightEventData.Types.EventType.PlaySpell:
				return new PlaySpellEvent(proto);
			case FightEventData.Types.EventType.TurnEnded:
				return new TurnEndedEvent(proto);
			case FightEventData.Types.EventType.ArmoredLifeChanged:
				return new ArmoredLifeChangedEvent(proto);
			case FightEventData.Types.EventType.EntityRemoved:
				return new EntityRemovedEvent(proto);
			case FightEventData.Types.EventType.ElementPointsChanged:
				return new ElementPointsChangedEvent(proto);
			case FightEventData.Types.EventType.CompanionAddedInReserve:
				return new CompanionAddedInReserveEvent(proto);
			case FightEventData.Types.EventType.ActionPointsChanged:
				return new ActionPointsChangedEvent(proto);
			case FightEventData.Types.EventType.CompanionReserveStateChanged:
				return new CompanionReserveStateChangedEvent(proto);
			case FightEventData.Types.EventType.EntityProtectionAdded:
				return new EntityProtectionAddedEvent(proto);
			case FightEventData.Types.EventType.EntityProtectionRemoved:
				return new EntityProtectionRemovedEvent(proto);
			case FightEventData.Types.EventType.MagicalDamageModifierChanged:
				return new MagicalDamageModifierChangedEvent(proto);
			case FightEventData.Types.EventType.MagicalHealModifierChanged:
				return new MagicalHealModifierChangedEvent(proto);
			case FightEventData.Types.EventType.MovementPointsChanged:
				return new MovementPointsChangedEvent(proto);
			case FightEventData.Types.EventType.DiceThrown:
				return new DiceThrownEvent(proto);
			case FightEventData.Types.EventType.SummoningAdded:
				return new SummoningAddedEvent(proto);
			case FightEventData.Types.EventType.EntityActionReset:
				return new EntityActionResetEvent(proto);
			case FightEventData.Types.EventType.ReservePointsChanged:
				return new ReservePointsChangedEvent(proto);
			case FightEventData.Types.EventType.ReserveUsed:
				return new ReserveUsedEvent(proto);
			case FightEventData.Types.EventType.PropertyChanged:
				return new PropertyChangedEvent(proto);
			case FightEventData.Types.EventType.FloorMechanismAdded:
				return new FloorMechanismAddedEvent(proto);
			case FightEventData.Types.EventType.FloorMechanismActivation:
				return new FloorMechanismActivationEvent(proto);
			case FightEventData.Types.EventType.ObjectMechanismAdded:
				return new ObjectMechanismAddedEvent(proto);
			case FightEventData.Types.EventType.SpellCostModifierAdded:
				return new SpellCostModifierAddedEvent(proto);
			case FightEventData.Types.EventType.SpellCostModifierRemoved:
				return new SpellCostModifierRemovedEvent(proto);
			case FightEventData.Types.EventType.TeamAdded:
				return new TeamAddedEvent(proto);
			case FightEventData.Types.EventType.FightEnded:
				return new FightEndedEvent(proto);
			case FightEventData.Types.EventType.Transformation:
				return new TransformationEvent(proto);
			case FightEventData.Types.EventType.ElementaryChanged:
				return new ElementaryChangedEvent(proto);
			case FightEventData.Types.EventType.DamageReduced:
				return new DamageReducedEvent(proto);
			case FightEventData.Types.EventType.Attack:
				return new AttackEvent(proto);
			case FightEventData.Types.EventType.Explosion:
				return new ExplosionEvent(proto);
			case FightEventData.Types.EventType.EntityAnimation:
				return new EntityAnimationEvent(proto);
			case FightEventData.Types.EventType.EntitySkinChanged:
				return new EntitySkinChangedEvent(proto);
			case FightEventData.Types.EventType.FloatingCounterValueChanged:
				return new FloatingCounterValueChangedEvent(proto);
			case FightEventData.Types.EventType.AssemblageChanged:
				return new AssemblageChangedEvent(proto);
			case FightEventData.Types.EventType.PhysicalDamageModifierChanged:
				return new PhysicalDamageModifierChangedEvent(proto);
			case FightEventData.Types.EventType.PhysicalHealModifierChanged:
				return new PhysicalHealModifierChangedEvent(proto);
			case FightEventData.Types.EventType.BossSummoningsWarning:
				return new BossSummoningsWarningEvent(proto);
			case FightEventData.Types.EventType.BossReserveModification:
				return new BossReserveModificationEvent(proto);
			case FightEventData.Types.EventType.BossEvolutionStepModification:
				return new BossEvolutionStepModificationEvent(proto);
			case FightEventData.Types.EventType.BossTurnStart:
				return new BossTurnStartEvent(proto);
			case FightEventData.Types.EventType.BossLifeModification:
				return new BossLifeModificationEvent(proto);
			case FightEventData.Types.EventType.BossCastSpell:
				return new BossCastSpellEvent(proto);
			case FightEventData.Types.EventType.GameEnded:
				return new GameEndedEvent(proto);
			case FightEventData.Types.EventType.CompanionGiven:
				return new CompanionGivenEvent(proto);
			case FightEventData.Types.EventType.CompanionReceived:
				return new CompanionReceivedEvent(proto);
			case FightEventData.Types.EventType.TeamTurnStarted:
				return new TeamTurnStartedEvent(proto);
			case FightEventData.Types.EventType.BossTurnEnd:
				return new BossTurnEndEvent(proto);
			case FightEventData.Types.EventType.TurnSynchronization:
				return new TurnSynchronizationEvent(proto);
			case FightEventData.Types.EventType.EventForParenting:
				return new EventForParentingEvent(proto);
			case FightEventData.Types.EventType.TeamsScoreModification:
				return new TeamsScoreModificationEvent(proto);
			case FightEventData.Types.EventType.MaxLifeChanged:
				return new MaxLifeChangedEvent(proto);
			case FightEventData.Types.EventType.FightInitialized:
				return new FightInitializedEvent(proto);
			default:
				throw new InvalidEnumArgumentException(proto.EventType.ToString());
			}
		}
	}
}
