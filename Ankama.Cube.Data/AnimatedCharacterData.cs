using Ankama.Cube.Animations;
using Ankama.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

namespace Ankama.Cube.Data
{
	public abstract class AnimatedCharacterData : ScriptableObject, ITimelineAssetProvider
	{
		private enum TimelineResourceLoadingState
		{
			None,
			Loading,
			Loaded,
			Failed
		}

		[SerializeField]
		protected int m_areaSize = 1;

		[HideInInspector]
		[SerializeField]
		protected TimelineAssetDictionary m_timelineAssetDictionary;

		[SerializeField]
		protected CharacterEffect m_spawnEffect;

		[SerializeField]
		protected CharacterEffect m_deathEffect;

		[NonSerialized]
		private int m_referenceCounter;

		[NonSerialized]
		private TimelineResourceLoadingState m_timelineResourcesLoadState;

		public int areaSize => m_areaSize;

		public CharacterEffect spawnEffect => m_spawnEffect;

		public CharacterEffect deathEffect => m_deathEffect;

		public IEnumerator LoadTimelineResources()
		{
			m_referenceCounter++;
			switch (m_timelineResourcesLoadState)
			{
			case TimelineResourceLoadingState.Loaded:
			case TimelineResourceLoadingState.Failed:
				break;
			case TimelineResourceLoadingState.Loading:
				while (m_timelineResourcesLoadState == TimelineResourceLoadingState.Loading)
				{
					yield return null;
				}
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case TimelineResourceLoadingState.None:
			{
				m_timelineResourcesLoadState = TimelineResourceLoadingState.Loading;
				List<IEnumerator> loadRoutines = ListPool<IEnumerator>.Get(4);
				try
				{
					m_timelineAssetDictionary.GatherLoadRoutines(loadRoutines);
					if (null != m_spawnEffect)
					{
						loadRoutines.Add(m_spawnEffect.Load());
					}
					if (null != m_deathEffect)
					{
						loadRoutines.Add(m_deathEffect.Load());
					}
					GatherAdditionalResourcesLoadingRoutines(loadRoutines);
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
					m_timelineResourcesLoadState = TimelineResourceLoadingState.Failed;
				}
				yield return EnumeratorUtility.ParallelRecursiveImmediateSafeExecution(loadRoutines.ToArray());
				ListPool<IEnumerator>.Release(loadRoutines);
				m_timelineResourcesLoadState = TimelineResourceLoadingState.Loaded;
				break;
			}
			}
		}

		public void UnloadTimelineResources()
		{
			m_referenceCounter--;
			if (m_referenceCounter > 0)
			{
				return;
			}
			switch (m_timelineResourcesLoadState)
			{
			case TimelineResourceLoadingState.None:
				break;
			case TimelineResourceLoadingState.Failed:
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case TimelineResourceLoadingState.Loading:
			case TimelineResourceLoadingState.Loaded:
				m_timelineAssetDictionary.Unload();
				if (null != m_spawnEffect)
				{
					m_spawnEffect.Unload();
				}
				if (null != m_deathEffect)
				{
					m_deathEffect.Unload();
				}
				UnloadAdditionalResources();
				m_timelineResourcesLoadState = TimelineResourceLoadingState.None;
				break;
			}
		}

		public bool HasTimelineAsset(string currentTimelineKey)
		{
			return ((Dictionary<string, TimelineAsset>)m_timelineAssetDictionary).ContainsKey(currentTimelineKey);
		}

		public bool TryGetTimelineAsset(string key, out TimelineAsset timelineAsset)
		{
			return ((Dictionary<string, TimelineAsset>)m_timelineAssetDictionary).TryGetValue(key, out timelineAsset);
		}

		protected abstract void GatherAdditionalResourcesLoadingRoutines(List<IEnumerator> routines);

		protected abstract void UnloadAdditionalResources();

		protected AnimatedCharacterData()
			: this()
		{
		}
	}
}
