using Ankama.Cube.Animations;
using Ankama.Cube.Data;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps;
using Ankama.Cube.Maps.Objects;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Fight
{
	public class FightStatus : IMapEntityProvider
	{
		public static FightStatus local;

		public readonly int fightId;

		public readonly FightMapStatus mapStatus;

		public readonly FightContext context;

		public int localPlayerId;

		public int turnIndex;

		public int currentTurnPlayerId;

		public FightStatusEndReason endReason;

		private readonly Dictionary<int, EntityStatus> m_entities = new Dictionary<int, EntityStatus>();

		private readonly List<int> m_dirtyEntities = new List<int>();

		private EntitiesChangedFlags m_entitiesChangedFlags;

		public bool isEnded => endReason != FightStatusEndReason.None;

		public event EntitiesChangedDelegate EntitiesChanged;

		public FightStatus(int fightId, FightMapStatus mapStatus)
		{
			this.fightId = fightId;
			this.mapStatus = mapStatus;
			context = FightContext.Create(fightId);
		}

		public void Dispose()
		{
			Object.Destroy(context);
			foreach (EntityStatus value in m_entities.Values)
			{
				IEntityWithBoardPresence entityWithBoardPresence;
				if ((entityWithBoardPresence = (value as IEntityWithBoardPresence)) != null)
				{
					IsoObject view = entityWithBoardPresence.view;
					if (null != view)
					{
						view.DetachFromCell();
						view.Destroy();
					}
				}
			}
			m_entities.Clear();
			m_dirtyEntities.Clear();
		}

		public void TriggerUpdateEvents()
		{
			if (m_entitiesChangedFlags != 0)
			{
				EntitiesChangedFlags entitiesChangedFlags = m_entitiesChangedFlags;
				m_entitiesChangedFlags = EntitiesChangedFlags.None;
				this.EntitiesChanged?.Invoke(this, entitiesChangedFlags);
			}
		}

		[PublicAPI]
		public void AddEntity([NotNull] EntityStatus entity)
		{
			m_entities[entity.id] = entity;
			m_entitiesChangedFlags |= EntitiesChangedFlags.Added;
		}

		[PublicAPI]
		public void RemoveEntity(int id)
		{
			m_entities[id].MarkForRemoval();
			m_entitiesChangedFlags |= EntitiesChangedFlags.Removed;
			m_dirtyEntities.Add(id);
			FightLogicExecutor.NotifyEntityRemoved(fightId);
		}

		[PublicAPI]
		public bool TryRemoveEntity(int id)
		{
			if (m_entities.TryGetValue(id, out EntityStatus value))
			{
				value.MarkForRemoval();
				m_entitiesChangedFlags |= EntitiesChangedFlags.Removed;
				m_dirtyEntities.Add(id);
				FightLogicExecutor.NotifyEntityRemoved(fightId);
				return true;
			}
			return false;
		}

		public void NotifyEntityAreaMoved()
		{
			m_entitiesChangedFlags |= EntitiesChangedFlags.AreaMoved;
		}

		public void NotifyEntityPlayableStateChanged()
		{
			m_entitiesChangedFlags |= EntitiesChangedFlags.PlayableState;
		}

		[PublicAPI]
		public void Cleanup(int counter)
		{
			for (int i = 0; i < counter; i++)
			{
				int key = m_dirtyEntities[i];
				m_entities.Remove(key);
			}
			m_dirtyEntities.RemoveRange(0, counter);
		}

		public bool HasEntity<T>(Predicate<T> predicate) where T : class, IEntity
		{
			foreach (EntityStatus value in m_entities.Values)
			{
				T obj;
				if (!value.isDirty && (obj = (value as T)) != null && predicate(obj))
				{
					return true;
				}
			}
			return false;
		}

		public bool TryGetEntity<T>(Predicate<T> predicate, out T entityStatus) where T : class, IEntity
		{
			foreach (EntityStatus value in m_entities.Values)
			{
				T val;
				if (!value.isDirty && (val = (value as T)) != null && predicate(val))
				{
					entityStatus = val;
					return true;
				}
			}
			entityStatus = null;
			return false;
		}

		public IEnumerable<T> EnumerateEntities<T>() where T : class, IEntity
		{
			foreach (EntityStatus value in m_entities.Values)
			{
				T val;
				if (!value.isDirty && (val = (value as T)) != null)
				{
					yield return val;
				}
			}
		}

		public bool IsCharacterPlayable(ICharacterEntity character)
		{
			if (!character.actionUsed && character.ownerId == localPlayerId)
			{
				if (!character.canMove)
				{
					return character.canDoActionOnTarget;
				}
				return true;
			}
			return false;
		}

		public bool HasEntityBlockingMovementAt(Vector2Int position)
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			foreach (EntityStatus value in m_entities.Values)
			{
				IEntityWithBoardPresence entityWithBoardPresence;
				if (!value.isDirty && (entityWithBoardPresence = (value as IEntityWithBoardPresence)) != null && entityWithBoardPresence.blocksMovement && entityWithBoardPresence.area.Intersects(position))
				{
					return true;
				}
			}
			return false;
		}

		public bool TryGetEntityBlockingMovementAt(Vector2Int position, out IEntityWithBoardPresence entityBlockingMovement)
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			foreach (EntityStatus value in m_entities.Values)
			{
				IEntityWithBoardPresence entityWithBoardPresence;
				if (!value.isDirty && (entityWithBoardPresence = (value as IEntityWithBoardPresence)) != null && entityWithBoardPresence.blocksMovement && entityWithBoardPresence.area.Intersects(position))
				{
					entityBlockingMovement = entityWithBoardPresence;
					return true;
				}
			}
			entityBlockingMovement = null;
			return false;
		}

		public bool TryGetEntityAt<T>(Vector2Int position, out T character) where T : class, IEntityWithBoardPresence
		{
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			foreach (EntityStatus value in m_entities.Values)
			{
				T val;
				if (!value.isDirty && (val = (value as T)) != null && val.area.Intersects(position))
				{
					character = val;
					return true;
				}
			}
			character = null;
			return false;
		}

		public bool TryGetPlayableCharacterAt(Vector2Int position, out ICharacterEntity character)
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			foreach (EntityStatus value in m_entities.Values)
			{
				ICharacterEntity characterEntity;
				if (!value.isDirty && (characterEntity = (value as ICharacterEntity)) != null && characterEntity.area.Intersects(position))
				{
					if (!IsCharacterPlayable(characterEntity))
					{
						break;
					}
					character = characterEntity;
					return true;
				}
			}
			character = null;
			return false;
		}

		public IEnumerable<ICharacterEntity> EnumeratePlayableCharacters()
		{
			foreach (EntityStatus value in m_entities.Values)
			{
				ICharacterEntity characterEntity;
				if (!value.isDirty && (characterEntity = (value as ICharacterEntity)) != null && IsCharacterPlayable(characterEntity))
				{
					yield return characterEntity;
				}
			}
		}

		[PublicAPI]
		public bool HasEntity(int id)
		{
			return m_entities.ContainsKey(id);
		}

		[NotNull]
		[PublicAPI]
		public EntityStatus GetEntity(int id)
		{
			return m_entities[id];
		}

		[PublicAPI]
		public bool TryGetEntity(int id, out EntityStatus entityStatus)
		{
			return m_entities.TryGetValue(id, out entityStatus);
		}

		[PublicAPI]
		public bool HasEntity<T>(int id) where T : IEntity
		{
			if (m_entities.TryGetValue(id, out EntityStatus value) && value is T)
			{
				return true;
			}
			return false;
		}

		[NotNull]
		[PublicAPI]
		public T GetEntity<T>(int id) where T : IEntity
		{
			return (T)(object)m_entities[id];
		}

		[PublicAPI]
		public bool TryGetEntity<T>(int id, out T entityStatus) where T : IEntity
		{
			if (m_entities.TryGetValue(id, out EntityStatus value) && value is T)
			{
				entityStatus = (T)(object)value;
				return true;
			}
			entityStatus = default(T);
			return false;
		}

		[PublicAPI]
		public bool HasEntity<T>(int id, [NotNull] Predicate<T> predicate) where T : IEntity
		{
			EntityStatus value;
			if (m_entities.TryGetValue(id, out value) && value is T && predicate((T)(object)value))
			{
				return true;
			}
			return false;
		}

		[PublicAPI]
		public bool FindEntity<T>(Predicate<T> predicate, out T entityStatus) where T : IEntity
		{
			foreach (EntityStatus value in m_entities.Values)
			{
				if (value is T)
				{
					T val = (T)(object)value;
					if (predicate(val))
					{
						entityStatus = val;
						return true;
					}
				}
			}
			entityStatus = default(T);
			return false;
		}

		[PublicAPI]
		public bool TryGetEntity<T>(int id, Predicate<T> predicate, out T entityStatus) where T : IEntity
		{
			if (m_entities.TryGetValue(id, out EntityStatus value) && value is T)
			{
				T val = (T)(object)value;
				if (predicate(val))
				{
					entityStatus = val;
					return true;
				}
			}
			entityStatus = default(T);
			return false;
		}

		[PublicAPI]
		public IEnumerable<IEntity> EnumerateEntities()
		{
			foreach (EntityStatus value in m_entities.Values)
			{
				if (!value.isDirty)
				{
					yield return value;
				}
			}
		}

		[PublicAPI]
		public IEnumerable<T> EnumerateEntities<T>(Predicate<T> predicate) where T : IEntity
		{
			foreach (EntityStatus value in m_entities.Values)
			{
				if (!value.isDirty && value is T)
				{
					T val = (T)(object)value;
					if (predicate(val))
					{
						yield return val;
					}
				}
			}
		}

		[PublicAPI]
		public IEnumerable<Coord> EnumerateCoords()
		{
			return mapStatus.EnumerateCoords();
		}

		public PlayerStatus GetLocalPlayer()
		{
			return (PlayerStatus)m_entities[localPlayerId];
		}

		public IEnumerable<PlayerStatus> EnumeratePlayers()
		{
			foreach (EntityStatus value in m_entities.Values)
			{
				PlayerStatus playerStatus;
				if (!value.isDirty && (playerStatus = (value as PlayerStatus)) != null)
				{
					yield return playerStatus;
				}
			}
		}
	}
}
