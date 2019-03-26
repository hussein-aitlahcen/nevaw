using Ankama.Cube.Data;
using Ankama.Cube.Fight;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps;
using Ankama.Cube.Maps.Objects;
using Ankama.Cube.UI.Components;
using Ankama.Cube.UI.DeckMaker;
using Ankama.Cube.UI.Fight.NotificationWindow;
using Ankama.Cube.UI.Fight.Windows;
using Ankama.Cube.Utility;
using Ankama.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Ankama.Cube.UI.Fight
{
	public sealed class SpellBarRework : MonoBehaviour, ISpellStatusCellRendererConfigurator, ISpellCellRendererConfigurator, IWithTooltipCellRendererConfigurator, ICellRendererConfigurator, IDragNDropValidator
	{
		[SerializeField]
		private DynamicList m_spellList;

		[SerializeField]
		private FightTooltip m_tooltip;

		[SerializeField]
		private TooltipPosition m_tooltipPosition;

		[SerializeField]
		private FightUIFactory m_factory;

		private CastEventListener m_castEventListener;

		private readonly ObjectPool<Queue<SpellStatusData>> m_queuePool = new ObjectPool<Queue<SpellStatusData>>();

		private readonly Dictionary<int, Queue<SpellStatusData>> m_spellUsabilityQueue = new Dictionary<int, Queue<SpellStatusData>>();

		private readonly Dictionary<int, SpellStatusData> m_spellUsabilityInView = new Dictionary<int, SpellStatusData>();

		private readonly List<SpellStatus> m_spellStatusList = new List<SpellStatus>();

		private readonly Dictionary<EventCategory, List<int>> m_spellsPerCategoryInvalidatingStatus = new Dictionary<EventCategory, List<int>>();

		private readonly Dictionary<EventCategory, List<int>> m_spellsPerCategoryInvalidatingView = new Dictionary<EventCategory, List<int>>();

		private PlayerStatus m_playerStatus;

		private SpellStatusCellRenderer m_spellBeingCast;

		private readonly List<SpellStatusCellRenderer> m_spellsInDoneCasting = new List<SpellStatusCellRenderer>();

		private bool m_interactable;

		private CastHighlight m_castHighlight;

		private bool m_doneCasting;

		private readonly Queue<List<int>> m_refreshUsabilityQueue = new Queue<List<int>>();

		public FightTooltip tooltip => m_tooltip;

		public TooltipPosition tooltipPosition => m_tooltipPosition;

		private void Awake()
		{
			m_castEventListener = new CastEventListener();
			m_castEventListener.OnCastSpellDragBegin += OnCastSpellDragBegin;
			m_castEventListener.OnCastSpellDragEnd += OnCastSpellDragEnd;
			FightCastManager.OnTargetChange += OnFightMapTargetChanged;
			FightCastManager.OnUserActionEnd += OnFightMapUserActionEnd;
			m_spellList.SetCellRendererConfigurator(this);
		}

		private void OnDestroy()
		{
			FightCastManager.OnTargetChange -= OnFightMapTargetChanged;
			FightCastManager.OnUserActionEnd -= OnFightMapUserActionEnd;
		}

		private void OnFightMapTargetChanged(bool hasTarget, CellObject cellObject)
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			if (!(m_spellBeingCast == null))
			{
				CleanCastHighlight();
				if (hasTarget && cellObject != null)
				{
					DragNDropListener.instance.SnapDragToWorldPosition(CameraHandler.current.camera, cellObject.get_transform().get_position());
					m_spellBeingCast.OnEnterTarget();
					m_castHighlight = m_factory.CreateCastHighlight((SpellStatus)m_spellBeingCast.value, cellObject.highlight.get_transform());
				}
				else
				{
					DragNDropListener.instance.CancelSnapDrag();
					m_spellBeingCast.OnExitTarget();
				}
			}
		}

		private void OnFightMapUserActionEnd(FightCastState state)
		{
			if (!(null == m_spellBeingCast))
			{
				m_doneCasting = (state == FightCastState.DoneCasting);
				switch (state)
				{
				case FightCastState.Targeting:
					m_spellBeingCast.StartCast();
					break;
				case FightCastState.Cancelled:
					m_spellBeingCast.CancelCast();
					DragNDropListener.instance.CancelSnapDrag();
					m_spellBeingCast = null;
					CleanCastHighlight();
					break;
				case FightCastState.Casting:
					m_spellBeingCast.StartCast();
					DragNDropListener.instance.CancelSnapDrag();
					break;
				case FightCastState.DoneCasting:
					m_spellBeingCast.DoneCasting();
					CleanCastHighlight();
					m_spellsInDoneCasting.Add(m_spellBeingCast);
					m_spellBeingCast = null;
					break;
				default:
					throw new ArgumentOutOfRangeException("state", state, null);
				}
			}
		}

		private void OnCastSpellDragBegin(SpellStatusCellRenderer spellStatus, bool click)
		{
			m_spellBeingCast = spellStatus;
			FightMap.current.SetTargetInputMode(click ? AbstractFightMap.TargetInputMode.Click : AbstractFightMap.TargetInputMode.Drag);
			FightCastManager.StartCastingSpell(m_playerStatus, (SpellStatus)spellStatus.value);
		}

		private void OnCastSpellDragEnd(SpellStatusCellRenderer cellRenderer, bool onTarget)
		{
			if (m_spellsInDoneCasting.Contains(cellRenderer))
			{
				if (!m_doneCasting && !onTarget)
				{
					FightCastManager.StopCastingSpell(cancelled: true);
				}
				m_spellsInDoneCasting.Remove(cellRenderer);
			}
		}

		private void CleanCastHighlight()
		{
			if (m_castHighlight != null)
			{
				m_factory.DestroyCellHighlight(m_castHighlight);
				m_castHighlight = null;
			}
		}

		public void SetPlayerStatus(PlayerStatus playerStatus)
		{
			m_playerStatus = playerStatus;
		}

		public void SetInteractable(bool interactable)
		{
			m_interactable = interactable;
			m_spellList.UpdateAllConfigurators();
		}

		public void AddSpellStatus(SpellStatus spellStatus)
		{
			SpellStatusData data = default(SpellStatusData);
			CastValidityHelper.RecomputeSpellCastValidity(spellStatus.ownerPlayer, spellStatus, ref data);
			CastValidityHelper.RecomputeSpellCost(spellStatus, ref data);
			EnqueueSpellStatusData(spellStatus.instanceId, data);
			HashSet<EventCategory> hashSet = new HashSet<EventCategory>();
			foreach (EventCategory item in spellStatus.definition.eventsInvalidatingCost)
			{
				hashSet.Add(item);
			}
			foreach (EventCategory item2 in spellStatus.definition.eventsInvalidatingCasting)
			{
				hashSet.Add(item2);
			}
			AddFightEventListeners(spellStatus, hashSet, status: true);
		}

		public IEnumerator AddSpell(SpellStatus spellStatus)
		{
			DequeueSpellStatusEvent(spellStatus.instanceId, andUpdate: false);
			m_spellStatusList.Add(spellStatus);
			m_spellList.Insert(m_spellStatusList.Count - 1, spellStatus);
			HashSet<EventCategory> hashSet = new HashSet<EventCategory>();
			foreach (EventCategory item in spellStatus.definition.eventsInvalidatingCost)
			{
				hashSet.Add(item);
			}
			foreach (EventCategory item2 in spellStatus.definition.eventsInvalidatingCasting)
			{
				hashSet.Add(item2);
			}
			AddFightEventListeners(spellStatus, hashSet, status: false);
			yield break;
		}

		public void RemoveSpellStatus(int spellInstanceId)
		{
			int i = 0;
			for (int count = m_spellStatusList.Count; i < count && m_spellStatusList[i].instanceId != spellInstanceId; i++)
			{
			}
			if (i < m_spellStatusList.Count)
			{
				SpellStatus spellStatusToRemove = m_spellStatusList[i];
				RemoveFightEventListeners(spellStatusToRemove, status: true);
			}
		}

		public IEnumerator RemoveSpell(int spellInstanceId)
		{
			while (m_spellsInDoneCasting.Count != 0)
			{
				yield return null;
			}
			int i = 0;
			for (int count = m_spellStatusList.Count; i < count && m_spellStatusList[i].instanceId != spellInstanceId; i++)
			{
			}
			if (i < m_spellStatusList.Count)
			{
				SpellStatus spellStatusToRemove = m_spellStatusList[i];
				RemoveFightEventListeners(spellStatusToRemove, status: false);
				m_spellStatusList.RemoveAt(i);
				m_spellList.RemoveAt(i);
				m_spellUsabilityInView.Remove(spellInstanceId);
			}
		}

		public void RefreshUsability(PlayerStatus status, bool recomputeCosts)
		{
			List<int> list = new List<int>();
			IEnumerator<SpellStatus> spellStatusEnumerator = status.GetSpellStatusEnumerator();
			while (spellStatusEnumerator.MoveNext())
			{
				SpellStatus current = spellStatusEnumerator.Current;
				if (current != null)
				{
					SpellStatusData data = default(SpellStatusData);
					CastValidityHelper.RecomputeSpellCastValidity(current.ownerPlayer, current, ref data);
					CastValidityHelper.RecomputeSpellCost(current, ref data);
					EnqueueSpellStatusData(current.instanceId, data);
					list.Add(current.instanceId);
				}
			}
			m_refreshUsabilityQueue.Enqueue(list);
		}

		public IEnumerator UpdateUsability(bool recomputeCosts)
		{
			m_spellUsabilityInView.Clear();
			List<int> list = m_refreshUsabilityQueue.Dequeue();
			int i = 0;
			for (int count = list.Count; i < count; i++)
			{
				if (m_spellUsabilityQueue.TryGetValue(list[i], out Queue<SpellStatusData> value) && value.Count != 0)
				{
					m_spellUsabilityInView.Add(list[i], value.Dequeue());
				}
				else
				{
					Log.Error($"No SpellStatusData found in queue for spellInstanceId {list[i]}", 304, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\UI\\FightRework\\SpellBar\\SpellBarRework.cs");
				}
			}
			m_spellList.UpdateAllConfigurators();
			CheckQueue();
			yield break;
		}

		private void OnCastingValidityUpdated(EventCategory category)
		{
			if (m_spellsPerCategoryInvalidatingStatus.TryGetValue(category, out List<int> value))
			{
				foreach (int item in value)
				{
					if (m_playerStatus.TryGetSpell(item, out SpellStatus spellStatus))
					{
						SpellStatusData data = default(SpellStatusData);
						CastValidityHelper.RecomputeSpellCost(spellStatus, ref data);
						CastValidityHelper.RecomputeSpellCastValidity(spellStatus.ownerPlayer, spellStatus, ref data);
						EnqueueSpellStatusData(spellStatus.instanceId, data);
					}
				}
			}
		}

		private void OnCastingValidityViewUpdated(EventCategory category)
		{
			if (m_spellsPerCategoryInvalidatingView.TryGetValue(category, out List<int> value))
			{
				foreach (int item in value)
				{
					DequeueSpellStatusEvent(item, andUpdate: true);
				}
			}
		}

		private void EnqueueSpellStatusData(int statusId, SpellStatusData spellStatusData)
		{
			if (!m_spellUsabilityQueue.TryGetValue(statusId, out Queue<SpellStatusData> value))
			{
				value = m_queuePool.Get();
				m_spellUsabilityQueue.Add(statusId, value);
			}
			value.Enqueue(spellStatusData);
			CheckQueue();
		}

		private void DequeueSpellStatusEvent(int spellStatusId, bool andUpdate)
		{
			if (m_spellUsabilityQueue.TryGetValue(spellStatusId, out Queue<SpellStatusData> value))
			{
				if (m_spellUsabilityInView.ContainsKey(spellStatusId))
				{
					m_spellUsabilityInView[spellStatusId] = value.Dequeue();
				}
				else
				{
					m_spellUsabilityInView.Add(spellStatusId, value.Dequeue());
				}
				if (TryGetSpellStatusByInstanceId(spellStatusId, out SpellStatus spellStatus) && andUpdate)
				{
					m_spellList.UpdateConfiguratorWithValue(spellStatus);
				}
				CheckQueue();
			}
		}

		private bool TryGetSpellStatusByInstanceId(int instanceId, out SpellStatus spellStatus)
		{
			spellStatus = null;
			int i = 0;
			for (int count = m_spellStatusList.Count; i < count; i++)
			{
				SpellStatus spellStatus2 = m_spellStatusList[i];
				if (spellStatus2.instanceId == instanceId)
				{
					spellStatus = spellStatus2;
					return true;
				}
			}
			return false;
		}

		private void CheckQueue()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<int, Queue<SpellStatusData>> item in m_spellUsabilityQueue)
			{
				stringBuilder.Append($"spell {item.Key}: count={item.Value.Count} - ");
			}
		}

		private void AddFightEventListeners(SpellStatus spellStatus, IEnumerable<EventCategory> eventsToAdd, bool status)
		{
			Dictionary<EventCategory, List<int>> dictionary = status ? m_spellsPerCategoryInvalidatingStatus : m_spellsPerCategoryInvalidatingView;
			foreach (EventCategory item in eventsToAdd)
			{
				if (!dictionary.TryGetValue(item, out List<int> value))
				{
					value = new List<int>();
					dictionary.Add(item, value);
					if (status)
					{
						FightLogicExecutor.AddListenerUpdateStatus(spellStatus.ownerPlayer.fightId, OnCastingValidityUpdated, item);
					}
					else
					{
						FightLogicExecutor.AddListenerUpdateView(spellStatus.ownerPlayer.fightId, OnCastingValidityViewUpdated, item);
					}
				}
				value.Add(spellStatus.instanceId);
			}
		}

		private void RemoveFightEventListeners(SpellStatus spellStatusToRemove, bool status)
		{
			List<EventCategory> list = new List<EventCategory>();
			Dictionary<EventCategory, List<int>> dictionary = status ? m_spellsPerCategoryInvalidatingStatus : m_spellsPerCategoryInvalidatingView;
			foreach (KeyValuePair<EventCategory, List<int>> item in dictionary)
			{
				for (int num = item.Value.Count - 1; num >= 0; num--)
				{
					if (item.Value[num] == spellStatusToRemove.instanceId)
					{
						item.Value.RemoveAt(num);
						break;
					}
				}
				if (item.Value.Count == 0)
				{
					list.Add(item.Key);
				}
			}
			foreach (EventCategory item2 in list)
			{
				if (status)
				{
					FightLogicExecutor.RemoveListenerUpdateStatus(spellStatusToRemove.ownerPlayer.fightId, OnCastingValidityUpdated, item2);
				}
				else
				{
					FightLogicExecutor.RemoveListenerUpdateView(spellStatusToRemove.ownerPlayer.fightId, OnCastingValidityViewUpdated, item2);
				}
				dictionary.Remove(item2);
			}
		}

		public IDragNDropValidator GetDragNDropValidator()
		{
			return this;
		}

		public bool IsParentInteractable()
		{
			if (m_interactable)
			{
				return m_spellBeingCast == null;
			}
			return false;
		}

		public SpellStatusData? GetSpellStatusData(SpellStatus spellStatus)
		{
			if (spellStatus == null)
			{
				return null;
			}
			if (m_spellUsabilityInView.TryGetValue(spellStatus.instanceId, out SpellStatusData value))
			{
				return value;
			}
			return null;
		}

		public CastEventListener GetEventListener()
		{
			return m_castEventListener;
		}

		public bool IsValidDrag(object value)
		{
			if (FightCastManager.currentCastType != 0)
			{
				return false;
			}
			SpellStatus spellStatus = (SpellStatus)value;
			if (CastValidityHelper.ComputeSpellCostCastValidity(m_playerStatus, spellStatus) != 0)
			{
				return false;
			}
			CastValidity castValidity = CastValidityHelper.ComputeSpellCastValidity(m_playerStatus, spellStatus);
			if (castValidity != 0)
			{
				NotificationWindowManager.DisplayNotification(TextCollectionUtility.GetFormattedText(castValidity));
			}
			return castValidity == CastValidity.SUCCESS;
		}

		public bool IsValidDrop(object value)
		{
			return true;
		}

		public SpellBarRework()
			: this()
		{
		}
	}
}
