using Ankama.Cube.Audio;
using Ankama.Cube.Data;
using Ankama.Cube.Data.Maps;
using Ankama.Cube.Fight;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps.Objects;
using Ankama.Cube.Maps.VisualEffects;
using Ankama.Cube.States;
using Ankama.Cube.UI.Fight;
using Ankama.Utilities;
using FMODUnity;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Maps
{
	[UsedImplicitly]
	public sealed class FightMap : AbstractFightMap
	{
		private Dictionary<int, GameObject> m_monsterSpawnCellDictionary;

		public static FightMap current;

		[SerializeField]
		[HideInInspector]
		private FightMapDefinition m_definition;

		[SerializeField]
		private MapRenderSettings m_renderSettings;

		[SerializeField]
		private BossFightMapResources m_bossFightMapResources;

		[SerializeField]
		private BossObject m_bossObject;

		[SerializeField]
		private AudioEventGroup m_musicGroup;

		[SerializeField]
		private AudioEventGroup m_ambianceGroup;

		private readonly FightMapAudioContext m_audioContext = new FightMapAudioContext();

		private AudioWorldMusicRequest m_worldMusicRequest;

		public BossObject bossObject => m_bossObject;

		public FightMapDefinition definition => m_definition;

		public MapRenderSettings ambience => m_renderSettings;

		public unsafe IEnumerator AddMonsterSpawnCell(int x, int y, Direction direction)
		{
			MonsterSpawnCellDefinition monsterSpawnCellDefinition = m_bossFightMapResources.monsterSpawnCellDefinition;
			if (null == monsterSpawnCellDefinition)
			{
				yield break;
			}
			IMapDefinition mapDefinition = m_mapDefinition;
			Vector2 val = Vector2Int.op_Implicit(mapDefinition.sizeMin);
			if ((float)x < ((IntPtr)(void*)val).x || (float)y < ((IntPtr)(void*)val).y)
			{
				yield break;
			}
			Vector2 val2 = Vector2Int.op_Implicit(mapDefinition.sizeMax);
			if ((float)x >= ((IntPtr)(void*)val2).x || (float)y >= ((IntPtr)(void*)val2).y)
			{
				yield break;
			}
			int index = mapDefinition.GetCellIndex(x, y);
			CellObject cellObject = m_cellObjectsByIndex[index];
			Transform transform = cellObject.get_transform();
			Vector3 position = transform.get_position() + 0.5f * Vector3.get_up();
			Quaternion rotation = Quaternion.get_identity();
			if (AudioManager.isReady)
			{
				AudioReference appearanceSound = monsterSpawnCellDefinition.appearanceSound;
				if (appearanceSound.get_isValid())
				{
					AudioManager.PlayOneShot(appearanceSound, transform);
				}
			}
			VisualEffect appearanceEffect = monsterSpawnCellDefinition.appearanceEffect;
			if (null != appearanceEffect)
			{
				Object.Instantiate<VisualEffect>(appearanceEffect, position, rotation, transform);
				float appearanceDelay = monsterSpawnCellDefinition.appearanceDelay;
				if (appearanceDelay > 0f)
				{
					yield return (object)new WaitForTime(appearanceDelay);
				}
			}
			GameObject val3 = monsterSpawnCellDefinition.Instantiate(position, rotation, cellObject.get_transform());
			if (!(null == val3))
			{
				val3.GetComponent<SpawnCellObject>().SetDirection(direction);
				m_monsterSpawnCellDictionary.Add(index, val3);
			}
		}

		public IEnumerator RemoveMonsterSpawnCell(int x, int y)
		{
			MonsterSpawnCellDefinition monsterSpawnCellDefinition = m_bossFightMapResources.monsterSpawnCellDefinition;
			if (null == monsterSpawnCellDefinition)
			{
				yield break;
			}
			int cellIndex = m_mapDefinition.GetCellIndex(x, y);
			if (!m_monsterSpawnCellDictionary.TryGetValue(cellIndex, out GameObject instance))
			{
				yield break;
			}
			m_monsterSpawnCellDictionary.Remove(cellIndex);
			Transform transform = instance.get_transform();
			if (AudioManager.isReady)
			{
				AudioReference disappearanceSound = monsterSpawnCellDefinition.disappearanceSound;
				if (disappearanceSound.get_isValid())
				{
					AudioManager.PlayOneShot(disappearanceSound, transform);
				}
			}
			VisualEffect disappearanceEffect = monsterSpawnCellDefinition.disappearanceEffect;
			if (null != disappearanceEffect)
			{
				Object.Instantiate<VisualEffect>(disappearanceEffect, transform.get_position(), transform.get_rotation(), transform.get_parent());
				float disappearanceDelay = monsterSpawnCellDefinition.disappearanceDelay;
				if (disappearanceDelay > 0f)
				{
					yield return (object)new WaitForTime(disappearanceDelay);
				}
			}
			monsterSpawnCellDefinition.DestroyInstance(instance);
		}

		public IEnumerator ClearMonsterSpawnCells(int fightId)
		{
			MonsterSpawnCellDefinition monsterSpawnCellDefinition = m_bossFightMapResources.monsterSpawnCellDefinition;
			if (null == monsterSpawnCellDefinition)
			{
				yield break;
			}
			FightMapDefinition definition = m_definition;
			FightMapRegionDefinition obj = definition.regions[fightId];
			Vector2Int sizeMin = obj.sizeMin;
			Vector2Int sizeMax = obj.sizeMax;
			List<int> indicesToRemove = ListPool<int>.Get();
			foreach (KeyValuePair<int, GameObject> item in m_monsterSpawnCellDictionary)
			{
				int key = item.Key;
				Vector2Int cellCoords = definition.GetCellCoords(key);
				if (cellCoords.get_x() >= sizeMin.get_x() && cellCoords.get_y() >= sizeMin.get_y() && cellCoords.get_x() < sizeMax.get_x() && cellCoords.get_y() < sizeMax.get_y())
				{
					Transform transform = item.Value.get_transform();
					if (AudioManager.isReady)
					{
						AudioReference disappearanceSound = monsterSpawnCellDefinition.disappearanceSound;
						if (disappearanceSound.get_isValid())
						{
							AudioManager.PlayOneShot(disappearanceSound, transform);
						}
					}
					VisualEffect disappearanceEffect = monsterSpawnCellDefinition.disappearanceEffect;
					if (null != disappearanceEffect)
					{
						Object.Instantiate<VisualEffect>(disappearanceEffect, transform.get_position(), transform.get_rotation(), transform.get_parent());
					}
					indicesToRemove.Add(key);
				}
			}
			int indicesToRemoveCount = indicesToRemove.Count;
			if (indicesToRemoveCount > 0)
			{
				float disappearanceDelay = monsterSpawnCellDefinition.disappearanceDelay;
				if (disappearanceDelay > 0f)
				{
					yield return (object)new WaitForTime(disappearanceDelay);
				}
				for (int i = 0; i < indicesToRemoveCount; i++)
				{
					int key2 = indicesToRemove[i];
					if (m_monsterSpawnCellDictionary.TryGetValue(key2, out GameObject value))
					{
						monsterSpawnCellDefinition.DestroyInstance(value);
					}
				}
			}
			ListPool<int>.Release(indicesToRemove);
		}

		public void AddHeroLostFeedback(Vector2Int position)
		{
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			GameObject[] heroLostFeedbacks = m_bossFightMapResources.heroLostFeedbacks;
			int num = heroLostFeedbacks.Length;
			if (num != 0)
			{
				int num2 = Random.Range(0, num);
				GameObject val = heroLostFeedbacks[num2];
				if (null == val)
				{
					Log.Error($"HeroLostFeedback at index {num2} is null for map named {m_definition.get_displayName()}.", 213, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\FightMap.BossFight.cs");
					return;
				}
				if (!TryGetCellObject(position.get_x(), position.get_y(), out CellObject cellObject))
				{
					Log.Error($"Tried to add an HeroLostFeedback instance at position {position} but there is no cell there.", 220, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\FightMap.BossFight.cs");
					return;
				}
				Transform transform = cellObject.get_transform();
				Vector3 val2 = transform.get_position() + 0.5f * Vector3.get_up();
				Object.Instantiate<GameObject>(val, val2, Quaternion.get_identity(), transform);
			}
		}

		protected override void ApplyMovement(Vector2Int[] path, ICharacterEntity trackedCharacter, IEntityWithBoardPresence targetedEntity)
		{
			FightState instance = FightState.instance;
			if (instance != null)
			{
				if (targetedEntity != null)
				{
					instance.frame.SendEntityAttack(trackedCharacter.id, path, targetedEntity.id);
				}
				else if (path.Length > 1)
				{
					instance.frame.SendEntityMovement(trackedCharacter.id, path);
				}
			}
		}

		protected override Color GetHighlightColor(FightMapFeedbackColors feedbackColors, IMapEntityProvider mapEntityProvider, ICharacterEntity trackedCharacter)
		{
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			FightStatus local = FightStatus.local;
			PlayerType playerType = (local != mapEntityProvider) ? ((trackedCharacter.teamIndex == GameStatus.localPlayerTeamIndex) ? PlayerType.Ally : PlayerType.Opponent) : ((local.localPlayerId != trackedCharacter.ownerId) ? ((trackedCharacter.teamIndex == GameStatus.localPlayerTeamIndex) ? (PlayerType.Ally | PlayerType.Local) : (PlayerType.Opponent | PlayerType.Local)) : PlayerType.Player);
			return feedbackColors.GetPlayerColor(playerType);
		}

		public override MapCellIndicator GetCellIndicator(int x, int y)
		{
			Dictionary<int, GameObject> monsterSpawnCellDictionary = m_monsterSpawnCellDictionary;
			if (monsterSpawnCellDictionary != null)
			{
				int cellIndex = m_mapDefinition.GetCellIndex(x, y);
				if (monsterSpawnCellDictionary.ContainsKey(cellIndex))
				{
					return MapCellIndicator.Death;
				}
			}
			return MapCellIndicator.None;
		}

		private void Awake()
		{
			m_mapDefinition = m_definition;
			m_interactiveMode = InteractiveMode.None;
			Create();
			this.set_enabled(false);
		}

		protected override void Update()
		{
			base.Update();
			FightUIRework.tooltipsEnabled = (!m_pathFinder.tracking && FightCastManager.currentCastType == FightCastManager.CurrentCastType.None);
		}

		public IEnumerator Initialize()
		{
			CameraHandler.AddMapRotationListener(OnMapRotationChanged);
			FightStatus local = FightStatus.local;
			int regionCount = m_mapDefinition.regionCount;
			m_movementContexts = new FightMapMovementContext[regionCount];
			for (int i = 0; i < regionCount; i++)
			{
				FightStatus fightStatus = FightLogicExecutor.GetFightStatus(i);
				fightStatus.EntitiesChanged += OnEntitiesChanged;
				FightMapMovementContext fightMapMovementContext = new FightMapMovementContext(fightStatus.mapStatus, fightStatus);
				if (fightStatus == local)
				{
					m_localMovementContext = fightMapMovementContext;
				}
				m_movementContexts[i] = fightMapMovementContext;
			}
			if (m_localMovementContext != null)
			{
				IMapStateProvider stateProvider = m_localMovementContext.stateProvider;
				m_targetContext = new FightMapTargetContext(stateProvider);
			}
			BoxCollider mapCollider = CreateCollider();
			InitializeHandlers(mapCollider, giveUserControl: false);
			MonsterSpawnCellDefinition monsterSpawnCellDefinition = m_bossFightMapResources.monsterSpawnCellDefinition;
			if (null != monsterSpawnCellDefinition)
			{
				yield return monsterSpawnCellDefinition.Initialize();
				m_monsterSpawnCellDictionary = new Dictionary<int, GameObject>();
			}
			if (AudioManager.isReady)
			{
				m_audioContext.Initialize();
				m_worldMusicRequest = AudioManager.LoadWorldMusic(m_musicGroup, m_ambianceGroup, m_audioContext);
				while (m_worldMusicRequest.state == AudioWorldMusicRequest.State.Loading)
				{
					yield return null;
				}
			}
		}

		public void Begin()
		{
			if (m_worldMusicRequest != null)
			{
				AudioManager.StartWorldMusic(m_worldMusicRequest);
			}
			this.set_enabled(true);
		}

		public void Stop()
		{
			SetNoInteractionPhase();
			if (m_activeMovementContext != null)
			{
				m_activeMovementContext.End();
				m_activeMovementContext = null;
				m_feedbackNeedsUpdate = true;
			}
		}

		public void End()
		{
			if (m_worldMusicRequest != null)
			{
				AudioManager.StopWorldMusic(m_worldMusicRequest);
			}
		}

		public void Release()
		{
			CameraHandler.RemoveMapRotationListener(OnMapRotationChanged);
			if (FightLogicExecutor.isValid)
			{
				int regionCount = m_mapDefinition.regionCount;
				for (int i = 0; i < regionCount; i++)
				{
					FightStatus fightStatus = FightLogicExecutor.GetFightStatus(i);
					if (fightStatus != null)
					{
						fightStatus.EntitiesChanged -= OnEntitiesChanged;
					}
				}
			}
			MonsterSpawnCellDefinition monsterSpawnCellDefinition = m_bossFightMapResources.monsterSpawnCellDefinition;
			if (null != monsterSpawnCellDefinition)
			{
				foreach (GameObject value in m_monsterSpawnCellDictionary.Values)
				{
					monsterSpawnCellDefinition.DestroyInstance(value);
				}
				m_monsterSpawnCellDictionary.Clear();
				m_monsterSpawnCellDictionary = null;
				monsterSpawnCellDefinition.Release();
			}
			if (AudioManager.isReady)
			{
				m_audioContext.Release();
			}
			m_worldMusicRequest = null;
			m_movementContexts = null;
			m_localMovementContext = null;
			m_activeMovementContext = null;
			m_targetContext = null;
		}

		public void SetTurnIndex(int turnIndex)
		{
			if (m_audioContext != null)
			{
				m_audioContext.turnIndex = turnIndex;
			}
		}

		public void SetLocalPlayerHeroLife(int life, int baseLife)
		{
			if (m_audioContext != null)
			{
				m_audioContext.localPlayerHeroLife = Mathf.Clamp01((float)life / (float)baseLife);
			}
		}

		public void SetBossEvolutionStep(int value)
		{
			if (m_audioContext != null)
			{
				m_audioContext.bossEvolutionStep = value;
			}
		}

		private void OnMapRotationChanged(DirectionAngle previousMapRotation, DirectionAngle newMapRotation)
		{
			m_pathFinderFeedbackManager.SetMapRotation(newMapRotation);
			m_cellPointerManager.SetMapRotation(newMapRotation);
		}

		private void OnEntitiesChanged(FightStatus fightStatus, EntitiesChangedFlags flags)
		{
			int fightId = fightStatus.fightId;
			FightMapMovementContext fightMapMovementContext = m_movementContexts[fightId];
			if ((flags & (EntitiesChangedFlags.Added | EntitiesChangedFlags.Removed | EntitiesChangedFlags.AreaMoved)) != 0 && m_activeMovementContext == fightMapMovementContext)
			{
				ICharacterEntity trackedCharacter = fightMapMovementContext.trackedCharacter;
				if (trackedCharacter != null)
				{
					fightMapMovementContext.End();
					if (!trackedCharacter.isDirty)
					{
						fightMapMovementContext.Begin(trackedCharacter, m_pathFinder);
					}
					else
					{
						m_activeMovementContext = null;
					}
					m_feedbackNeedsUpdate = true;
				}
			}
			if (m_targetContext != null && (flags & (EntitiesChangedFlags.Removed | EntitiesChangedFlags.AreaMoved)) != 0)
			{
				m_targetContext.Refresh();
				m_feedbackNeedsUpdate = true;
			}
			if (m_interactiveMode == InteractiveMode.Movement && m_localMovementContext == fightMapMovementContext)
			{
				m_cellPointerManager.RefreshPlayableCharactersHighlights(this, fightStatus);
			}
			m_inputHandler.SetDirty();
		}
	}
}
