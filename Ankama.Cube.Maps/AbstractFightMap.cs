using Ankama.Cube.Data;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Fight.Movement;
using Ankama.Cube.Maps.Feedbacks;
using Ankama.Cube.Maps.Objects;
using Ankama.Cube.SRP;
using Ankama.Utilities;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Maps
{
	public abstract class AbstractFightMap : MonoBehaviour, IMap
	{
		protected enum InteractiveMode
		{
			None,
			Movement,
			Target
		}

		public enum TargetInputMode
		{
			Drag,
			Click
		}

		private struct CellObjectAnimationInstance
		{
			private readonly CellObjectAnimationParameters m_parameters;

			private readonly Vector2Int m_origin;

			private readonly Quaternion m_rotation;

			private readonly float m_strength;

			private readonly float m_time;

			private readonly Vector2Int m_minBounds;

			private readonly Vector2Int m_maxBounds;

			public CellObjectAnimationInstance(CellObjectAnimationParameters parameters, Vector2Int origin, Quaternion rotation, float strength, float time)
			{
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				//IL_0009: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				//IL_0027: Unknown result type (might be due to invalid IL or missing references)
				m_parameters = parameters;
				m_origin = origin;
				m_rotation = rotation;
				m_strength = strength;
				m_time = time;
				parameters.GetBounds(origin, rotation, time, out m_minBounds, out m_maxBounds);
			}

			public float Resolve(Vector2Int coords)
			{
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Unknown result type (might be due to invalid IL or missing references)
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				//IL_0045: Unknown result type (might be due to invalid IL or missing references)
				//IL_004a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0066: Unknown result type (might be due to invalid IL or missing references)
				//IL_0068: Unknown result type (might be due to invalid IL or missing references)
				//IL_006e: Unknown result type (might be due to invalid IL or missing references)
				int x = coords.get_x();
				int y = coords.get_y();
				int num = x;
				Vector2Int val = m_minBounds;
				if (num >= val.get_x())
				{
					int num2 = x;
					val = m_maxBounds;
					if (num2 <= val.get_x())
					{
						int num3 = y;
						val = m_minBounds;
						if (num3 >= val.get_y())
						{
							int num4 = y;
							val = m_maxBounds;
							if (num4 <= val.get_y())
							{
								return m_strength * m_parameters.Compute(coords, m_origin, m_rotation, m_time);
							}
						}
					}
				}
				return 0f;
			}
		}

		[SerializeField]
		[HideInInspector]
		protected CellObject[] m_cellObjects = new CellObject[0];

		[SerializeField]
		protected FightMapFeedbackResources m_feedbackResources;

		[SerializeField]
		protected float m_gravity = -9.81f;

		[NonSerialized]
		public Action<Target?> onTargetSelected;

		[NonSerialized]
		public Action<Target?, CellObject> onTargetChanged;

		protected IMapDefinition m_mapDefinition;

		protected InteractiveMode m_interactiveMode;

		protected FightMapInputHandler m_inputHandler;

		private TargetInputMode m_targetInputMode = TargetInputMode.Click;

		protected readonly FightPathFinder m_pathFinder = new FightPathFinder();

		protected readonly FightPathFinderFeedbackManager m_pathFinderFeedbackManager = new FightPathFinderFeedbackManager();

		protected readonly FightMapCellPointerManager m_cellPointerManager = new FightMapCellPointerManager();

		protected bool m_feedbackNeedsUpdate;

		protected FightMapMovementContext[] m_movementContexts;

		protected FightMapMovementContext m_activeMovementContext;

		protected FightMapMovementContext m_localMovementContext;

		protected FightMapTargetContext m_targetContext;

		protected CellObject[] m_cellObjectsByIndex;

		protected MapVirtualGrid m_virtualGrid;

		private readonly List<CellObject> m_referenceCells = new List<CellObject>(64);

		private readonly List<CellObject> m_linkedCells = new List<CellObject>(1);

		private readonly List<CellObjectAnimationInstance> m_cellObjectAnimationInstances = new List<CellObjectAnimationInstance>(2);

		private bool m_virtualGridIsDirty = true;

		private bool m_cellObjectNeedsCleanup;

		private IObjectWithFocus m_objectFocusedByCursor;

		[PublicAPI]
		public void SetNoInteractionPhase()
		{
			EndCurrentPhase();
			m_interactiveMode = InteractiveMode.None;
		}

		[PublicAPI]
		public bool IsInMovementPhase()
		{
			return m_interactiveMode == InteractiveMode.Movement;
		}

		[PublicAPI]
		public void SetMovementPhase()
		{
			EndCurrentPhase();
			m_interactiveMode = InteractiveMode.Movement;
			FightMapMovementContext localMovementContext = m_localMovementContext;
			if (localMovementContext != null)
			{
				m_cellPointerManager.BeginHighlightingPlayableCharacters(this, localMovementContext.entityProvider);
			}
		}

		[PublicAPI]
		public bool IsInTargetingPhase()
		{
			return m_interactiveMode == InteractiveMode.Target;
		}

		[PublicAPI]
		public void SetTargetingPhase([NotNull] IEnumerable<Target> targets)
		{
			if (m_targetContext == null)
			{
				Log.Error("Targeting phase requested but no target context exists.", 130, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\AbstractFightMap.cs");
				return;
			}
			EndCurrentPhase();
			m_interactiveMode = InteractiveMode.Target;
			m_targetContext.Begin(targets);
			m_cellPointerManager.SetCharacterFocusLayer();
			m_feedbackNeedsUpdate = true;
		}

		[PublicAPI]
		public void EndCurrentPhase()
		{
			switch (m_interactiveMode)
			{
			case InteractiveMode.None:
				return;
			case InteractiveMode.Movement:
				if (m_pathFinder.tracking)
				{
					m_pathFinder.End();
					m_activeMovementContext.UpdateTarget(null);
					m_cellPointerManager.SetAnimatedCursor(value: false);
					m_feedbackNeedsUpdate = true;
				}
				m_cellPointerManager.EndHighlightingPlayableCharacters();
				break;
			case InteractiveMode.Target:
				if (m_targetContext != null && m_targetContext.End())
				{
					m_cellPointerManager.SetDefaultLayer();
					m_feedbackNeedsUpdate = true;
				}
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			m_inputHandler.SetDirty();
			m_interactiveMode = InteractiveMode.None;
		}

		[PublicAPI]
		public bool HasAvailableTarget()
		{
			if (m_targetContext != null)
			{
				return m_targetContext.isActive;
			}
			return false;
		}

		[PublicAPI]
		public void SetTargetInputMode(TargetInputMode targetMode)
		{
			m_targetInputMode = targetMode;
		}

		protected void Create()
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			IMapDefinition mapDefinition = m_mapDefinition;
			Vector2Int val = mapDefinition.sizeMax - mapDefinition.sizeMin;
			m_cellObjectsByIndex = new CellObject[val.get_x() * val.get_y()];
			int num = m_cellObjects.Length;
			for (int i = 0; i < num; i++)
			{
				CellObject cellObject = m_cellObjects[i];
				Vector2Int coords = cellObject.coords;
				int cellIndex = mapDefinition.GetCellIndex(coords.get_x(), coords.get_y());
				m_cellObjectsByIndex[cellIndex] = cellObject;
				cellObject.Initialize(this);
			}
			m_virtualGrid = new MapVirtualGrid(mapDefinition, m_cellObjects);
			CreateCellHighlights();
		}

		protected unsafe BoxCollider CreateCollider()
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			IMapDefinition mapDefinition = m_mapDefinition;
			Vector2Int sizeMin = mapDefinition.sizeMin;
			Vector2 val = Vector2Int.op_Implicit(mapDefinition.sizeMax - sizeMin);
			Vector3 val2 = default(Vector3);
			val2._002Ector(-0.5f, 0f, -0.5f);
			Vector3 val3 = default(Vector3);
			val3._002Ector((float)sizeMin.get_x(), 0f, (float)sizeMin.get_y());
			Vector3 val4 = default(Vector3);
			val4._002Ector(((IntPtr)(void*)val).x, 0f, ((IntPtr)(void*)val).y);
			BoxCollider obj = this.get_gameObject().AddComponent<BoxCollider>();
			obj.set_center(Vector3Int.op_Implicit(mapDefinition.origin) + val3 + 0.5f * val4 + val2);
			obj.set_size(new Vector3(((IntPtr)(void*)val).x, 0f, ((IntPtr)(void*)val).y));
			obj.set_isTrigger(true);
			return obj;
		}

		protected void InitializeHandlers(BoxCollider mapCollider, bool giveUserControl)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			CameraHandler current = CameraHandler.current;
			current.Initialize(m_mapDefinition, mapCollider.get_bounds(), giveUserControl);
			m_inputHandler = new FightMapInputHandler(mapCollider, current);
		}

		private void CreateCellHighlights()
		{
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Expected O, but got Unknown
			uint doNotRenderInReflectionRenderMask = LayerMaskNames.doNotRenderInReflectionRenderMask;
			FightMapFeedbackResources feedbackResources = m_feedbackResources;
			Material val;
			Material val2;
			if (null == feedbackResources)
			{
				val = null;
				val2 = null;
				Log.Error("Fight map has no feedback resources.", 271, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\AbstractFightMap.cs");
			}
			else
			{
				val = feedbackResources.areaFeedbackMaterial;
				if (null == val)
				{
					Log.Error("Feedback resources named '" + feedbackResources.get_name() + "' has no highlight material.", 278, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\AbstractFightMap.cs");
				}
				val2 = feedbackResources.movementFeedbackMaterial;
				if (null == val2)
				{
					Log.Error("Feedback resources named '" + feedbackResources.get_name() + "' has no movement highlight material.", 284, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\AbstractFightMap.cs");
				}
			}
			GameObject val3 = new GameObject("Highlight");
			CellHighlight prefab = val3.AddComponent<CellHighlight>();
			CellObject[] cellObjects = m_cellObjects;
			int num = cellObjects.Length;
			for (int i = 0; i < num; i++)
			{
				cellObjects[i].CreateHighlight(prefab, val, doNotRenderInReflectionRenderMask);
			}
			Object.Destroy(val3);
			m_pathFinderFeedbackManager.Initialize(this, val2, doNotRenderInReflectionRenderMask);
			m_cellPointerManager.Initialize(feedbackResources.movementFeedbackResources, val2, doNotRenderInReflectionRenderMask);
		}

		private void OnDestroy()
		{
			m_pathFinderFeedbackManager.Release();
			m_cellPointerManager.Release();
		}

		protected abstract void ApplyMovement(Vector2Int[] path, [NotNull] ICharacterEntity trackedCharacter, [CanBeNull] IEntityWithBoardPresence targetedEntity);

		[UsedImplicitly]
		protected virtual void Update()
		{
			UpdateSystems();
			switch (m_interactiveMode)
			{
			case InteractiveMode.None:
				UpdatePreviewMode();
				break;
			case InteractiveMode.Movement:
				UpdateMovementMode();
				break;
			case InteractiveMode.Target:
				UpdateTargetMode();
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		private void LateUpdate()
		{
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			if (m_feedbackNeedsUpdate)
			{
				UpdateFeedbacks();
				m_feedbackNeedsUpdate = false;
			}
			MapVirtualGrid virtualGrid = m_virtualGrid;
			List<CellObject> referenceCells = m_referenceCells;
			List<CellObject> linkedCells = m_linkedCells;
			List<CellObjectAnimationInstance> cellObjectAnimationInstances = m_cellObjectAnimationInstances;
			float deltaTime = Time.get_deltaTime();
			float gravityVelocity = deltaTime * m_gravity;
			if (m_virtualGridIsDirty)
			{
				referenceCells.Clear();
				virtualGrid.GetReferenceCellsNoAlloc(referenceCells);
				m_virtualGridIsDirty = false;
			}
			int count = referenceCells.Count;
			int count2 = cellObjectAnimationInstances.Count;
			if (count2 > 0)
			{
				for (int i = 0; i < count2; i++)
				{
					CellObjectAnimationInstance cellObjectAnimationInstance = cellObjectAnimationInstances[i];
					for (int j = 0; j < count; j++)
					{
						CellObject cellObject = referenceCells[j];
						float heightDelta = cellObjectAnimationInstance.Resolve(cellObject.coords);
						cellObject.ApplyAnimation(heightDelta);
					}
				}
				cellObjectAnimationInstances.Clear();
				m_cellObjectNeedsCleanup = true;
			}
			else if (m_cellObjectNeedsCleanup)
			{
				bool flag = false;
				for (int k = 0; k < count; k++)
				{
					flag = (referenceCells[k].CleanupAnimation(deltaTime, gravityVelocity) | flag);
				}
				m_cellObjectNeedsCleanup = flag;
			}
			for (int l = 0; l < count; l++)
			{
				CellObject cellObject2 = referenceCells[l];
				bool isSleeping = cellObject2.ResolvePhysics(deltaTime, gravityVelocity);
				linkedCells.Clear();
				virtualGrid.GetLinkedCellsNoAlloc(cellObject2.coords, linkedCells);
				int count3 = linkedCells.Count;
				for (int m = 0; m < count3; m++)
				{
					linkedCells[m].CopyPhysics(cellObject2, isSleeping, deltaTime, gravityVelocity);
				}
			}
		}

		private void UpdateSystems()
		{
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_0212: Unknown result type (might be due to invalid IL or missing references)
			//IL_0222: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			FightMapInputHandler inputHandler = m_inputHandler;
			if (!inputHandler.Update(m_mapDefinition))
			{
				return;
			}
			if (m_objectFocusedByCursor != null)
			{
				m_objectFocusedByCursor.SetFocus(value: false);
				m_objectFocusedByCursor = null;
			}
			FightMapCellPointerManager cellPointerManager = m_cellPointerManager;
			Vector2Int? targetCell = inputHandler.targetCell;
			if (targetCell.HasValue)
			{
				Vector2Int value = targetCell.Value;
				if (TryGetRegionIndex(value, out int regionIndex))
				{
					FightMapMovementContext fightMapMovementContext = m_movementContexts[regionIndex];
					fightMapMovementContext.entityProvider.TryGetEntityAt(value, out IEntityWithBoardPresence character);
					CellObject referenceCell = m_virtualGrid.GetReferenceCell(value);
					if (null != referenceCell)
					{
						cellPointerManager.SetCursorPosition(referenceCell);
						cellPointerManager.ShowCursor();
					}
					else
					{
						cellPointerManager.HideCursor();
					}
					IObjectWithFocus objectWithFocus;
					if (character != null && (objectWithFocus = (character.view as IObjectWithFocus)) != null)
					{
						objectWithFocus.SetFocus(value: true);
						m_objectFocusedByCursor = objectWithFocus;
					}
					if (m_pathFinder.tracking)
					{
						if (fightMapMovementContext == m_activeMovementContext)
						{
							ICharacterEntity trackedCharacter = fightMapMovementContext.trackedCharacter;
							bool flag;
							if (fightMapMovementContext.canDoActionOnTarget)
							{
								IEntityWithBoardPresence entityWithBoardPresence = null;
								if (character != null && character != trackedCharacter && (fightMapMovementContext.GetCell(value).state & FightMapMovementContext.CellState.Targetable) != 0)
								{
									entityWithBoardPresence = character;
								}
								fightMapMovementContext.UpdateTarget(entityWithBoardPresence);
								flag = (entityWithBoardPresence != null);
							}
							else
							{
								flag = false;
							}
							if (fightMapMovementContext.canMove)
							{
								if (flag)
								{
									if (trackedCharacter.hasRange)
									{
										m_pathFinder.Reset();
									}
									else
									{
										m_pathFinder.Move(fightMapMovementContext.stateProvider, fightMapMovementContext.grid, value, isTargeting: true);
									}
								}
								else if ((fightMapMovementContext.GetCell(value).state & (FightMapMovementContext.CellState.Reachable | FightMapMovementContext.CellState.Occupied)) == FightMapMovementContext.CellState.Reachable)
								{
									m_pathFinder.Move(fightMapMovementContext.stateProvider, fightMapMovementContext.grid, value, isTargeting: false);
								}
								else
								{
									m_pathFinder.Reset();
								}
							}
							cellPointerManager.SetAnimatedCursor(flag);
						}
						else
						{
							m_pathFinder.Reset();
							cellPointerManager.SetAnimatedCursor(value: false);
						}
						m_feedbackNeedsUpdate = true;
					}
					else
					{
						if (m_interactiveMode != InteractiveMode.Target || onTargetChanged == null)
						{
							return;
						}
						if (m_targetContext.TryGetTargetAt(value, out Target target))
						{
							if (target.type == Target.Type.Entity && target.entity == character)
							{
								m_targetContext.UpdateTarget(value, character);
							}
							else
							{
								m_targetContext.UpdateTarget(value, null);
							}
							onTargetChanged(target, referenceCell);
						}
						else
						{
							m_targetContext.UpdateTarget(value, null);
							onTargetChanged(null, null);
						}
					}
					return;
				}
			}
			if (m_pathFinder.tracking)
			{
				m_pathFinder.Reset();
				m_feedbackNeedsUpdate = true;
			}
			m_cellPointerManager.HideCursor();
			if (m_interactiveMode == InteractiveMode.Target)
			{
				onTargetChanged?.Invoke(null, null);
			}
		}

		private void UpdatePreviewMode()
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			FightMapInputHandler inputHandler = m_inputHandler;
			if (inputHandler.pressedMouseButton)
			{
				if (!inputHandler.mouseButtonPressLocation.HasValue)
				{
					return;
				}
				Vector2Int value = inputHandler.mouseButtonPressLocation.Value;
				if (!TryGetRegionIndex(value, out int regionIndex))
				{
					return;
				}
				FightMapMovementContext fightMapMovementContext = m_movementContexts[regionIndex];
				if (fightMapMovementContext.entityProvider.TryGetEntityAt(value, out ICharacterEntity character))
				{
					if (m_activeMovementContext != null)
					{
						m_activeMovementContext.End();
					}
					fightMapMovementContext.Begin(character, m_pathFinder);
					m_activeMovementContext = fightMapMovementContext;
					m_feedbackNeedsUpdate = true;
				}
			}
			else if (inputHandler.releasedMouseButton)
			{
				FightMapMovementContext activeMovementContext = m_activeMovementContext;
				if (activeMovementContext != null)
				{
					activeMovementContext.End();
					m_activeMovementContext = null;
					m_feedbackNeedsUpdate = true;
				}
			}
		}

		private void UpdateMovementMode()
		{
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			FightMapInputHandler inputHandler = m_inputHandler;
			if (inputHandler.pressedMouseButton)
			{
				if (m_pathFinder.tracking || !inputHandler.mouseButtonPressLocation.HasValue)
				{
					return;
				}
				Vector2Int value = inputHandler.mouseButtonPressLocation.Value;
				if (!TryGetRegionIndex(value, out int regionIndex))
				{
					return;
				}
				FightMapMovementContext fightMapMovementContext = m_movementContexts[regionIndex];
				IMapEntityProvider entityProvider = fightMapMovementContext.entityProvider;
				if (entityProvider.TryGetEntityAt(value, out ICharacterEntity character))
				{
					if (m_activeMovementContext != null)
					{
						m_activeMovementContext.End();
					}
					fightMapMovementContext.Begin(character, m_pathFinder);
					m_activeMovementContext = fightMapMovementContext;
					m_feedbackNeedsUpdate = true;
					if (entityProvider.IsCharacterPlayable(character))
					{
						m_pathFinder.Begin(value, character.movementPoints, fightMapMovementContext.canPassThrough);
						m_cellPointerManager.EndHighlightingPlayableCharacters();
					}
					m_cellPointerManager.ShowCursor();
					m_cellPointerManager.SetAnimatedCursor(value: false);
				}
			}
			else
			{
				if (!inputHandler.releasedMouseButton)
				{
					return;
				}
				FightMapMovementContext activeMovementContext = m_activeMovementContext;
				if (activeMovementContext != null)
				{
					if (m_pathFinder.tracking)
					{
						m_cellPointerManager.BeginHighlightingPlayableCharacters(this, activeMovementContext.entityProvider);
						Vector2Int[] path = m_pathFinder.currentPath.ToArray();
						ApplyMovement(path, activeMovementContext.trackedCharacter, activeMovementContext.targetedEntity);
						m_pathFinder.End();
						m_cellPointerManager.SetAnimatedCursor(value: false);
					}
					activeMovementContext.End();
					m_activeMovementContext = null;
					m_feedbackNeedsUpdate = true;
				}
			}
		}

		private void UpdateTargetMode()
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			if (!TargetInputEvent(out Vector2Int? coords))
			{
				return;
			}
			if (onTargetSelected == null)
			{
				Log.Warning("A target was selected but nothing was listening.", 757, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\AbstractFightMap.cs");
				return;
			}
			if (coords.HasValue)
			{
				Vector2Int value = coords.Value;
				if (m_targetContext.TryGetTargetAt(value, out Target target))
				{
					onTargetSelected(target);
					return;
				}
			}
			onTargetSelected(null);
		}

		private void UpdateFeedbacks()
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Unknown result type (might be due to invalid IL or missing references)
			//IL_0269: Unknown result type (might be due to invalid IL or missing references)
			//IL_026b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0270: Unknown result type (might be due to invalid IL or missing references)
			//IL_02af: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0311: Unknown result type (might be due to invalid IL or missing references)
			FightMapFeedbackResources feedbackResources = m_feedbackResources;
			if (null == feedbackResources)
			{
				return;
			}
			FightMapMovementContext[] movementContexts = m_movementContexts;
			int num = movementContexts.Length;
			for (int i = 0; i < num; i++)
			{
				FightMapMovementContext fightMapMovementContext = movementContexts[i];
				if (!fightMapMovementContext.hasEnded)
				{
					continue;
				}
				IMapDefinition mapDefinition = m_mapDefinition;
				IMapStateProvider stateProvider = fightMapMovementContext.stateProvider;
				Vector2Int sizeMin = stateProvider.sizeMin;
				Vector2Int sizeMax = stateProvider.sizeMax;
				for (int j = sizeMin.get_y(); j < sizeMax.get_y(); j++)
				{
					for (int k = sizeMin.get_x(); k < sizeMax.get_x(); k++)
					{
						int cellIndex = mapDefinition.GetCellIndex(k, j);
						CellObject cellObject = m_cellObjectsByIndex[cellIndex];
						if (!(null == cellObject))
						{
							cellObject.highlight.ClearSprite();
						}
					}
				}
			}
			FightMapTargetContext targetContext = m_targetContext;
			if (targetContext != null)
			{
				if (targetContext.isActive)
				{
					IMapDefinition mapDefinition2 = m_mapDefinition;
					IMapStateProvider stateProvider2 = targetContext.stateProvider;
					Vector2Int sizeMin2 = stateProvider2.sizeMin;
					Vector2Int sizeMax2 = stateProvider2.sizeMax;
					Color targetableAreaColor = feedbackResources.feedbackColors.targetableAreaColor;
					for (int l = sizeMin2.get_y(); l < sizeMax2.get_y(); l++)
					{
						for (int m = sizeMin2.get_x(); m < sizeMax2.get_x(); m++)
						{
							int cellIndex2 = mapDefinition2.GetCellIndex(m, l);
							CellObject cellObject2 = m_cellObjectsByIndex[cellIndex2];
							if (!(null == cellObject2))
							{
								FightMapFeedbackHelper.SetupSpellTargetHighlight(feedbackResources, targetContext, cellObject2.coords, cellObject2.highlight, targetableAreaColor);
							}
						}
					}
					m_pathFinderFeedbackManager.Clear();
					return;
				}
				if (targetContext.hasEnded)
				{
					IMapDefinition mapDefinition3 = m_mapDefinition;
					IMapStateProvider stateProvider3 = targetContext.stateProvider;
					Vector2Int sizeMin3 = stateProvider3.sizeMin;
					Vector2Int sizeMax3 = stateProvider3.sizeMax;
					for (int n = sizeMin3.get_y(); n < sizeMax3.get_y(); n++)
					{
						for (int num2 = sizeMin3.get_x(); num2 < sizeMax3.get_x(); num2++)
						{
							int cellIndex3 = mapDefinition3.GetCellIndex(num2, n);
							CellObject cellObject3 = m_cellObjectsByIndex[cellIndex3];
							if (!(null == cellObject3))
							{
								cellObject3.highlight.ClearSprite();
							}
						}
					}
				}
			}
			FightMapMovementContext activeMovementContext = m_activeMovementContext;
			if (activeMovementContext == null)
			{
				m_pathFinderFeedbackManager.Clear();
				return;
			}
			ICharacterEntity trackedCharacter = activeMovementContext.trackedCharacter;
			if (trackedCharacter != null)
			{
				Color highlightColor = GetHighlightColor(feedbackResources.feedbackColors, activeMovementContext.entityProvider, trackedCharacter);
				IMapDefinition mapDefinition4 = m_mapDefinition;
				IMapStateProvider stateProvider4 = activeMovementContext.stateProvider;
				Vector2Int sizeMin4 = stateProvider4.sizeMin;
				Vector2Int sizeMax4 = stateProvider4.sizeMax;
				for (int num3 = sizeMin4.get_y(); num3 < sizeMax4.get_y(); num3++)
				{
					for (int num4 = sizeMin4.get_x(); num4 < sizeMax4.get_x(); num4++)
					{
						int cellIndex4 = mapDefinition4.GetCellIndex(num4, num3);
						CellObject cellObject4 = m_cellObjectsByIndex[cellIndex4];
						if (!(null == cellObject4))
						{
							FightMapFeedbackHelper.SetupMovementAreaHighlight(feedbackResources, activeMovementContext, cellObject4.coords, cellObject4.highlight, highlightColor);
						}
					}
				}
			}
			if (m_pathFinder.tracking)
			{
				Vector2Int? target = activeMovementContext.targetedEntity?.area.refCoord;
				m_pathFinderFeedbackManager.Setup(feedbackResources.movementFeedbackResources, m_pathFinder.currentPath, target);
			}
			else
			{
				m_pathFinderFeedbackManager.Clear();
			}
		}

		private bool TargetInputEvent(out Vector2Int? coords)
		{
			FightMapInputHandler inputHandler = m_inputHandler;
			switch (m_targetInputMode)
			{
			case TargetInputMode.Click:
				coords = inputHandler.mouseButtonReleaseLocation;
				return inputHandler.clickedMouseButton;
			case TargetInputMode.Drag:
				coords = inputHandler.mouseButtonReleaseLocation;
				return inputHandler.releasedMouseButton;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		private bool TryGetRegionIndex(Vector2Int coords, out int regionIndex)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			FightMapMovementContext[] movementContexts = m_movementContexts;
			int num = movementContexts.Length;
			if (num == 1)
			{
				regionIndex = 0;
				return true;
			}
			for (int i = 0; i < num; i++)
			{
				if (movementContexts[i].Contains(coords))
				{
					regionIndex = i;
					return true;
				}
			}
			regionIndex = -1;
			return false;
		}

		protected virtual Color GetHighlightColor([NotNull] FightMapFeedbackColors feedbackColors, [NotNull] IMapEntityProvider mapEntityProvider, [NotNull] ICharacterEntity trackedCharacter)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return feedbackColors.targetableAreaColor;
		}

		public virtual CellObject GetCellObject(int x, int y)
		{
			int cellIndex = m_mapDefinition.GetCellIndex(x, y);
			return m_cellObjectsByIndex[cellIndex];
		}

		public unsafe virtual bool TryGetCellObject(int x, int y, out CellObject cellObject)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			IMapDefinition mapDefinition = m_mapDefinition;
			Vector2 val = Vector2Int.op_Implicit(mapDefinition.sizeMin);
			if ((float)x < ((IntPtr)(void*)val).x || (float)y < ((IntPtr)(void*)val).y)
			{
				cellObject = null;
				return false;
			}
			Vector2 val2 = Vector2Int.op_Implicit(mapDefinition.sizeMax);
			if ((float)x >= ((IntPtr)(void*)val2).x || (float)y >= ((IntPtr)(void*)val2).y)
			{
				cellObject = null;
				return false;
			}
			int cellIndex = mapDefinition.GetCellIndex(x, y);
			cellObject = m_cellObjectsByIndex[cellIndex];
			return null != cellObject;
		}

		public unsafe virtual Vector2Int GetCellCoords(Vector3 worldPosition)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			Vector3Int origin = m_mapDefinition.origin;
			Vector3Int val = new Vector3Int(Mathf.FloorToInt(((IntPtr)(void*)worldPosition).x), origin.get_y(), Mathf.FloorToInt(((IntPtr)(void*)worldPosition).z)) - origin;
			return new Vector2Int(val.get_x(), val.get_z());
		}

		public void AddArea(Area area)
		{
			m_virtualGrid.AddArea(area);
			m_virtualGridIsDirty = true;
		}

		public void MoveArea(Area from, Area to)
		{
			m_virtualGrid.MoveArea(from, to);
			m_virtualGridIsDirty = true;
		}

		public void RemoveArea(Area area)
		{
			m_virtualGrid.RemoveArea(area);
			m_virtualGridIsDirty = true;
		}

		public virtual MapCellIndicator GetCellIndicator(int x, int y)
		{
			return MapCellIndicator.None;
		}

		public void ApplyCellObjectAnimation([NotNull] CellObjectAnimationParameters parameters, Vector2Int origin, Quaternion rotation, float strength, float time)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			if (parameters.isValid)
			{
				CellObjectAnimationInstance item = new CellObjectAnimationInstance(parameters, origin, rotation, strength, time);
				m_cellObjectAnimationInstances.Add(item);
			}
		}

		protected AbstractFightMap()
			: this()
		{
		}
	}
}
