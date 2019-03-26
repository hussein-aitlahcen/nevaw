using Ankama.Cube.Data;
using Ankama.Cube.Fight.Entities;
using Ankama.Cube.Maps.Objects;
using Ankama.Cube.SRP;
using Ankama.Utilities;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Maps
{
	public class FightMapTargetContext
	{
		private enum CharacterFocusTweenState
		{
			None,
			In,
			Out
		}

		private const float TargetCharacterFocusFactor = 0.6f;

		private const float TweenInDelay = 0.4f;

		private const float TweenInDuration = 13f / 30f;

		private const float TweenOutDelay = 0.0833333358f;

		private const float TweenOutDuration = 0.333333343f;

		public readonly IMapStateProvider stateProvider;

		private readonly List<Target> m_data = new List<Target>();

		private readonly Dictionary<int, Target> m_targets = new Dictionary<int, Target>();

		private TweenerCore<float, float, FloatOptions> m_characterFocusTween;

		private CharacterFocusTweenState m_characterFocusTweenState;

		private float m_characterFocusFactor;

		public IEntityWithBoardPresence targetedEntity;

		private bool m_hasEnded;

		public bool isActive
		{
			get;
			private set;
		}

		public bool hasEnded
		{
			get
			{
				bool hasEnded = m_hasEnded;
				m_hasEnded = false;
				return hasEnded;
			}
		}

		public FightMapTargetContext([NotNull] IMapStateProvider stateProvider)
		{
			this.stateProvider = stateProvider;
		}

		public void Begin([NotNull] IEnumerable<Target> targetEnumerable)
		{
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			IMapStateProvider mapStateProvider = stateProvider;
			List<Target> data = m_data;
			Dictionary<int, Target> targets = m_targets;
			bool flag = false;
			foreach (Target item in targetEnumerable)
			{
				data.Add(item);
				switch (item.type)
				{
				case Target.Type.Coord:
				{
					Coord coord = item.coord;
					int cellIndex2 = mapStateProvider.GetCellIndex(coord.x, coord.y);
					targets.Add(cellIndex2, item);
					break;
				}
				case Target.Type.Entity:
				{
					IEntityWithBoardPresence entityWithBoardPresence;
					if ((entityWithBoardPresence = (item.entity as IEntityWithBoardPresence)) != null)
					{
						ICharacterObject characterObject;
						if ((characterObject = (entityWithBoardPresence.view as ICharacterObject)) != null)
						{
							characterObject.ShowSpellTargetFeedback(isSelected: false);
						}
						Vector2Int[] occupiedCoords = entityWithBoardPresence.area.occupiedCoords;
						int num = occupiedCoords.Length;
						for (int i = 0; i < num; i++)
						{
							Vector2Int val = occupiedCoords[i];
							int cellIndex = mapStateProvider.GetCellIndex(val.get_x(), val.get_y());
							targets.Add(cellIndex, item);
						}
						flag = true;
					}
					break;
				}
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
			if (flag)
			{
				StartCharacterFocus();
			}
			isActive = (targets.Count > 0);
		}

		public void UpdateTarget(Vector2Int targetCellCoords, IEntityWithBoardPresence targetEntity)
		{
			ICharacterObject characterObject;
			if (targetedEntity != null && (characterObject = (targetedEntity.view as ICharacterObject)) != null)
			{
				characterObject.ShowSpellTargetFeedback(isSelected: false);
			}
			ICharacterObject characterObject2;
			if (targetEntity != null && (characterObject2 = (targetEntity.view as ICharacterObject)) != null)
			{
				characterObject2.ShowSpellTargetFeedback(isSelected: true);
			}
			targetedEntity = targetEntity;
		}

		public bool End()
		{
			StopCharacterFocus();
			if (isActive)
			{
				Dictionary<int, Target> targets = m_targets;
				foreach (Target value in targets.Values)
				{
					ICharacterObject characterObject;
					IEntityWithBoardPresence entityWithBoardPresence;
					if (value.type == Target.Type.Entity && (entityWithBoardPresence = (value.entity as IEntityWithBoardPresence)) != null && (characterObject = (entityWithBoardPresence.view as ICharacterObject)) != null)
					{
						characterObject.HideSpellTargetFeedback();
					}
				}
				targets.Clear();
				m_data.Clear();
				m_hasEnded = true;
				targetedEntity = null;
				isActive = false;
				return true;
			}
			return false;
		}

		public void Refresh()
		{
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			if (!isActive)
			{
				return;
			}
			IMapStateProvider mapStateProvider = stateProvider;
			List<Target> data = m_data;
			Dictionary<int, Target> targets = m_targets;
			targets.Clear();
			int count = data.Count;
			for (int i = 0; i < count; i++)
			{
				Target target = data[i];
				switch (target.type)
				{
				case Target.Type.Coord:
				{
					Coord coord = target.coord;
					int cellIndex2 = mapStateProvider.GetCellIndex(coord.x, coord.y);
					targets.Add(cellIndex2, target);
					break;
				}
				case Target.Type.Entity:
				{
					IEntityWithBoardPresence entityWithBoardPresence;
					if ((entityWithBoardPresence = (target.entity as IEntityWithBoardPresence)) != null)
					{
						Vector2Int[] occupiedCoords = entityWithBoardPresence.area.occupiedCoords;
						int num = occupiedCoords.Length;
						for (int j = 0; j < num; j++)
						{
							Vector2Int val = occupiedCoords[j];
							int cellIndex = mapStateProvider.GetCellIndex(val.get_x(), val.get_y());
							targets.Add(cellIndex, target);
						}
					}
					else
					{
						Log.Error(string.Format("Entity target with id {0} does not implement {1}.", target.entity.id, "IEntityWithBoardPresence"), 228, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Maps\\FightMapTargetContext.cs");
					}
					break;
				}
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		public bool HasNonEntityTargetAt(Vector2Int coords)
		{
			int cellIndex = stateProvider.GetCellIndex(coords.get_x(), coords.get_y());
			if (m_targets.TryGetValue(cellIndex, out Target value))
			{
				return value.type != Target.Type.Entity;
			}
			return false;
		}

		public bool TryGetTargetAt(Vector2Int coords, out Target target)
		{
			int cellIndex = stateProvider.GetCellIndex(coords.get_x(), coords.get_y());
			return m_targets.TryGetValue(cellIndex, out target);
		}

		private unsafe void StartCharacterFocus()
		{
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Expected O, but got Unknown
			switch (m_characterFocusTweenState)
			{
			case CharacterFocusTweenState.In:
				return;
			case CharacterFocusTweenState.Out:
				TweenExtensions.Kill(m_characterFocusTween, false);
				m_characterFocusTween = null;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case CharacterFocusTweenState.None:
				break;
			}
			float num = 13f / 30f * (0.6f - m_characterFocusFactor) / 0.6f;
			if (Mathf.Approximately(0f, num))
			{
				m_characterFocusTweenState = CharacterFocusTweenState.None;
				return;
			}
			m_characterFocusTween = TweenSettingsExtensions.OnComplete<TweenerCore<float, float, FloatOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(new DOGetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), 0.6f, num), 7), new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			if (m_characterFocusTweenState == CharacterFocusTweenState.None)
			{
				TweenSettingsExtensions.SetDelay<TweenerCore<float, float, FloatOptions>>(m_characterFocusTween, 0.4f);
			}
			m_characterFocusTweenState = CharacterFocusTweenState.In;
		}

		private unsafe void StopCharacterFocus()
		{
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Expected O, but got Unknown
			switch (m_characterFocusTweenState)
			{
			case CharacterFocusTweenState.Out:
				return;
			case CharacterFocusTweenState.In:
				TweenExtensions.Kill(m_characterFocusTween, false);
				m_characterFocusTween = null;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case CharacterFocusTweenState.None:
				break;
			}
			float num = 0.333333343f * m_characterFocusFactor / 0.6f;
			if (Mathf.Approximately(0f, num))
			{
				m_characterFocusTweenState = CharacterFocusTweenState.None;
				return;
			}
			m_characterFocusTween = TweenSettingsExtensions.OnComplete<TweenerCore<float, float, FloatOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(new DOGetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), 0f, num), 7), new TweenCallback((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
			if (m_characterFocusTweenState == CharacterFocusTweenState.None)
			{
				TweenSettingsExtensions.SetDelay<TweenerCore<float, float, FloatOptions>>(m_characterFocusTween, 0.0833333358f);
			}
			m_characterFocusTweenState = CharacterFocusTweenState.Out;
		}

		private float CharacterFocusTweenGetter()
		{
			return m_characterFocusFactor;
		}

		private void CharacterFocusTweenSetter(float value)
		{
			CubeSRP.set_characterFocusFactor(value);
			m_characterFocusFactor = value;
		}

		private void OnCharacterFocusTweenComplete()
		{
			m_characterFocusTweenState = CharacterFocusTweenState.None;
			m_characterFocusTween = null;
		}
	}
}
