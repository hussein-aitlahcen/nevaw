using Ankama.AssetManagement;
using Ankama.Utilities;
using FMODUnity;
using System.Collections;
using UnityEngine;

namespace Ankama.Cube.Audio.UI
{
	public abstract class AudioEventUILoader : MonoBehaviour
	{
		protected enum InitializationState
		{
			None,
			Loading,
			Loaded,
			Error
		}

		protected InitializationState m_initializationState;

		private AudioBankLoadRequest[] m_bankLoadRequests;

		protected IEnumerator Load(AudioReferenceWithParameters audioReference)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if (!AudioManager.TryGetDefaultBankName(AudioReferenceWithParameters.op_Implicit(audioReference), out string bankName))
			{
				m_initializationState = InitializationState.Error;
				yield break;
			}
			AudioBankLoadRequest bankLoadRequest = AudioManager.LoadBankAsync(bankName);
			m_bankLoadRequests = new AudioBankLoadRequest[1]
			{
				bankLoadRequest
			};
			m_initializationState = InitializationState.Loading;
			while (!bankLoadRequest.isDone)
			{
				yield return null;
			}
			m_initializationState = ((AssetManagerError.op_Implicit(bankLoadRequest.error) == 0) ? InitializationState.Loaded : InitializationState.Error);
		}

		protected IEnumerator Load(params AudioReferenceWithParameters[] audioReferences)
		{
			int num = audioReferences.Length;
			int bankCount = 0;
			if (num == 0)
			{
				m_initializationState = InitializationState.Loaded;
				yield break;
			}
			string[] array = new string[num];
			for (int i = 0; i < num; i++)
			{
				AudioReferenceWithParameters val = audioReferences[i];
				if (!val.get_isValid() || !AudioManager.TryGetDefaultBankName(AudioReferenceWithParameters.op_Implicit(val), out string bankName))
				{
					continue;
				}
				int num2 = 0;
				while (true)
				{
					if (num2 < bankCount)
					{
						if (bankName.Equals(array[num2]))
						{
							break;
						}
						num2++;
						continue;
					}
					array[bankCount] = bankName;
					int num3 = bankCount + 1;
					bankCount = num3;
					break;
				}
			}
			if (bankCount == 0)
			{
				m_initializationState = InitializationState.Error;
				yield break;
			}
			AudioBankLoadRequest[] bankLoadRequests = new AudioBankLoadRequest[bankCount];
			for (int j = 0; j < bankCount; j++)
			{
				bankLoadRequests[j] = AudioManager.LoadBankAsync(array[j]);
			}
			m_bankLoadRequests = bankLoadRequests;
			m_initializationState = InitializationState.Loading;
			yield return EnumeratorUtility.ParallelRecursiveImmediateSafeExecution((IEnumerator[])bankLoadRequests);
			for (int k = 0; k < bankCount; k++)
			{
				if (AssetManagerError.op_Implicit(bankLoadRequests[k].error) == 0)
				{
					m_initializationState = InitializationState.Loaded;
					yield break;
				}
			}
			m_initializationState = InitializationState.Error;
		}

		protected void Unload()
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			if (!AudioManager.isReady)
			{
				return;
			}
			AudioBankLoadRequest[] bankLoadRequests = m_bankLoadRequests;
			if (bankLoadRequests == null)
			{
				return;
			}
			int num = bankLoadRequests.Length;
			for (int i = 0; i < num; i++)
			{
				AudioBankLoadRequest audioBankLoadRequest = m_bankLoadRequests[i];
				if (!audioBankLoadRequest.isDone)
				{
					audioBankLoadRequest.Cancel();
				}
				else if (AssetManagerError.op_Implicit(audioBankLoadRequest.error) == 0)
				{
					AudioManager.UnloadBank(audioBankLoadRequest.bankName);
				}
			}
			m_bankLoadRequests = null;
		}

		protected virtual void OnDestroy()
		{
			Unload();
		}

		protected AudioEventUILoader()
			: this()
		{
		}
	}
}
