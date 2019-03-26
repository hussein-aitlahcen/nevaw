using Ankama.Animations;
using Ankama.Animations.Events;
using Ankama.Cube.Animations;
using Ankama.Cube.Data;
using Ankama.Cube.Maps.Objects;
using Ankama.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Ankama.Cube.Maps
{
	public class MapPathfindingActor : MonoBehaviour
	{
		[SerializeField]
		private PlayableDirector m_playableDirector;

		[SerializeField]
		private Animator2D m_animator2D;

		[SerializeField]
		private float m_speedFactor = 1f;

		[SerializeField]
		private float m_heightOffest = 0.07f;

		private Coroutine m_movementCoroutine;

		private const float MovementCellTraversalTime = 5f;

		private bool m_hasTimeline;

		private DirectionAngle m_mapRotation;

		private Direction m_direction = Direction.SouthEast;

		private CharacterAnimationParameters m_animationParameters;

		private CharacterAnimationCallback m_animationCallback;

		private AnimatedFightCharacterData m_characterData;

		private MapCharacterObjectContext m_context;

		private bool m_isRunning;

		public Direction direction
		{
			get
			{
				return m_direction;
			}
			set
			{
				if (value != m_direction)
				{
					if (m_context != null)
					{
						m_context.UpdateDirection(m_direction, value);
					}
					m_direction = value;
				}
			}
		}

		public unsafe Vector2 coords => new Vector2(((IntPtr)(void*)this.get_transform().get_position()).x, ((IntPtr)(void*)this.get_transform().get_position()).z);

		private void Awake()
		{
		}

		private IEnumerator Start()
		{
			yield break;
		}

		protected unsafe void OnEnable()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			m_animator2D.add_AnimationLooped(new AnimationLoopedEventHandler((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_animationCallback = new CharacterAnimationCallback(m_animator2D);
			m_context = new MapCharacterObjectContext(this);
			m_context.Initialize();
			m_playableDirector.set_extrapolationMode(0);
		}

		protected unsafe void OnDisable()
		{
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Expected O, but got Unknown
			if (m_movementCoroutine != null)
			{
				this.StopCoroutine(m_movementCoroutine);
				m_movementCoroutine = null;
			}
			if (m_animationCallback != null)
			{
				m_animationCallback.Release();
				m_animationCallback = null;
			}
			if (m_context != null)
			{
				m_context.Release();
				m_context = null;
			}
			m_animator2D.remove_AnimationLooped(new AnimationLoopedEventHandler((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		}

		public unsafe void SetCharacterData(AnimatedFightCharacterData data, AnimatedObjectDefinition def)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected O, but got Unknown
			m_characterData = data;
			m_animator2D.add_Initialised(new Animator2DInitialisedEventHandler((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			m_animator2D.SetDefinition(def, null, (Graphic[])null);
		}

		private unsafe void InitAnimatorCallback(object sender, Animator2DInitialisedEventArgs e)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			m_animator2D.remove_Initialised(new Animator2DInitialisedEventHandler((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			PlayIdleAnimation();
		}

		public void FollowPath(List<Vector3> path)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			FollowPath(path, Vector3.get_zero());
		}

		public void FollowPath(List<Vector3> path, Vector3 endLookAt)
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			if (!(m_animator2D.GetDefinition() == null))
			{
				if (m_movementCoroutine != null)
				{
					this.StopCoroutine(m_movementCoroutine);
				}
				if (path == null)
				{
					PlayIdleAnimation();
				}
				else
				{
					m_movementCoroutine = this.StartCoroutine(MoveToRoutine(path, endLookAt));
				}
			}
		}

		private Direction GetDirection(Vector3 start, Vector3 end)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			Vector3 direction = end - start;
			return GetDirection(direction);
		}

		private unsafe Direction GetDirection(Vector3 direction)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			if (Mathf.Abs(((IntPtr)(void*)direction).x) > Mathf.Abs(((IntPtr)(void*)direction).z))
			{
				if (((IntPtr)(void*)direction).x < 0f)
				{
					return Direction.SouthWest;
				}
				return Direction.NorthEast;
			}
			if (((IntPtr)(void*)direction).z < 0f)
			{
				return Direction.SouthEast;
			}
			return Direction.NorthWest;
		}

		private IEnumerator MoveToRoutine(List<Vector3> path, Vector3 endLookAt)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			int movementCellsCount = path.Count;
			if (movementCellsCount <= 1)
			{
				yield break;
			}
			Animator2D animator = m_animator2D;
			AnimatedFightCharacterData.IdleToRunTransitionMode idleToRunTransitionMode = m_characterData.idleToRunTransitionMode;
			Vector3 startCell = path[0];
			if (!m_isRunning && idleToRunTransitionMode.HasFlag(AnimatedFightCharacterData.IdleToRunTransitionMode.IdleToRun))
			{
				Direction direction = (movementCellsCount >= 2) ? GetDirection(startCell, path[1]) : this.direction;
				CharacterAnimationInfo transitionAnimationInfo2 = new CharacterAnimationInfo(new Vector2(startCell.x, startCell.z), "idle_run", "idle-to-run", loops: false, direction, m_mapRotation);
				if (!transitionAnimationInfo2.animationName.Equals(m_animator2D.get_animationName()))
				{
					StartAnimation(transitionAnimationInfo2);
				}
				while (!CharacterObjectUtility.HasAnimationEnded(animator, transitionAnimationInfo2))
				{
					yield return null;
				}
			}
			m_isRunning = true;
			Vector3 previousCoords = startCell;
			int num;
			for (int i = 1; i < movementCellsCount; i = num)
			{
				Vector3 coords = path[i];
				Vector3 direction2 = coords - previousCoords;
				float magnitude = direction2.get_magnitude();
				Direction direction3 = GetDirection(direction2);
				CharacterAnimationInfo animationInfo = new CharacterAnimationInfo(new Vector2(coords.x, coords.z), "run", "run", loops: true, direction3, m_mapRotation);
				StartAnimation(animationInfo, null, null, restart: false);
				float cellTraversalDuration = magnitude * 5f * (1f / m_speedFactor) / (float)animator.get_frameRate();
				float animationTime = 0f;
				do
				{
					Vector3 val = Vector3.Lerp(previousCoords, coords, animationTime / cellTraversalDuration);
					this.get_transform().set_position(val + Vector3.get_up() * m_heightOffest);
					yield return null;
					animationTime += Time.get_deltaTime();
				}
				while (animationTime < cellTraversalDuration);
				this.get_transform().set_position(coords + Vector3.get_up() * m_heightOffest);
				previousCoords = coords;
				num = i + 1;
			}
			m_isRunning = false;
			if (idleToRunTransitionMode.HasFlag(AnimatedFightCharacterData.IdleToRunTransitionMode.RunToIdle))
			{
				CharacterAnimationInfo transitionAnimationInfo2 = new CharacterAnimationInfo(new Vector2(previousCoords.x, previousCoords.z), "run_idle", "run-to-idle", loops: false, this.direction, m_mapRotation);
				StartAnimation(transitionAnimationInfo2);
				while (!CharacterObjectUtility.HasAnimationEnded(animator, transitionAnimationInfo2))
				{
					yield return null;
				}
			}
			if (endLookAt != Vector3.get_zero())
			{
				m_direction = GetDirection(endLookAt);
			}
			PlayIdleAnimation();
			m_movementCoroutine = null;
		}

		private void PlayIdleAnimation()
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			CharacterAnimationInfo animationInfo = new CharacterAnimationInfo(coords, "idle", "idle", loops: true, m_direction, m_mapRotation);
			StartAnimation(animationInfo, null, null, restart: false);
		}

		private void StartAnimation(CharacterAnimationInfo animationInfo, Action onComplete = null, Action onCancel = null, bool restart = true, bool async = false)
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			string animationName = animationInfo.animationName;
			string timelineKey = animationInfo.timelineKey;
			m_animator2D.get_transform().set_localRotation(animationInfo.flipX ? Quaternion.Euler(0f, -135f, 0f) : Quaternion.Euler(0f, 45f, 0f));
			direction = animationInfo.direction;
			ITimelineAssetProvider characterData = m_characterData;
			if (characterData != null)
			{
				TimelineAsset timelineAsset;
				bool flag = characterData.TryGetTimelineAsset(timelineKey, out timelineAsset);
				if (flag && null != timelineAsset)
				{
					if (timelineAsset != m_playableDirector.get_playableAsset())
					{
						m_playableDirector.Play(timelineAsset);
					}
					else
					{
						if (restart || !m_animator2D.get_animationName().Equals(animationName))
						{
							m_playableDirector.set_time(0.0);
						}
						m_playableDirector.Resume();
					}
					m_hasTimeline = true;
				}
				else
				{
					if (flag)
					{
						Log.Warning("Character named '" + m_characterData.get_name() + "' has a timeline setup for key '" + timelineKey + "' but the actual asset is null.", 323, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\HavreMap\\MapPathfindingActor.cs");
					}
					m_playableDirector.set_time(0.0);
					m_playableDirector.Pause();
					m_hasTimeline = false;
				}
			}
			m_animationCallback.Setup(animationName, restart, onComplete, onCancel);
			m_animator2D.SetAnimation(animationName, animationInfo.loops, async, restart);
			m_animationParameters = animationInfo.parameters;
		}

		private void OnAnimationLooped(object sender, AnimationLoopedEventArgs e)
		{
			if (m_hasTimeline)
			{
				m_playableDirector.set_time(0.0);
				m_playableDirector.Resume();
			}
		}

		public MapPathfindingActor()
			: this()
		{
		}
	}
}
