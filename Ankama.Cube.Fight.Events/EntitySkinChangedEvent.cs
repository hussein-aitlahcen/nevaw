using Ankama.Cube.Data;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps.Objects;
using Ankama.Utilities;
using System.Collections;

namespace Ankama.Cube.Fight.Events
{
	public class EntitySkinChangedEvent : FightEvent, IRelatedToEntity
	{
		public int concernedEntity
		{
			get;
			private set;
		}

		public int characterSkinId
		{
			get;
			private set;
		}

		public int gender
		{
			get;
			private set;
		}

		public EntitySkinChangedEvent(int eventId, int? parentEventId, int concernedEntity, int characterSkinId, int gender)
			: base(FightEventData.Types.EventType.EntitySkinChanged, eventId, parentEventId)
		{
			this.concernedEntity = concernedEntity;
			this.characterSkinId = characterSkinId;
			this.gender = gender;
		}

		public EntitySkinChangedEvent(FightEventData proto)
			: base(FightEventData.Types.EventType.EntitySkinChanged, proto)
		{
			concernedEntity = proto.Int1;
			characterSkinId = proto.Int2;
			gender = proto.Int3;
		}

		public override IEnumerator UpdateView(FightStatus fightStatus)
		{
			if (fightStatus.TryGetEntity(concernedEntity, out IEntityWithBoardPresence entityStatus))
			{
				CharacterObject characterObject = entityStatus.view as CharacterObject;
				if (null != characterObject)
				{
					yield return characterObject.ChangeAnimatedCharacterData(characterSkinId, (Gender)gender);
				}
				else
				{
					Log.Error(FightEventErrors.EntityHasIncompatibleView<IEntityWithBoardPresence>(entityStatus), 22, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\EntitySkinChangedEvent.cs");
				}
			}
			else
			{
				Log.Error(FightEventErrors.EntityNotFound<IEntityWithBoardPresence>(concernedEntity), 27, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\Events\\EntitySkinChangedEvent.cs");
			}
		}
	}
}
