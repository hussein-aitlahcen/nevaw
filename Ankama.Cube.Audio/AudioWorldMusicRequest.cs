using Ankama.AssetManagement;
using Ankama.Utilities;
using FMOD.Studio;
using FMODUnity;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Audio
{
	public sealed class AudioWorldMusicRequest : CustomYieldInstruction
	{
		public enum State
		{
			None,
			Loading,
			Loaded,
			Playing,
			Stopping,
			Stopped,
			Error
		}

		private enum PendingStateChange
		{
			None,
			Play,
			Stop
		}

		private static readonly List<AudioWorldMusicRequest> s_instances = new List<AudioWorldMusicRequest>();

		public readonly AudioReferenceWithParameters music;

		public readonly AudioReferenceWithParameters ambiance;

		public readonly AudioContext context;

		private PendingStateChange m_pendingState;

		private AudioBankLoadRequest m_musicBankLoadRequest;

		private AudioBankLoadRequest m_ambianceBankLoadRequest;

		public EventInstance musicEventInstance
		{
			get;
			private set;
		}

		public EventInstance ambianceEventInstance
		{
			get;
			private set;
		}

		public override bool keepWaiting
		{
			get
			{
				UpdateInternal();
				if (state != State.Loading)
				{
					return state == State.Stopping;
				}
				return true;
			}
		}

		public State state
		{
			get;
			private set;
		}

		public AssetManagerError error
		{
			get;
			private set;
		} = AssetManagerError.op_Implicit(0);


		internal static void ClearAllRequests()
		{
			List<AudioWorldMusicRequest> list = s_instances;
			int count = list.Count;
			for (int i = 0; i < count; i++)
			{
				list[i].CancelInternal();
			}
			s_instances.Clear();
		}

		internal AudioWorldMusicRequest(AudioReferenceWithParameters music, AudioReferenceWithParameters ambiance, [CanBeNull] AudioContext context, bool playAutomatically)
			: this()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			this.music = music;
			this.ambiance = ambiance;
			this.context = context;
			m_pendingState = (playAutomatically ? PendingStateChange.Play : PendingStateChange.None);
			Guid eventGuid = music.get_eventGuid();
			Guid eventGuid2 = ambiance.get_eventGuid();
			if (!AssetManager.get_isReady())
			{
				Log.Error("Tried to load a world music but the AudioManager isn't ready.", 85, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioWorldMusicRequest.cs");
				Fail(AssetManagerError.op_Implicit(60));
				return;
			}
			string bankName;
			if (eventGuid != Guid.Empty)
			{
				if (!AudioManager.TryGetDefaultBankName(eventGuid, out bankName))
				{
					Log.Warning($"Could not get default bank name for requested world music with guid {eventGuid}.", 95, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioWorldMusicRequest.cs");
					Fail(AssetManagerError.op_Implicit(10));
					return;
				}
			}
			else
			{
				bankName = string.Empty;
			}
			string bankName2;
			if (eventGuid2 != Guid.Empty)
			{
				if (!AudioManager.TryGetDefaultBankName(eventGuid2, out bankName2))
				{
					Log.Warning($"Could not get default bank name for requested world ambiance with guid {eventGuid}.", 110, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioWorldMusicRequest.cs");
					Fail(AssetManagerError.op_Implicit(10));
					return;
				}
			}
			else
			{
				bankName2 = string.Empty;
			}
			if (bankName.Length > 0)
			{
				m_musicBankLoadRequest = AudioManager.LoadBankAsync(bankName);
			}
			if (bankName2.Length > 0 && !bankName.Equals(bankName2))
			{
				m_ambianceBankLoadRequest = AudioManager.LoadBankAsync(bankName2);
			}
			state = State.Loading;
			s_instances.Add(this);
			UpdateInternal();
		}

		private void UpdateInternal()
		{
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Invalid comparison between Unknown and I4
			//IL_0205: Unknown result type (might be due to invalid IL or missing references)
			//IL_021f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0226: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Invalid comparison between Unknown and I4
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			switch (state)
			{
			case State.None:
			case State.Loaded:
			case State.Playing:
			case State.Stopped:
			case State.Error:
				break;
			case State.Loading:
			{
				if ((m_musicBankLoadRequest != null && !m_musicBankLoadRequest.isDone) || (m_ambianceBankLoadRequest != null && !m_ambianceBankLoadRequest.isDone))
				{
					break;
				}
				if (m_musicBankLoadRequest != null && AssetManagerError.op_Implicit(m_musicBankLoadRequest.error) != 0)
				{
					Log.Warning("Could not load audio bank named '" + m_musicBankLoadRequest.bankName + "' for requested world music.", 149, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioWorldMusicRequest.cs");
					Fail(m_musicBankLoadRequest.error);
					break;
				}
				if (m_ambianceBankLoadRequest != null && AssetManagerError.op_Implicit(m_ambianceBankLoadRequest.error) != 0)
				{
					Log.Warning("Could not load audio bank named '" + m_ambianceBankLoadRequest.bankName + "' for requested world ambiance.", 156, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioWorldMusicRequest.cs");
					Fail(m_ambianceBankLoadRequest.error);
					break;
				}
				AudioReferenceWithParameters val3 = music;
				if (val3.get_isValid() && AudioManager.TryCreateInstance(music, out EventInstance eventInstance))
				{
					if (context != null)
					{
						context.AddEventInstance(eventInstance);
					}
					val3 = music;
					val3.ApplyParameters(eventInstance);
					this.musicEventInstance = eventInstance;
				}
				val3 = ambiance;
				if (val3.get_isValid() && AudioManager.TryCreateInstance(ambiance, out EventInstance eventInstance2))
				{
					if (context != null)
					{
						context.AddEventInstance(eventInstance2);
					}
					val3 = ambiance;
					val3.ApplyParameters(eventInstance2);
					this.ambianceEventInstance = eventInstance2;
				}
				state = State.Loaded;
				switch (m_pendingState)
				{
				case PendingStateChange.None:
					break;
				case PendingStateChange.Play:
					StartInternal();
					break;
				case PendingStateChange.Stop:
					UnloadInternal();
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
				break;
			}
			case State.Stopping:
			{
				EventInstance musicEventInstance = this.musicEventInstance;
				EventInstance ambianceEventInstance = this.ambianceEventInstance;
				bool flag = false;
				if (musicEventInstance.isValid())
				{
					PLAYBACK_STATE val = default(PLAYBACK_STATE);
					if ((int)musicEventInstance.getPlaybackState(ref val) == 0 && (int)val != 2)
					{
						flag = true;
					}
					else
					{
						musicEventInstance.release();
						musicEventInstance.clearHandle();
					}
				}
				if (ambianceEventInstance.isValid())
				{
					PLAYBACK_STATE val2 = default(PLAYBACK_STATE);
					if ((int)ambianceEventInstance.getPlaybackState(ref val2) == 0 && (int)val2 != 2)
					{
						flag = true;
					}
					else
					{
						ambianceEventInstance.release();
						ambianceEventInstance.clearHandle();
					}
				}
				if (!flag)
				{
					UnloadInternal();
					state = State.Stopped;
				}
				break;
			}
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		public void Start()
		{
			if (m_pendingState == PendingStateChange.None)
			{
				m_pendingState = PendingStateChange.Play;
				if (state == State.Loaded)
				{
					StartInternal();
				}
			}
		}

		public void Stop()
		{
			if (m_pendingState != PendingStateChange.Stop)
			{
				m_pendingState = PendingStateChange.Stop;
				if (state == State.Playing)
				{
					StopInternal();
				}
				else if (state == State.Loaded)
				{
					UnloadInternal();
				}
			}
		}

		private void StartInternal()
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			state = State.Playing;
			EventInstance musicEventInstance = this.musicEventInstance;
			if (musicEventInstance.isValid())
			{
				musicEventInstance.start();
			}
			EventInstance ambianceEventInstance = this.ambianceEventInstance;
			if (ambianceEventInstance.isValid())
			{
				ambianceEventInstance.start();
			}
		}

		private void StopInternal()
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			state = State.Stopping;
			EventInstance musicEventInstance = this.musicEventInstance;
			EventInstance ambianceEventInstance = this.ambianceEventInstance;
			if (musicEventInstance.isValid())
			{
				musicEventInstance.stop(0);
			}
			if (musicEventInstance.isValid())
			{
				ambianceEventInstance.stop(0);
			}
			UpdateInternal();
		}

		private void UnloadInternal()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			if (m_musicBankLoadRequest != null)
			{
				if (AssetManagerError.op_Implicit(m_musicBankLoadRequest.error) == 0)
				{
					AudioManager.UnloadBank(m_musicBankLoadRequest.bankName);
				}
				m_musicBankLoadRequest = null;
			}
			if (m_ambianceBankLoadRequest != null)
			{
				if (AssetManagerError.op_Implicit(m_ambianceBankLoadRequest.error) == 0)
				{
					AudioManager.UnloadBank(m_ambianceBankLoadRequest.bankName);
				}
				m_ambianceBankLoadRequest = null;
			}
			s_instances.Remove(this);
		}

		private void CancelInternal()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			EventInstance musicEventInstance = this.musicEventInstance;
			if (musicEventInstance.isValid())
			{
				musicEventInstance.stop(1);
				musicEventInstance.release();
				musicEventInstance.clearHandle();
			}
			EventInstance ambianceEventInstance = this.ambianceEventInstance;
			if (ambianceEventInstance.isValid())
			{
				ambianceEventInstance.stop(1);
				ambianceEventInstance.release();
				ambianceEventInstance.clearHandle();
			}
			if (m_musicBankLoadRequest != null)
			{
				if (AssetManagerError.op_Implicit(m_musicBankLoadRequest.error) == 0)
				{
					AudioManager.UnloadBank(m_musicBankLoadRequest.bankName);
				}
				m_musicBankLoadRequest = null;
			}
			if (m_ambianceBankLoadRequest != null)
			{
				if (AssetManagerError.op_Implicit(m_ambianceBankLoadRequest.error) == 0)
				{
					AudioManager.UnloadBank(m_ambianceBankLoadRequest.bankName);
				}
				m_ambianceBankLoadRequest = null;
			}
			error = AssetManagerError.op_Implicit(50);
			state = State.Error;
		}

		private void Fail(AssetManagerError e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			error = e;
			state = State.Error;
			UnloadInternal();
		}
	}
}
