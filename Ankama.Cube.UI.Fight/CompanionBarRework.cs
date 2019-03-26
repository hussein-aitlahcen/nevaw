using Ankama.Cube.Data;
using Ankama.Cube.Fight;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps;
using Ankama.Cube.Maps.Objects;
using Ankama.Cube.Protocols.FightCommonProtocol;
using Ankama.Cube.UI.Components;
using Ankama.Cube.UI.DeckMaker;
using Ankama.Cube.UI.Fight.NotificationWindow;
using Ankama.Cube.UI.Fight.Windows;
using Ankama.Cube.Utility;
using Ankama.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.UI.Fight
{
	public sealed class CompanionBarRework : MonoBehaviour, ICompanionStatusCellRendererConfigurator, ICompanionCellRendererConfigurator, IWithTooltipCellRendererConfigurator, ICellRendererConfigurator, IDragNDropValidator
	{
		[SerializeField]
		private DynamicList m_companionList;

		[SerializeField]
		private DynamicList m_additionalCompanionList;

		[SerializeField]
		private FightTooltip m_tooltip;

		[SerializeField]
		private TooltipPosition m_tooltipPosition;

		[SerializeField]
		private FightUIFactory m_factory;

		private readonly ObjectPool<Queue<CompanionStatusData>> m_queuePool = new ObjectPool<Queue<CompanionStatusData>>();

		private readonly Dictionary<int, Queue<CompanionStatusData>> m_companionStatusQueue = new Dictionary<int, Queue<CompanionStatusData>>();

		private readonly Dictionary<int, CompanionStatusData> m_companionStatusInView = new Dictionary<int, CompanionStatusData>();

		private readonly List<ReserveCompanionStatus> m_companionStatusList = new List<ReserveCompanionStatus>();

		private readonly List<ReserveCompanionStatus> m_additionalCompanionStatusList = new List<ReserveCompanionStatus>();

		private readonly Dictionary<EventCategory, List<int>> m_companionsPerCategoryInvalidatingStatus = new Dictionary<EventCategory, List<int>>();

		private readonly Dictionary<EventCategory, List<int>> m_companionsPerCategoryInvalidatingView = new Dictionary<EventCategory, List<int>>();

		private CastEventListener m_castEventListener;

		private PlayerStatus m_playerStatus;

		private CompanionStatusCellRenderer m_companionBeingCast;

		private bool m_interactable;

		private CastHighlight m_castHighlight;

		public FightTooltip tooltip => m_tooltip;

		public TooltipPosition tooltipPosition => m_tooltipPosition;

		public void SetPlayerStatus(PlayerStatus playerStatus)
		{
			m_playerStatus = playerStatus;
		}

		public void SetInteractable(bool interactable)
		{
			m_interactable = interactable;
			m_companionList.UpdateAllConfigurators();
			m_additionalCompanionList.UpdateAllConfigurators();
		}

		private void Awake()
		{
			m_companionList.SetCellRendererConfigurator(this);
			m_additionalCompanionList.SetCellRendererConfigurator(this);
			m_castEventListener = new CastEventListener();
			m_castEventListener.OnCastCompanionDragBegin += OnCastCompanionDragBegin;
			m_castEventListener.OnCastCompanionDragEnd += OnCastCompanionDragEnd;
			FightCastManager.OnTargetChange += OnFightMapTargetChanged;
			FightCastManager.OnUserActionEnd += OnFightMapUserActionEnd;
		}

		private void OnDestroy()
		{
			FightCastManager.OnTargetChange -= OnFightMapTargetChanged;
			FightCastManager.OnUserActionEnd -= OnFightMapUserActionEnd;
		}

		private void OnFightMapTargetChanged(bool hasTarget, CellObject cellObject)
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			if (!(m_companionBeingCast == null))
			{
				CleanCastHighlight();
				if (hasTarget && cellObject != null)
				{
					DragNDropListener.instance.SnapDragToWorldPosition(CameraHandler.current.camera, cellObject.get_transform().get_position());
					m_companionBeingCast.OnEnterTarget();
					m_castHighlight = m_factory.CreateCastHighlight((ReserveCompanionStatus)m_companionBeingCast.value, cellObject.highlight.get_transform());
				}
				else
				{
					DragNDropListener.instance.CancelSnapDrag();
					m_companionBeingCast.OnExitTarget();
				}
			}
		}

		private void OnFightMapUserActionEnd(FightCastState state)
		{
			if (!(null == m_companionBeingCast))
			{
				switch (state)
				{
				case FightCastState.Targeting:
					m_companionBeingCast.StartCast();
					break;
				case FightCastState.Cancelled:
					m_companionBeingCast.CancelCast();
					DragNDropListener.instance.CancelSnapDrag();
					m_companionBeingCast = null;
					CleanCastHighlight();
					break;
				case FightCastState.Casting:
					m_companionBeingCast.StartCast();
					DragNDropListener.instance.CancelSnapDrag();
					break;
				case FightCastState.DoneCasting:
					m_companionBeingCast.DoneCasting();
					m_companionBeingCast = null;
					CleanCastHighlight();
					break;
				default:
					throw new ArgumentOutOfRangeException("state", state, null);
				}
			}
		}

		private void OnCastCompanionDragBegin(CompanionStatusCellRenderer cellRenderer, bool click)
		{
			m_companionBeingCast = cellRenderer;
			FightMap.current.SetTargetInputMode(click ? AbstractFightMap.TargetInputMode.Click : AbstractFightMap.TargetInputMode.Drag);
			FightCastManager.StartInvokingCompanion(m_playerStatus, (ReserveCompanionStatus)cellRenderer.value);
		}

		private void OnCastCompanionDragEnd(CompanionStatusCellRenderer cellRenderer, bool onTarget)
		{
			if (!(m_companionBeingCast == null))
			{
				m_companionBeingCast = null;
				if (!onTarget)
				{
					FightCastManager.StopInvokingCompanion(cancelled: true);
				}
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

		public void RefreshUsability(PlayerStatus playerStatus)
		{
			foreach (ReserveCompanionStatus item in playerStatus.GetAvailableCompanionStatusEnumerator())
			{
				EnqueueCompanionStatusData(playerStatus, item);
			}
			foreach (ReserveCompanionStatus item2 in playerStatus.GetAdditionalCompanionStatusEnumerator())
			{
				EnqueueCompanionStatusData(playerStatus, item2);
			}
		}

		public void AddCompanionStatus(PlayerStatus playerStatus, ReserveCompanionStatus companion)
		{
			EnqueueCompanionStatusData(playerStatus, companion);
			HashSet<EventCategory> hashSet = new HashSet<EventCategory>();
			foreach (EventCategory item in companion.definition.eventsInvalidatingCost)
			{
				hashSet.Add(item);
			}
			foreach (EventCategory item2 in companion.definition.eventsInvalidatingCasting)
			{
				hashSet.Add(item2);
			}
			AddFightEventListeners(companion, hashSet, status: true);
		}

		public void ChangeCompanionStateStatus(PlayerStatus playerStatus, int companionDefinitionId, CompanionReserveState state)
		{
			if (playerStatus.TryGetCompanion(companionDefinitionId, out ReserveCompanionStatus companionStatus))
			{
				EnqueueCompanionStatusData(playerStatus, companionStatus);
			}
		}

		public void RemoveCompanionStatus(int companionDefinitionId)
		{
			RemoveFightEventListeners(companionDefinitionId, m_playerStatus.fightId, status: true);
		}

		public IEnumerator UpdateAvailableCompanions()
		{
			m_companionStatusInView.Clear();
			foreach (KeyValuePair<int, Queue<CompanionStatusData>> item in m_companionStatusQueue)
			{
				Queue<CompanionStatusData> value = item.Value;
				if (value.Count != 0)
				{
					m_companionStatusInView.Add(item.Key, value.Dequeue());
				}
			}
			m_companionList.UpdateAllConfigurators();
			m_additionalCompanionList.UpdateAllConfigurators();
			yield break;
		}

		public IEnumerator AddCompanion(ReserveCompanionStatus companion)
		{
			DequeueCompanionStatusData(companion, andUpdate: false);
			HashSet<EventCategory> hashSet = new HashSet<EventCategory>();
			foreach (EventCategory item in companion.definition.eventsInvalidatingCost)
			{
				hashSet.Add(item);
			}
			foreach (EventCategory item2 in companion.definition.eventsInvalidatingCasting)
			{
				hashSet.Add(item2);
			}
			AddFightEventListeners(companion, hashSet, status: false);
			if (companion.isGiven)
			{
				m_additionalCompanionStatusList.Add(companion);
				m_additionalCompanionList.Insert(m_additionalCompanionStatusList.Count - 1, companion);
			}
			else
			{
				m_companionStatusList.Add(companion);
				m_companionList.Insert(m_companionStatusList.Count - 1, companion);
			}
			yield break;
		}

		public IEnumerator ChangeCompanionState(PlayerStatus playerStatus, int companionDefinitionId)
		{
			if (playerStatus.TryGetCompanion(companionDefinitionId, out ReserveCompanionStatus companionStatus))
			{
				DequeueCompanionStatusData(companionStatus, andUpdate: true);
			}
			yield break;
		}

		public IEnumerator RemoveCompanion(int companionDefinitionId)
		{
			int i = 0;
			for (int count = m_additionalCompanionStatusList.Count; i < count && m_additionalCompanionStatusList[i].definition.get_id() != companionDefinitionId; i++)
			{
			}
			if (i < m_additionalCompanionStatusList.Count)
			{
				ReserveCompanionStatus reserveCompanionStatus = m_additionalCompanionStatusList[i];
				RemoveFightEventListeners(reserveCompanionStatus.definition.get_id(), m_playerStatus.fightId, status: false);
				m_additionalCompanionStatusList.RemoveAt(i);
				m_additionalCompanionList.RemoveAt(i);
				m_companionStatusQueue.Remove(companionDefinitionId);
				m_companionStatusInView.Remove(companionDefinitionId);
			}
			yield break;
		}

		private void OnCastingValidityUpdated(EventCategory category)
		{
			if (m_companionsPerCategoryInvalidatingStatus.TryGetValue(category, out List<int> value))
			{
				foreach (int item in value)
				{
					if (m_playerStatus.TryGetCompanion(item, out ReserveCompanionStatus companionStatus))
					{
						EnqueueCompanionStatusData(m_playerStatus, companionStatus);
					}
				}
			}
		}

		private void OnCastingValidityViewUpdated(EventCategory category)
		{
			if (m_companionsPerCategoryInvalidatingView.TryGetValue(category, out List<int> value))
			{
				foreach (int item in value)
				{
					if (m_playerStatus.TryGetCompanion(item, out ReserveCompanionStatus companionStatus))
					{
						DequeueCompanionStatusData(companionStatus, andUpdate: true);
					}
				}
			}
		}

		private void EnqueueCompanionStatusData(PlayerStatus playerStatus, ReserveCompanionStatus companion)
		{
			CompanionStatusData companionStatusData = default(CompanionStatusData);
			companionStatusData.state = companion.state;
			companionStatusData.isGiven = companion.isGiven;
			CompanionStatusData data = companionStatusData;
			CastValidityHelper.RecomputeCompanionCastValidity(playerStatus, companion, ref data);
			CastValidityHelper.RecomputeCompanionCost(companion, ref data);
			int id = companion.definition.get_id();
			if (!m_companionStatusQueue.TryGetValue(id, out Queue<CompanionStatusData> value))
			{
				value = m_queuePool.Get();
				m_companionStatusQueue.Add(id, value);
			}
			value.Enqueue(data);
		}

		private void DequeueCompanionStatusData(ReserveCompanionStatus companionStatus, bool andUpdate)
		{
			if (!m_companionStatusQueue.TryGetValue(companionStatus.definition.get_id(), out Queue<CompanionStatusData> value) || value.Count == 0)
			{
				return;
			}
			if (m_companionStatusInView.ContainsKey(companionStatus.definition.get_id()))
			{
				m_companionStatusInView[companionStatus.definition.get_id()] = value.Dequeue();
			}
			else
			{
				m_companionStatusInView.Add(companionStatus.definition.get_id(), value.Dequeue());
			}
			if (andUpdate)
			{
				if (companionStatus.isGiven)
				{
					m_additionalCompanionList.UpdateAllConfigurators();
				}
				else
				{
					m_companionList.UpdateAllConfigurators();
				}
			}
		}

		private void AddFightEventListeners(ReserveCompanionStatus companionStatus, IEnumerable<EventCategory> eventsToAdd, bool status)
		{
			foreach (EventCategory item in eventsToAdd)
			{
				AddFightEventListener(companionStatus, item, status);
			}
			AddFightEventListener(companionStatus, EventCategory.ElementPointsChanged, status);
		}

		private void AddFightEventListener(ReserveCompanionStatus companionStatus, EventCategory eventCategory, bool status)
		{
			Dictionary<EventCategory, List<int>> dictionary = status ? m_companionsPerCategoryInvalidatingStatus : m_companionsPerCategoryInvalidatingView;
			if (!dictionary.TryGetValue(eventCategory, out List<int> value))
			{
				value = new List<int>();
				dictionary.Add(eventCategory, value);
				if (status)
				{
					FightLogicExecutor.AddListenerUpdateStatus(companionStatus.ownerPlayer.fightId, OnCastingValidityUpdated, eventCategory);
				}
				else
				{
					FightLogicExecutor.AddListenerUpdateView(companionStatus.ownerPlayer.fightId, OnCastingValidityViewUpdated, eventCategory);
				}
			}
			value.Add(companionStatus.definition.get_id());
		}

		private void RemoveFightEventListeners(int companionDefinitionId, int fightId, bool status)
		{
			List<EventCategory> list = new List<EventCategory>();
			Dictionary<EventCategory, List<int>> dictionary = status ? m_companionsPerCategoryInvalidatingStatus : m_companionsPerCategoryInvalidatingView;
			foreach (KeyValuePair<EventCategory, List<int>> item in dictionary)
			{
				for (int num = item.Value.Count - 1; num >= 0; num--)
				{
					if (item.Value[num] == companionDefinitionId)
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
					FightLogicExecutor.RemoveListenerUpdateStatus(fightId, OnCastingValidityUpdated, item2);
				}
				else
				{
					FightLogicExecutor.RemoveListenerUpdateView(fightId, OnCastingValidityViewUpdated, item2);
				}
				dictionary.Remove(item2);
			}
		}

		public CompanionStatusData? GetCompanionStatusData(ReserveCompanionStatus companion)
		{
			if (companion == null)
			{
				return null;
			}
			if (m_companionStatusInView.TryGetValue(companion.definition.get_id(), out CompanionStatusData value))
			{
				return value;
			}
			return null;
		}

		public CastEventListener GetEventListener()
		{
			return m_castEventListener;
		}

		public bool IsParentInteractable()
		{
			return m_interactable;
		}

		public IDragNDropValidator GetDragNDropValidator()
		{
			return this;
		}

		public bool IsValidDrag(object value)
		{
			if (FightCastManager.currentCastType != 0)
			{
				return false;
			}
			ReserveCompanionStatus companionStatus = (ReserveCompanionStatus)value;
			if (CastValidityHelper.ComputeCompanionCostCastValidity(m_playerStatus, companionStatus) != 0)
			{
				return false;
			}
			CastValidity castValidity = CastValidityHelper.ComputeCompanionCastValidity(m_playerStatus, companionStatus);
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

		public CompanionBarRework()
			: this()
		{
		}
	}
}
