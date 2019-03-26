using Ankama.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	public abstract class ScriptableEffect : ScriptableObject
	{
		protected enum InitializationState
		{
			None,
			Loading,
			Loaded,
			Failed
		}

		[NonSerialized]
		protected InitializationState m_initializationState;

		[NonSerialized]
		private int m_referenceCounter;

		public IEnumerator Load()
		{
			m_referenceCounter++;
			switch (m_initializationState)
			{
			case InitializationState.Loaded:
			case InitializationState.Failed:
				break;
			case InitializationState.Loading:
				do
				{
					yield return null;
				}
				while (m_initializationState == InitializationState.Loading);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case InitializationState.None:
				m_initializationState = InitializationState.Loading;
				yield return LoadInternal();
				break;
			}
		}

		public void Unload()
		{
			m_referenceCounter--;
			if (m_referenceCounter <= 0)
			{
				switch (m_initializationState)
				{
				case InitializationState.None:
					break;
				case InitializationState.Failed:
					break;
				default:
					throw new ArgumentOutOfRangeException();
				case InitializationState.Loading:
				case InitializationState.Loaded:
					UnloadInternal();
					m_initializationState = InitializationState.None;
					break;
				}
			}
		}

		protected abstract IEnumerator LoadInternal();

		protected abstract void UnloadInternal();

		public static IEnumerator LoadAll<T>(ICollection<T> effects) where T : ScriptableEffect
		{
			int count = effects.Count;
			switch (count)
			{
			case 0:
				yield break;
			case 1:
				foreach (T effect in effects)
				{
					yield return effect.Load();
				}
				yield break;
			}
			IEnumerator[] array = new IEnumerator[count];
			int num = 0;
			foreach (T effect2 in effects)
			{
				array[num] = effect2.Load();
				num++;
			}
			yield return EnumeratorUtility.ParallelRecursiveImmediateSafeExecution(array);
		}

		protected ScriptableEffect()
			: this()
		{
		}
	}
}
