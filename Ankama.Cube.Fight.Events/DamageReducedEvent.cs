using Ankama.Cube.Protocols.CommonProtocol;

namespace Ankama.Cube.Fight.Events
{
	public class DamageReducedEvent : FightEvent, IRelatedToEntity
	{
		public int concernedEntity
		{
			get;
			private set;
		}

		public int reductionValue
		{
			get;
			private set;
		}

		public DamageReductionType reason
		{
			get;
			private set;
		}

		public DamageReducedEvent(int eventId, int? parentEventId, int concernedEntity, int reductionValue, DamageReductionType reason)
			: base(FightEventData.Types.EventType.DamageReduced, eventId, parentEventId)
		{
			this.concernedEntity = concernedEntity;
			this.reductionValue = reductionValue;
			this.reason = reason;
		}

		public DamageReducedEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.DamageReduced, proto)
		{
			concernedEntity = proto.Int1;
			reductionValue = proto.Int2;
			reason = proto.DamageReductionType1;
		}
	}
}
