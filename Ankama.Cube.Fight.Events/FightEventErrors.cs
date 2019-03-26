using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps.Objects;
using Ankama.Cube.Protocols.CommonProtocol;
using DataEditor;

namespace Ankama.Cube.Fight.Events
{
	public static class FightEventErrors
	{
		public static string PlayerNotFound(int entityId)
		{
			return $"Could not find player with id {entityId}.";
		}

		public static string PlayerNotFound(int entityId, int fightId)
		{
			return $"Could not find player with id {entityId} in fight with id {fightId}.";
		}

		public static string EntityNotFound<T>(int entityId) where T : IEntity
		{
			return $"Could not find entity with id {entityId} or entity does not implement {typeof(T).Name}.";
		}

		public static string EntityHasNoView(IEntityWithBoardPresence entity)
		{
			return $"{entity.GetType().Name} entity with id {entity.id} doesn't have a valid view.";
		}

		public static string EntityHasIncompatibleView<T>(IEntityWithBoardPresence entity)
		{
			IsoObject view = entity.view;
			if (!(null == view))
			{
				return $"{entity.GetType().Name} entity with id {entity.id} has a view of type {((object)entity.view).GetType().Name} that does not implement {typeof(T).Name}.";
			}
			return EntityHasNoView(entity);
		}

		public static string EntityCreationFailed<EntityType, DefinitionType>(int entityId, int definitionId)
		{
			return $"Could not create {typeof(EntityType).Name} entity because no {typeof(DefinitionType).Name} with id {definitionId} could be found.";
		}

		public static string InvalidPosition(CellCoord refCoord)
		{
			return $"Invalid position {{{refCoord.X}, {refCoord.Y}}}.";
		}

		public static string ReserveCompanionNotFound(int definitionId, int playerId)
		{
			return $"Could not find a reserve companion with definition id {definitionId} in the reserve of player with id {playerId}";
		}

		public static string DefinitionNotFound<DefinitionType>(int id) where DefinitionType : EditableData
		{
			return $"Could not find {typeof(DefinitionType).Name} with id {id}.";
		}
	}
}
