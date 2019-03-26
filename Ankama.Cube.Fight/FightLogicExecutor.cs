using Ankama.Cube.Data;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Fight.Events;
using Ankama.Utilities;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Fight
{
	public static class FightLogicExecutor
	{
		private class DirtySpellsCounter
		{
			private readonly List<PlayerStatus> m_keys;

			private readonly List<int> m_values;

			public int count => m_keys.Count;

			public DirtySpellsCounter(int capacity)
			{
				m_keys = new List<PlayerStatus>(capacity);
				m_values = new List<int>(capacity);
			}

			public bool GetAt(int index, out PlayerStatus key, out int value)
			{
				if (index >= 0 && index < m_values.Count)
				{
					key = m_keys[index];
					value = m_values[index];
					if (key != null)
					{
						return value > 0;
					}
					return false;
				}
				key = null;
				value = 0;
				return false;
			}

			public void ResetAll()
			{
				int count = m_values.Count;
				for (int i = 0; i < count; i++)
				{
					m_values[i] = 0;
				}
			}

			public void Increment(PlayerStatus key, int value)
			{
				int num = m_keys.IndexOf(key);
				if (num >= 0 && num < m_values.Count)
				{
					List<int> values = m_values;
					int index = num;
					values[index] += value;
				}
				else
				{
					m_keys.Add(key);
					m_values.Add(value);
				}
			}
		}

		private class Instance
		{
			public readonly FightStatus fightStatus;

			private int m_dirtyEntitiesCounter;

			private readonly DirtySpellsCounter m_dirtySpellsCounters = new DirtySpellsCounter(2);

			private IEnumerator m_current;

			private bool m_awaitsSynchronization;

			private readonly Queue<IEnumerator> m_executionQueue = new Queue<IEnumerator>();

			private readonly Dictionary<EventCategory, List<Action<EventCategory>>> m_listenerUpdateStatus = new Dictionary<EventCategory, List<Action<EventCategory>>>();

			private readonly Dictionary<EventCategory, List<Action<EventCategory>>> m_listenerUpdateView = new Dictionary<EventCategory, List<Action<EventCategory>>>();

			public Instance(FightStatus fightStatus)
			{
				this.fightStatus = fightStatus;
			}

			public IEnumerator Tick()
			{
				while (true)
				{
					if (m_current != null)
					{
						yield return m_current;
						if (m_executionQueue.Count > 0)
						{
							m_current = m_executionQueue.Dequeue();
						}
						else
						{
							m_current = null;
						}
					}
					else
					{
						yield return null;
					}
				}
			}

			public void Clear()
			{
				fightStatus.Dispose();
				m_executionQueue.Clear();
				m_current = null;
			}

			public void ProcessFightEvents(List<FightEvent> fightEvents)
			{
				ProcessFightEventsUpdateStatus(fightEvents, fightStatus);
				ProcessFightEventsUpdateViews(fightEvents, fightStatus);
				CleanUpFightStatus();
			}

			public void NotifyEntityRemoved()
			{
				m_dirtyEntitiesCounter++;
			}

			public void NotifySpellRemovedForPlayer(PlayerStatus playerStatus)
			{
				m_dirtySpellsCounters.Increment(playerStatus, 1);
			}

			public void NotifySpellReAddedForPlayer(PlayerStatus playerStatus)
			{
				m_dirtySpellsCounters.Increment(playerStatus, -1);
			}

			private void ProcessFightEventsUpdateStatus(List<FightEvent> fightEvents, FightStatus activeFightStatus)
			{
				m_dirtyEntitiesCounter = 0;
				m_dirtySpellsCounters.ResetAll();
				int count = fightEvents.Count;
				for (int i = 0; i < count; i++)
				{
					FightEvent fightEvent = fightEvents[i];
					long? num = fightEvent.parentEventId;
					if (num.HasValue)
					{
						s_eventHierarchyBuffer[num.Value].AddChildEvent(fightEvent);
					}
					try
					{
						fightEvent.UpdateStatus(activeFightStatus);
					}
					catch (Exception ex)
					{
						Log.Error($"Exception occured while event {fightEvent.eventType} #{fightEvent.eventId} updated fight status.", 144, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightLogicExecutor.Instance.cs");
						Debug.LogException(ex);
					}
					s_eventHierarchyBuffer.Add(fightEvent.eventId, fightEvent);
				}
				try
				{
					activeFightStatus.TriggerUpdateEvents();
				}
				catch (Exception ex2)
				{
					Log.Error($"Exception occured while triggering update events of fight status #{activeFightStatus.fightId}.", 163, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightLogicExecutor.Instance.cs");
					Debug.LogException(ex2);
				}
				s_eventHierarchyBuffer.Clear();
			}

			private void ProcessFightEventsUpdateViews(List<FightEvent> fightEvents, FightStatus activeFightStatus)
			{
				int num = 1;
				int count = fightEvents.Count;
				FightEvent fightEvent = fightEvents[0];
				s_eventGroupBuffer.Add(fightEvent);
				while (num < count && fightEvent.IsInvisible())
				{
					FightEvent fightEvent2 = fightEvents[num];
					num++;
					s_eventGroupBuffer.Add(fightEvent2);
					fightEvent = fightEvent2;
				}
				while (num < count)
				{
					FightEvent fightEvent3 = fightEvents[num];
					num++;
					if (!fightEvent3.IsInvisible() && !fightEvent3.CanBeGroupedWith(fightEvent))
					{
						SendFightEventGroupToExecution(activeFightStatus);
					}
					s_eventGroupBuffer.Add(fightEvent3);
					fightEvent = fightEvent3;
				}
				SendFightEventGroupToExecution(activeFightStatus);
			}

			private void SendFightEventGroupToExecution(FightStatus activeFightStatus)
			{
				int count = s_eventGroupBuffer.Count;
				if (count == 1)
				{
					FightEvent fightEvent = s_eventGroupBuffer[0];
					try
					{
						if (fightEvent.SynchronizeExecution())
						{
							Execute(SetupSynchronizationBarrier());
							IEnumerator enumerator = fightEvent.UpdateView(activeFightStatus);
							if (enumerator != null)
							{
								Execute(enumerator);
							}
							Execute(ReleaseSynchronizationBarrier());
						}
						else
						{
							IEnumerator enumerator2 = fightEvent.UpdateView(activeFightStatus);
							if (enumerator2 != null)
							{
								Execute(enumerator2);
							}
						}
					}
					catch (Exception ex)
					{
						Log.Error($"Exception occured while event {fightEvent.eventType} #{fightEvent.eventId} updated fight view.", 255, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightLogicExecutor.Instance.cs");
						Debug.LogException(ex);
					}
				}
				else
				{
					IEnumerator[] array = new IEnumerator[count];
					for (int i = 0; i < count; i++)
					{
						FightEvent fightEvent2 = s_eventGroupBuffer[i];
						try
						{
							array[i] = fightEvent2.UpdateView(activeFightStatus);
						}
						catch (Exception ex2)
						{
							Log.Error($"Exception occured while event {fightEvent2.eventType} #{fightEvent2.eventId} updated fight view.", 273, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightLogicExecutor.Instance.cs");
							Debug.LogException(ex2);
						}
					}
					IEnumerator action = EnumeratorUtility.ParallelRecursiveImmediateExecution(array);
					Execute(action);
				}
				s_eventGroupBuffer.Clear();
			}

			private void Execute([NotNull] IEnumerator action)
			{
				if (m_current == null)
				{
					m_current = action;
				}
				else
				{
					m_executionQueue.Enqueue(action);
				}
			}

			private void CleanUpFightStatus()
			{
				if (m_dirtyEntitiesCounter > 0)
				{
					Execute(Cleanup(m_dirtyEntitiesCounter));
				}
				int count = m_dirtySpellsCounters.count;
				for (int i = 0; i < count; i++)
				{
					if (m_dirtySpellsCounters.GetAt(i, out PlayerStatus key, out int value) && value > 0)
					{
						Execute(key.CleanupDirtySpells(value));
					}
				}
				Execute(ClearSpellEffectOverrides());
			}

			private IEnumerator Cleanup(int dirtyEntitiesCounter)
			{
				fightStatus.Cleanup(dirtyEntitiesCounter);
				yield break;
			}

			private IEnumerator ClearSpellEffectOverrides()
			{
				FightSpellEffectFactory.ClearSpellEffectOverrides(fightStatus.fightId);
				yield break;
			}

			public void AddListenerUpdateStatus(Action<EventCategory> listener, EventCategory eventCategoryToListen)
			{
				if (!m_listenerUpdateStatus.TryGetValue(eventCategoryToListen, out List<Action<EventCategory>> value))
				{
					value = new List<Action<EventCategory>>();
					m_listenerUpdateStatus.Add(eventCategoryToListen, value);
				}
				value.Add(listener);
			}

			public void AddListenerUpdateView(Action<EventCategory> listener, EventCategory category)
			{
				if (!m_listenerUpdateView.TryGetValue(category, out List<Action<EventCategory>> value))
				{
					value = new List<Action<EventCategory>>();
					m_listenerUpdateView.Add(category, value);
				}
				value.Add(listener);
			}

			public void RemoveListenerUpdateStatus(Action<EventCategory> listener, EventCategory category)
			{
				if (!m_listenerUpdateStatus.TryGetValue(category, out List<Action<EventCategory>> value) || !value.Remove(listener))
				{
					Log.Error($"Try to remove an unknown status listener for event category {category}.", 375, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightLogicExecutor.Instance.cs");
				}
			}

			public void RemoveListenerUpdateView(Action<EventCategory> listener, EventCategory category)
			{
				if (!m_listenerUpdateView.TryGetValue(category, out List<Action<EventCategory>> value) || !value.Remove(listener))
				{
					Log.Error($"Try to remove an unknown view listener for event category {category}.", 388, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightLogicExecutor.Instance.cs");
				}
			}

			public void FireUpdateStatus(EventCategory category)
			{
				if (m_listenerUpdateStatus.TryGetValue(category, out List<Action<EventCategory>> value))
				{
					int i = 0;
					for (int count = value.Count; i < count; i++)
					{
						value[i](category);
					}
				}
			}

			public void FireUpdateView(EventCategory category)
			{
				if (m_listenerUpdateView.TryGetValue(category, out List<Action<EventCategory>> value))
				{
					int i = 0;
					for (int count = value.Count; i < count; i++)
					{
						value[i](category);
					}
				}
			}
		}

		private struct SynchronizationBarrier
		{
			public readonly long id;

			public readonly int counter;

			public readonly int maxCounter;

			public SynchronizationBarrier(long id)
			{
				this.id = id;
				counter = 1;
				maxCounter = 1;
			}

			private SynchronizationBarrier(long id, int counter, int maxCounter)
			{
				this.id = id;
				this.counter = counter;
				this.maxCounter = maxCounter;
			}

			public static SynchronizationBarrier Upgrade(SynchronizationBarrier barrier)
			{
				return new SynchronizationBarrier(barrier.id, barrier.counter + 1, barrier.maxCounter + 1);
			}

			public static SynchronizationBarrier Downgrade(SynchronizationBarrier barrier)
			{
				return new SynchronizationBarrier(barrier.id, barrier.counter - 1, barrier.maxCounter);
			}
		}

		private static bool s_running;

		private static Instance[] s_instances;

		private static readonly Dictionary<long, FightEvent> s_eventHierarchyBuffer = new Dictionary<long, FightEvent>();

		private static readonly List<FightEvent> s_eventGroupBuffer = new List<FightEvent>(8);

		public static bool fightInitialized;

		private static long s_synchronizationBarrierId;

		private static SynchronizationBarrier s_synchronizationBarrier;

		public static bool isValid => s_instances != null;

		public static int fightCount => s_instances.Length;

		public static void Initialize(int count)
		{
			fightInitialized = false;
			s_instances = new Instance[count];
		}

		public static void AddFightStatus(FightStatus fightStatus)
		{
			s_instances[fightStatus.fightId] = new Instance(fightStatus);
		}

		public static void Start()
		{
			if (!s_running)
			{
				s_running = true;
				MonoBehaviourExtensions.StartCoroutineImmediateSafe(Main.monoBehaviour, Run(), null);
			}
		}

		public static void Stop()
		{
			if (s_running)
			{
				s_running = false;
			}
		}

		public static FightStatus GetFightStatus(int fightId)
		{
			return s_instances[fightId].fightStatus;
		}

		public static void ProcessFightEvents(int fightId, List<FightEvent> fightEvents)
		{
			s_instances[fightId].ProcessFightEvents(fightEvents);
		}

		public static void NotifyEntityRemoved(int fightId)
		{
			s_instances[fightId].NotifyEntityRemoved();
		}

		public static void NotifySpellRemovedForPlayer(int fightId, PlayerStatus playerStatus)
		{
			s_instances[fightId].NotifySpellRemovedForPlayer(playerStatus);
		}

		public static void NotifySpellReAddedForPlayer(int fightId, PlayerStatus playerStatus)
		{
			s_instances[fightId].NotifySpellReAddedForPlayer(playerStatus);
		}

		private static IEnumerator Run()
		{
			Instance[] array = s_instances;
			int instanceCount = array.Length;
			IEnumerator[] routines = new IEnumerator[instanceCount];
			for (int i = 0; i < instanceCount; i++)
			{
				routines[i] = array[i].Tick();
			}
			while (s_running)
			{
				for (int j = 0; j < instanceCount; j++)
				{
					CollectionsExtensions.MoveNextRecursiveImmediateSafe(routines[j], null);
				}
				yield return null;
			}
			for (int k = 0; k < instanceCount; k++)
			{
				s_instances[k].Clear();
			}
			s_instances = null;
			s_synchronizationBarrierId = 0L;
			s_synchronizationBarrier = default(SynchronizationBarrier);
		}

		public static void AddListenerUpdateStatus(int fightId, Action<EventCategory> listener, EventCategory eventCategoriesToListen)
		{
			s_instances[fightId].AddListenerUpdateStatus(listener, eventCategoriesToListen);
		}

		public static void AddListenerUpdateView(int fightId, Action<EventCategory> listener, EventCategory eventCategoriesToListen)
		{
			s_instances[fightId].AddListenerUpdateView(listener, eventCategoriesToListen);
		}

		public static void RemoveListenerUpdateStatus(int fightId, Action<EventCategory> listener, EventCategory category)
		{
			if (s_instances != null)
			{
				s_instances[fightId].RemoveListenerUpdateStatus(listener, category);
			}
		}

		public static void RemoveListenerUpdateView(int fightId, Action<EventCategory> listener, EventCategory category)
		{
			if (s_instances != null)
			{
				s_instances[fightId].RemoveListenerUpdateView(listener, category);
			}
		}

		public static void FireUpdateStatus(int fightId, EventCategory category)
		{
			s_instances[fightId].FireUpdateStatus(category);
		}

		public static void FireUpdateView(int fightId, EventCategory category)
		{
			s_instances[fightId].FireUpdateView(category);
		}

		private static IEnumerator SetupSynchronizationBarrier()
		{
			SynchronizationBarrier synchronizationBarrier = s_synchronizationBarrier;
			if (synchronizationBarrier.counter == 0)
			{
				s_synchronizationBarrier = new SynchronizationBarrier(s_synchronizationBarrierId++);
			}
			else if (synchronizationBarrier.maxCounter == s_instances.Length)
			{
				Log.Error("Synchronization error: tried to setup a barrier but the current barrier has already reached all instances.", 55, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightLogicExecutor.Synchronization.cs");
			}
			else
			{
				s_synchronizationBarrier = SynchronizationBarrier.Upgrade(synchronizationBarrier);
			}
			yield break;
		}

		private static IEnumerator ReleaseSynchronizationBarrier()
		{
			SynchronizationBarrier synchronizationBarrier = s_synchronizationBarrier;
			long barrierId = synchronizationBarrier.id;
			if (synchronizationBarrier.counter == 0)
			{
				Log.Error("Synchronization error: tried to release a barrier but no barrier is currently setup.", 69, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Fight\\FightLogicExecutor.Synchronization.cs");
				yield break;
			}
			int instanceCount = s_instances.Length;
			while (s_synchronizationBarrier.maxCounter != instanceCount)
			{
				yield return null;
				if (!s_running)
				{
					break;
				}
			}
			s_synchronizationBarrier = SynchronizationBarrier.Downgrade(s_synchronizationBarrier);
			do
			{
				synchronizationBarrier = s_synchronizationBarrier;
				if (synchronizationBarrier.id == barrierId && synchronizationBarrier.counter != 0)
				{
					yield return null;
					continue;
				}
				break;
			}
			while (s_running);
		}
	}
}
