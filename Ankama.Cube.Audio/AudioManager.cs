using Ankama.AssetManagement;
using Ankama.AssetManagement.AssetBundles;
using Ankama.AssetManagement.AssetReferences;
using Ankama.Cube.Utility;
using Ankama.Utilities;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Ankama.Cube.Audio
{
	public static class AudioManager
	{
		private struct BankInfo
		{
			public Bank bank;

			public int referenceCount;

			public string bundleName;

			public AudioBankLoadRequest completedRequest;
		}

		private struct BankLoading
		{
			public AudioBankLoadRequest loadRequest;

			public int referenceCount;
		}

		private struct MusicInstance : IEquatable<MusicInstance>
		{
			private static int s_nextIdentifier;

			private readonly int m_identifier;

			public readonly Guid guid;

			public EventInstance eventInstance;

			public int referenceCounter;

			public MusicInstance(Guid eventGuid)
			{
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				m_identifier = ++s_nextIdentifier;
				guid = eventGuid;
				eventInstance = default(EventInstance);
				referenceCounter = 1;
			}

			public bool Equals(MusicInstance other)
			{
				return m_identifier == other.m_identifier;
			}

			public override bool Equals(object obj)
			{
				if (obj != null)
				{
					object obj2;
					if ((obj2 = obj) is MusicInstance)
					{
						MusicInstance musicInstance = (MusicInstance)obj2;
						return m_identifier == musicInstance.m_identifier;
					}
					return false;
				}
				return false;
			}

			public override int GetHashCode()
			{
				return m_identifier;
			}

			public static bool operator ==(MusicInstance lhs, MusicInstance rhs)
			{
				return lhs.m_identifier == rhs.m_identifier;
			}

			public static bool operator !=(MusicInstance lhs, MusicInstance rhs)
			{
				return lhs.m_identifier != rhs.m_identifier;
			}
		}

		private class GuidComparer : IEqualityComparer<Guid>
		{
			bool IEqualityComparer<Guid>.Equals(Guid x, Guid y)
			{
				return x.Equals(y);
			}

			int IEqualityComparer<Guid>.GetHashCode(Guid obj)
			{
				return obj.GetHashCode();
			}
		}

		[Flags]
		private enum InitializationState
		{
			None = 0x0,
			Assembly = 0x1,
			Variant = 0x2,
			GameDataBundle = 0x4,
			MasterBundle = 0x8
		}

		private const string UIMusicTransitionParameterName = "Music_Menu";

		private static InitializationState s_initializationState = InitializationState.None;

		private static bool s_alreadyStartingMusic;

		private static FMODSettings s_settings;

		private static AudioManagerCallbackSource s_callbackSource;

		private static AudioListenerPosition s_listenerPosition;

		private static System s_studioSystem;

		private static System s_lowLevelSystem;

		private static readonly Dictionary<string, BankInfo> s_banks = new Dictionary<string, BankInfo>(StringComparer.OrdinalIgnoreCase);

		private static readonly Dictionary<string, BankLoading> s_banksLoading = new Dictionary<string, BankLoading>(StringComparer.OrdinalIgnoreCase);

		private static readonly List<string> s_finishedBanksLoading = new List<string>();

		private static readonly Dictionary<Guid, EventDescription> s_cachedDescriptions = new Dictionary<Guid, EventDescription>(new GuidComparer());

		private static readonly List<AudioContext> s_audioContexts = new List<AudioContext>(16);

		private static MusicInstance s_worldMusicInstance;

		private static MusicInstance s_worldAmbianceInstance;

		private static readonly List<MusicInstance> s_uiMusicStack = new List<MusicInstance>(2);

		public static bool isReady
		{
			get;
			private set;
		}

		public static AssetManagerError error
		{
			get;
			private set;
		} = AssetManagerError.op_Implicit(0);


		public static System studioSystem => s_studioSystem;

		[CanBeNull]
		public static Coroutine StartCoroutine(IEnumerator routine)
		{
			MonoBehaviour val = (null != s_callbackSource) ? s_callbackSource : ((AudioManagerCallbackSource)Main.monoBehaviour);
			if (!(null != val))
			{
				return null;
			}
			return val.StartCoroutine(routine);
		}

		public static IEnumerator Load()
		{
			while (!AssetManager.get_isReady())
			{
				yield return null;
			}
			string audioBundleVariant = AssetBundlesUtility.GetAudioBundleVariant();
			if (!AssetManager.AddActiveVariant(audioBundleVariant))
			{
				error = AssetManagerError.op_Implicit(30);
				Log.Error("Could not initialize audio manager: could not add audio bundle variant '" + audioBundleVariant + "'.", 165, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioManager.cs");
				yield break;
			}
			s_initializationState |= InitializationState.Variant;
			AssetBundleLoadRequest settingsBundleLoadRequest = AssetManager.LoadAssetBundle("core/gamedata");
			while (!settingsBundleLoadRequest.get_isDone())
			{
				yield return null;
			}
			if (AssetManagerError.op_Implicit(settingsBundleLoadRequest.get_error()) != 0)
			{
				error = settingsBundleLoadRequest.get_error();
				Log.Error($"Could not load settings bundle: {error}", 182, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioManager.cs");
				yield break;
			}
			s_initializationState |= InitializationState.GameDataBundle;
			AllAssetsLoadRequest<FMODSettings> settingsLoadRequest = AssetManager.LoadAllAssetsAsync<FMODSettings>("core/gamedata");
			while (!settingsLoadRequest.get_isDone())
			{
				yield return null;
			}
			if (AssetManagerError.op_Implicit(settingsLoadRequest.get_error()) != 0)
			{
				error = settingsLoadRequest.get_error();
				Log.Error($"Could not load settings from master bundle: {error}", 199, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioManager.cs");
				yield break;
			}
			FMODSettings[] assets = settingsLoadRequest.get_assets();
			if (assets.Length == 0)
			{
				error = AssetManagerError.op_Implicit(30);
				Log.Error("Could not initialize audio manager: settings not found.", 207, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioManager.cs");
				yield break;
			}
			FMODSettings settings = assets[0];
			FMODRuntimeCache runtimeCache = settings.runtimeCache;
			s_settings = settings;
			RESULT val = StartSystem();
			if ((int)val != 0)
			{
				error = AssetManagerError.op_Implicit(30);
				Log.Error($"Audio manager did not initialize properly: {val}.", 219, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioManager.cs");
				yield break;
			}
			AssetBundleLoadRequest masterBankBundleLoadRequest = AssetManager.LoadAssetBundle("core/audio/master");
			while (!masterBankBundleLoadRequest.get_isDone())
			{
				yield return null;
			}
			if (AssetManagerError.op_Implicit(masterBankBundleLoadRequest.get_error()) != 0)
			{
				ReleaseSystemInternal();
				error = masterBankBundleLoadRequest.get_error();
				Log.Error($"Could not load master bank bundle: {error}", 236, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioManager.cs");
				yield break;
			}
			s_initializationState |= InitializationState.MasterBundle;
			if (!((Dictionary<string, AssetReference>)runtimeCache.bankReferenceDictionary).TryGetValue(settings.masterBankName + ".strings", out AssetReference value))
			{
				ReleaseSystemInternal();
				error = AssetManagerError.op_Implicit(10);
				Log.Error("Could not get strings bank reference.", 249, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioManager.cs");
				yield break;
			}
			AssetLoadRequest<TextAsset> stringsBankLoadRequest = value.LoadFromAssetBundleAsync<TextAsset>("core/audio/master");
			while (!stringsBankLoadRequest.get_isDone())
			{
				yield return null;
			}
			if (AssetManagerError.op_Implicit(stringsBankLoadRequest.get_error()) != 0)
			{
				ReleaseSystemInternal();
				error = stringsBankLoadRequest.get_error();
				Log.Error($"Could not load strings bank asset: {error}", 264, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioManager.cs");
				yield break;
			}
			Bank bank;
			RESULT val2 = LoadMasterBank(stringsBankLoadRequest.get_asset(), out bank);
			if ((int)val2 != 0)
			{
				ReleaseSystemInternal();
				error = AssetManagerError.op_Implicit(30);
				Log.Error($"Could not load strings bank: {val2}", 274, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioManager.cs");
				yield break;
			}
			if (!((Dictionary<string, AssetReference>)runtimeCache.bankReferenceDictionary).TryGetValue(settings.masterBankName, out AssetReference value2))
			{
				ReleaseSystemInternal();
				error = AssetManagerError.op_Implicit(10);
				Log.Error("Could not get master bank reference.", 285, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioManager.cs");
				yield break;
			}
			AssetLoadRequest<TextAsset> masterBankLoadRequest = value2.LoadFromAssetBundleAsync<TextAsset>("core/audio/master");
			while (!masterBankLoadRequest.get_isDone())
			{
				yield return null;
			}
			if (AssetManagerError.op_Implicit(masterBankLoadRequest.get_error()) != 0)
			{
				ReleaseSystemInternal();
				error = masterBankLoadRequest.get_error();
				Log.Error($"Could not load master bank asset: {error}", 300, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioManager.cs");
				yield break;
			}
			RESULT val3 = LoadMasterBank(masterBankLoadRequest.get_asset(), out bank);
			if ((int)val3 != 0)
			{
				ReleaseSystemInternal();
				error = AssetManagerError.op_Implicit(30);
				Log.Error($"Could not load master bank: {val3}", 310, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioManager.cs");
				yield break;
			}
			s_studioSystem.setNumListeners(1);
			if (null != s_listenerPosition)
			{
				s_studioSystem.setListenerAttributes(0, FMODUtility.To3DAttributes(s_listenerPosition.get_transform()));
			}
			else
			{
				s_studioSystem.setListenerAttributes(0, FMODUtility.To3DAttributes(Vector3.get_zero()));
			}
			s_callbackSource = AudioManagerCallbackSource.Create();
			isReady = true;
			Log.Info("Initialization complete.", 334, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioManager.cs");
		}

		public static IEnumerator Unload()
		{
			foreach (BankLoading value in s_banksLoading.Values)
			{
				value.loadRequest.Cancel();
			}
			s_banksLoading.Clear();
			if (s_studioSystem.isValid())
			{
				AudioWorldMusicRequest.ClearAllRequests();
				List<MusicInstance> list = s_uiMusicStack;
				int count = list.Count;
				for (int i = 0; i < count; i++)
				{
					EventInstance eventInstance = list[i].eventInstance;
					if (eventInstance.isValid())
					{
						eventInstance.stop(1);
						eventInstance.release();
						eventInstance.clearHandle();
					}
				}
				list.Clear();
				foreach (KeyValuePair<string, BankInfo> s_bank in s_banks)
				{
					Bank bank = s_bank.Value.bank;
					RESULT val = bank.unload();
					bank.clearHandle();
					if ((int)val != 0)
					{
						Log.Warning($"Error while unloading bank named '{s_bank.Key}' while unloading audio manager: {val}", 386, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioManager.cs");
					}
				}
			}
			s_banks.Clear();
			s_cachedDescriptions.Clear();
			ReleaseSystemInternal();
			if (null != s_callbackSource)
			{
				Object.Destroy(s_callbackSource.get_gameObject());
				s_callbackSource = null;
			}
			s_settings = null;
			if (s_initializationState.HasFlag(InitializationState.MasterBundle))
			{
				yield return AssetManager.UnloadAssetBundle("core/audio/master");
				s_initializationState &= ~InitializationState.MasterBundle;
			}
			if (s_initializationState.HasFlag(InitializationState.GameDataBundle))
			{
				yield return AssetManager.UnloadAssetBundle("core/gamedata");
				s_initializationState &= ~InitializationState.GameDataBundle;
			}
			if (s_initializationState.HasFlag(InitializationState.Variant))
			{
				AssetManager.RemoveActiveVariant(AssetBundlesUtility.GetAudioBundleVariant());
				s_initializationState &= ~InitializationState.Variant;
			}
		}

		public static void UpdateInternal()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			if (!s_studioSystem.isValid())
			{
				return;
			}
			s_studioSystem.update();
			List<AudioContext> list = s_audioContexts;
			int count = list.Count;
			for (int i = 0; i < count; i++)
			{
				list[i].Cleanup();
			}
			if (s_banksLoading.Count > 0)
			{
				foreach (KeyValuePair<string, BankLoading> item in s_banksLoading)
				{
					BankLoading value = item.Value;
					AudioBankLoadRequest loadRequest = value.loadRequest;
					if (loadRequest.UpdateInternal())
					{
						string key = item.Key;
						if (AssetManagerError.op_Implicit(loadRequest.error) == 0)
						{
							BankInfo value2 = default(BankInfo);
							value2.bank = loadRequest.bank;
							value2.referenceCount = value.referenceCount;
							value2.bundleName = value.loadRequest.bundleName;
							value2.completedRequest = loadRequest;
							s_banks.Add(key, value2);
						}
						s_finishedBanksLoading.Add(key);
					}
				}
				List<string> list2 = s_finishedBanksLoading;
				int count2 = list2.Count;
				for (int j = 0; j < count2; j++)
				{
					string key2 = list2[j];
					s_banksLoading.Remove(key2);
				}
				s_finishedBanksLoading.Clear();
			}
		}

		private static RESULT StartSystem()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Invalid comparison between Unknown and I4
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Expected O, but got Unknown
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0225: Unknown result type (might be due to invalid IL or missing references)
			RESULT val = 0;
			try
			{
				if (!s_initializationState.HasFlag(InitializationState.Assembly))
				{
					FMODUtility.EnforceLibraryOrder();
					s_initializationState |= InitializationState.Assembly;
				}
				FMODSettings obj = s_settings;
				FMODPlatform currentPlatform = FMODUtility.get_currentPlatform();
				int sampleRate = s_settings.GetSampleRate(currentPlatform);
				int realChannels = obj.GetRealChannels(currentPlatform);
				int virtualChannels = obj.GetVirtualChannels(currentPlatform);
				SPEAKERMODE val2 = obj.GetSpeakerMode(currentPlatform);
				OUTPUTTYPE output = 0;
				ADVANCEDSETTINGS val3 = new ADVANCEDSETTINGS
				{
					randomSeed = (uint)DateTime.Now.Ticks,
					maxVorbisCodecs = realChannels
				};
				SetThreadAffinity();
				INITFLAGS val4 = 8;
				if (obj.IsLiveUpdateEnabled(currentPlatform))
				{
					val4 |= 1;
				}
				while (true)
				{
					CheckInitResult(System.create(ref s_studioSystem), "FMOD.Studio.System.create");
					CheckInitResult(s_studioSystem.getLowLevelSystem(ref s_lowLevelSystem), "FMOD.Studio.System.getLowLevelSystem");
					CheckInitResult(s_lowLevelSystem.setOutput(output), "FMOD.System.setOutput");
					CheckInitResult(s_lowLevelSystem.setSoftwareChannels(realChannels), "FMOD.System.setSoftwareChannels");
					CheckInitResult(s_lowLevelSystem.setSoftwareFormat(sampleRate, val2, 0), "FMOD.System.setSoftwareFormat");
					CheckInitResult(s_lowLevelSystem.setAdvancedSettings(ref val3), "FMOD.System.setAdvancedSettings");
					RESULT val5 = s_studioSystem.initialize(virtualChannels, val4, 0, IntPtr.Zero);
					if ((int)val5 != 0 && (int)val == 0)
					{
						val = val5;
						output = 2;
						Log.Error($"[FMOD] Studio::System::initialize returned {val5}, defaulting to no-sound mode.", 572, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioManager.cs");
					}
					else
					{
						CheckInitResult(val5, "Studio::System::initialize");
						if ((val4 & 1) == 0)
						{
							break;
						}
						s_studioSystem.flushCommands();
						if ((int)s_studioSystem.update() != 43)
						{
							return val;
						}
						val4 &= -2;
						Log.Warning("[FMOD] Cannot open network port for Live Update (in-use), restarting with Live Update disabled.", 587, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioManager.cs");
						CheckInitResult(s_studioSystem.release(), "FMOD.Studio.System.Release");
					}
				}
				return val;
			}
			catch (SystemNotInitializedException val6)
			{
				SystemNotInitializedException val7 = val6;
				val = val7.result;
				Log.Error("Encountered an error while starting FMOD systems: " + ((Exception)val7).Message, 599, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioManager.cs");
				return val;
			}
			catch (Exception ex)
			{
				val = 26;
				Log.Error("Encountered an exception while starting FMOD systems: " + ex.Message + ".", 604, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioManager.cs");
				return val;
			}
		}

		public static void BackupSystemInternal(out IntPtr studioHandle, out IntPtr lowLevelStudioHandle)
		{
			if (s_studioSystem.isValid())
			{
				studioHandle = s_studioSystem.handle;
				lowLevelStudioHandle = s_lowLevelSystem.handle;
			}
			else
			{
				studioHandle = IntPtr.Zero;
				lowLevelStudioHandle = IntPtr.Zero;
			}
		}

		public static void RestoreSystemInternal(IntPtr studioHandle, IntPtr lowLevelStudioHandle)
		{
			s_studioSystem.handle = studioHandle;
			s_lowLevelSystem.handle = lowLevelStudioHandle;
		}

		public static void PauseSystemInternal(bool paused)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			if (s_studioSystem.isValid())
			{
				if (isReady)
				{
					Pause(paused);
				}
				if (paused)
				{
					s_lowLevelSystem.mixerSuspend();
				}
				else
				{
					s_lowLevelSystem.mixerResume();
				}
			}
		}

		public static void ReleaseSystemInternal()
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			if (s_studioSystem.isValid())
			{
				RESULT val = s_studioSystem.release();
				s_studioSystem.clearHandle();
				if ((int)val != 0)
				{
					Log.Warning($"Error while releasing system: {val}", 661, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioManager.cs");
				}
			}
			isReady = false;
		}

		private static void CheckInitResult(RESULT result, string cause)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			if ((int)result != 0)
			{
				if (s_studioSystem.isValid())
				{
					s_studioSystem.release();
					s_studioSystem.clearHandle();
				}
				throw new SystemNotInitializedException(result, cause);
			}
		}

		private static void SetThreadAffinity()
		{
		}

		[PublicAPI]
		[NotNull]
		public static AudioBankLoadRequest LoadBankAsync([NotNull] string bankName, bool loadSamples = true)
		{
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			if (s_banks.TryGetValue(bankName, out BankInfo value))
			{
				value.referenceCount++;
				s_banks[bankName] = value;
				return value.completedRequest;
			}
			if (s_banksLoading.TryGetValue(bankName, out BankLoading value2))
			{
				value2.referenceCount++;
				if (loadSamples)
				{
					value2.loadRequest.RequestSampleLoadingInternal();
				}
				s_banksLoading[bankName] = value2;
				return value2.loadRequest;
			}
			if (!((Dictionary<string, AssetReference>)s_settings.runtimeCache.bankReferenceDictionary).TryGetValue(bankName, out AssetReference value3))
			{
				return new AudioBankLoadRequest(bankName, "Bank name '" + bankName + "' does not exist in the settings.");
			}
			if (!AssetBundlesUtility.TryGetAudioBundleName(bankName, out string bundleName))
			{
				return new AudioBankLoadRequest(bankName, "Bank name '" + bankName + "' does not follow nomenclature.");
			}
			AudioBankLoadRequest audioBankLoadRequest = new AudioBankLoadRequest(bankName, value3, bundleName, loadSamples);
			if (audioBankLoadRequest.isDone)
			{
				if (AssetManagerError.op_Implicit(audioBankLoadRequest.error) == 0)
				{
					value.bank = audioBankLoadRequest.bank;
					value.referenceCount = 1;
					value.bundleName = bundleName;
					value.completedRequest = audioBankLoadRequest;
					s_banks.Add(bankName, value);
				}
			}
			else
			{
				value2.loadRequest = audioBankLoadRequest;
				value2.referenceCount = 1;
				s_banksLoading.Add(bankName, value2);
			}
			return audioBankLoadRequest;
		}

		[PublicAPI]
		public static void UnloadBank([NotNull] string bankName)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			BankLoading value2;
			if (s_banks.TryGetValue(bankName, out BankInfo value))
			{
				value.referenceCount--;
				if (value.referenceCount == 0)
				{
					Bank bank = value.bank;
					RESULT val = bank.unload();
					bank.clearHandle();
					s_banks.Remove(bankName);
					AssetManager.UnloadAssetBundle(value.bundleName);
					if ((int)val != 0)
					{
						Log.Warning($"Error while unloading bank named '{bankName}': {val}", 807, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioManager.cs");
					}
				}
				else
				{
					s_banks[bankName] = value;
				}
			}
			else if (s_banksLoading.TryGetValue(bankName, out value2))
			{
				value2.referenceCount--;
				if (value2.referenceCount == 0)
				{
					value2.loadRequest.Cancel();
					s_banksLoading.Remove(bankName);
				}
				else
				{
					s_banksLoading[bankName] = value2;
				}
			}
			else
			{
				Log.Warning("Could not unload bank named '" + bankName + "' because it is neither loaded nor loading.", 836, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioManager.cs");
			}
		}

		[PublicAPI]
		public static bool TryGetDefaultBankName(AudioReference audioReference, [NotNull] out string bankName)
		{
			if (!isReady)
			{
				Log.Error("Tried to get a default bank name but the AudioManager isn't ready.", 848, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioManager.cs");
				bankName = string.Empty;
				return false;
			}
			FMODRuntimeCache runtimeCache = s_settings.runtimeCache;
			if (!((Dictionary<string, int>)runtimeCache.eventDefaultBankDictionary).TryGetValue(audioReference.get_eventGuidString(), out int value))
			{
				bankName = string.Empty;
				return false;
			}
			bankName = runtimeCache.bankNameList[value];
			return true;
		}

		[PublicAPI]
		public static bool TryGetDefaultBankName(Guid eventGuid, [NotNull] out string bankName)
		{
			if (!isReady)
			{
				Log.Error("Tried to get a default bank name but the AudioManager isn't ready.", 874, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioManager.cs");
				bankName = string.Empty;
				return false;
			}
			FMODRuntimeCache runtimeCache = s_settings.runtimeCache;
			if (!((Dictionary<string, int>)runtimeCache.eventDefaultBankDictionary).TryGetValue(eventGuid.ToString("N"), out int value))
			{
				bankName = string.Empty;
				return false;
			}
			bankName = runtimeCache.bankNameList[value];
			return true;
		}

		[PublicAPI]
		public static bool TryGetDefaultBankName([NotNull] string eventGuid, [NotNull] out string bankName)
		{
			FMODRuntimeCache runtimeCache = s_settings.runtimeCache;
			if (!((Dictionary<string, int>)runtimeCache.eventDefaultBankDictionary).TryGetValue(eventGuid, out int value))
			{
				bankName = string.Empty;
				return false;
			}
			bankName = runtimeCache.bankNameList[value];
			return true;
		}

		private static RESULT LoadMasterBank([NotNull] TextAsset asset, out Bank bank)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			string name = asset.get_name();
			BankInfo value = default(BankInfo);
			RESULT val = s_studioSystem.loadBankMemoryPoint(asset.get_bytes(), 0, ref bank);
			if ((int)val == 0)
			{
				value.bank = bank;
				value.referenceCount = 1;
				value.completedRequest = new AudioBankLoadRequest(name);
				s_banks.Add(name, value);
			}
			else
			{
				Log.Error($"Could not load bank named '{name}': {val}", 932, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioManager.cs");
			}
			return val;
		}

		[PublicAPI]
		public static bool TryCreateInstance(string path, out EventInstance eventInstance)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			if (!TryGetGuidFromPath(path, out Guid guid))
			{
				eventInstance = default(EventInstance);
				return false;
			}
			if (!TryGetEventDescription(guid, out EventDescription eventDescription))
			{
				eventInstance = default(EventInstance);
				return false;
			}
			RESULT val = eventDescription.createInstance(ref eventInstance);
			if ((int)val != 0)
			{
				Log.Warning($"Could not create event instance for path {path} (guid: {guid}): {val}", 962, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioManager.cs");
				return false;
			}
			return true;
		}

		[PublicAPI]
		public static bool TryCreateInstance(string path, Transform transform, out EventInstance eventInstance)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			if (!TryGetGuidFromPath(path, out Guid guid))
			{
				eventInstance = default(EventInstance);
				return false;
			}
			if (!TryGetEventDescription(guid, out EventDescription eventDescription))
			{
				eventInstance = default(EventInstance);
				return false;
			}
			RESULT val = eventDescription.createInstance(ref eventInstance);
			if ((int)val != 0)
			{
				Log.Warning($"Could not create event instance for path {path} (guid: {guid}): {val}", 989, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioManager.cs");
				return false;
			}
			Vector3 val2 = (null != transform) ? transform.get_position() : ((null != s_listenerPosition) ? s_listenerPosition.get_transform().get_position() : Vector3.get_zero());
			eventInstance.set3DAttributes(FMODUtility.To3DAttributes(val2));
			return true;
		}

		[PublicAPI]
		public static bool TryCreateInstance(Guid guid, out EventInstance eventInstance)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			if (!TryGetEventDescription(guid, out EventDescription eventDescription))
			{
				eventInstance = default(EventInstance);
				return false;
			}
			RESULT val = eventDescription.createInstance(ref eventInstance);
			if ((int)val != 0)
			{
				Log.Warning($"Could not create event instance for guid {guid}: {val}", 1019, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioManager.cs");
				return false;
			}
			return true;
		}

		[PublicAPI]
		public static bool TryCreateInstance(Guid guid, Transform transform, out EventInstance eventInstance)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			if (!TryGetEventDescription(guid, out EventDescription eventDescription))
			{
				eventInstance = default(EventInstance);
				return false;
			}
			RESULT val = eventDescription.createInstance(ref eventInstance);
			if ((int)val != 0)
			{
				Log.Warning($"Could not create event instance for guid {guid}: {val}", 1040, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioManager.cs");
				return false;
			}
			Vector3 val2 = (null != transform) ? transform.get_position() : ((null != s_listenerPosition) ? s_listenerPosition.get_transform().get_position() : Vector3.get_zero());
			eventInstance.set3DAttributes(FMODUtility.To3DAttributes(val2));
			return true;
		}

		[PublicAPI]
		public static bool TryCreateInstance(AudioReference audioReference, out EventInstance eventInstance)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			if (!TryGetEventDescription(audioReference.get_eventGuid(), out EventDescription eventDescription))
			{
				eventInstance = default(EventInstance);
				return false;
			}
			RESULT val = eventDescription.createInstance(ref eventInstance);
			if ((int)val != 0)
			{
				Log.Warning($"Could not create event instance for guid {audioReference.get_eventGuid()}: {val}", 1070, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioManager.cs");
				return false;
			}
			return true;
		}

		[PublicAPI]
		public static bool TryCreateInstance(AudioReference audioReference, Transform transform, out EventInstance eventInstance)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			if (!TryGetEventDescription(audioReference.get_eventGuid(), out EventDescription eventDescription))
			{
				eventInstance = default(EventInstance);
				return false;
			}
			RESULT val = eventDescription.createInstance(ref eventInstance);
			if ((int)val != 0)
			{
				Log.Warning($"Could not create event instance for guid {audioReference.get_eventGuid()}: {val}", 1091, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioManager.cs");
				return false;
			}
			Vector3 val2 = (null != transform) ? transform.get_position() : ((null != s_listenerPosition) ? s_listenerPosition.get_transform().get_position() : Vector3.get_zero());
			eventInstance.set3DAttributes(FMODUtility.To3DAttributes(val2));
			return true;
		}

		[PublicAPI]
		public static bool TryCreateInstance(AudioReferenceWithParameters audioReference, out EventInstance eventInstance)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			if (!TryGetEventDescription(audioReference.get_eventGuid(), out EventDescription eventDescription))
			{
				eventInstance = default(EventInstance);
				return false;
			}
			RESULT val = eventDescription.createInstance(ref eventInstance);
			if ((int)val != 0)
			{
				Log.Warning($"Could not create event instance for guid {audioReference.get_eventGuid()}: {val}", 1121, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioManager.cs");
				return false;
			}
			audioReference.ApplyParameters(eventInstance);
			return true;
		}

		[PublicAPI]
		public static bool TryCreateInstance(AudioReferenceWithParameters audioReference, Transform transform, out EventInstance eventInstance)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			if (!TryGetEventDescription(audioReference.get_eventGuid(), out EventDescription eventDescription))
			{
				eventInstance = default(EventInstance);
				return false;
			}
			RESULT val = eventDescription.createInstance(ref eventInstance);
			if ((int)val != 0)
			{
				Log.Warning($"Could not create event instance for guid {audioReference.get_eventGuid()}: {val}", 1144, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioManager.cs");
				return false;
			}
			audioReference.ApplyParameters(eventInstance);
			Vector3 val2 = (null != transform) ? transform.get_position() : ((null != s_listenerPosition) ? s_listenerPosition.get_transform().get_position() : Vector3.get_zero());
			eventInstance.set3DAttributes(FMODUtility.To3DAttributes(val2));
			return true;
		}

		[PublicAPI]
		public static void PlayOneShot(Guid guid, Transform transform = null)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			if (isReady && TryCreateInstance(guid, transform, out EventInstance eventInstance))
			{
				eventInstance.start();
				eventInstance.release();
				eventInstance.clearHandle();
			}
		}

		[PublicAPI]
		public static void PlayOneShot([NotNull] string path, Transform transform = null)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			Guid guid;
			if (isReady && TryGetGuidFromPath(path, out guid) && TryCreateInstance(guid, transform, out EventInstance eventInstance))
			{
				eventInstance.start();
				eventInstance.release();
				eventInstance.clearHandle();
			}
		}

		[PublicAPI]
		public static void PlayOneShot(AudioReference audioReference, Transform transform = null)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			if (isReady && TryCreateInstance(audioReference.get_eventGuid(), transform, out EventInstance eventInstance))
			{
				eventInstance.start();
				eventInstance.release();
				eventInstance.clearHandle();
			}
		}

		[PublicAPI]
		public static void PlayOneShot(AudioReferenceWithParameters audioReference, Transform transform = null)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			if (isReady && TryCreateInstance(audioReference.get_eventGuid(), transform, out EventInstance eventInstance))
			{
				audioReference.ApplyParameters(eventInstance);
				eventInstance.start();
				eventInstance.release();
				eventInstance.clearHandle();
			}
		}

		private static bool TryGetGuidFromPath([NotNull] string path, out Guid guid)
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Invalid comparison between Unknown and I4
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			if (path.Length == 0)
			{
				Log.Warning("An empty path cannot be converted into a Guid.", 1253, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioManager.cs");
				guid = default(Guid);
				return false;
			}
			if (path[0] == '{')
			{
				if ((int)Util.ParseID(path, ref guid) != 0)
				{
					Log.Warning("Could not parse path '" + path + "' into a valid Guid.", 1263, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioManager.cs");
					return false;
				}
			}
			else
			{
				RESULT val = s_studioSystem.lookupID(path, ref guid);
				if ((int)val != 0)
				{
					if ((int)val == 74)
					{
						Log.Warning("Could not find an event for path '" + path + "'.", 1274, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioManager.cs");
					}
					else
					{
						Log.Warning($"Could not find guid for event path '{path}' : {val}.", 1278, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioManager.cs");
					}
					return false;
				}
			}
			return true;
		}

		private static bool TryGetEventDescription(Guid guid, out EventDescription eventDescription)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			if (s_cachedDescriptions.TryGetValue(guid, out eventDescription) && eventDescription.isValid())
			{
				return true;
			}
			RESULT eventByID = s_studioSystem.getEventByID(guid, ref eventDescription);
			if ((int)eventByID != 0)
			{
				Log.Warning($"Could not find event description for guid {guid}: {eventByID}", 1301, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioManager.cs");
				return false;
			}
			s_cachedDescriptions[guid] = eventDescription;
			return true;
		}

		[PublicAPI]
		public static AudioWorldMusicRequest LoadWorldMusic(AudioEventGroup musicAudioGroup, AudioEventGroup ambianceAudioGroup, [CanBeNull] AudioContext audioContext = null, bool playAutomatically = false)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			musicAudioGroup.Collapse();
			_003F music = musicAudioGroup.isValid ? musicAudioGroup.instance : default(AudioReferenceWithParameters);
			ambianceAudioGroup.Collapse();
			AudioReferenceWithParameters ambiance = ambianceAudioGroup.isValid ? ambianceAudioGroup.instance : default(AudioReferenceWithParameters);
			AudioWorldMusicRequest audioWorldMusicRequest = new AudioWorldMusicRequest(music, ambiance, audioContext, playAutomatically);
			StartCoroutine((IEnumerator)audioWorldMusicRequest);
			return audioWorldMusicRequest;
		}

		[PublicAPI]
		public static void StartWorldMusic([NotNull] AudioWorldMusicRequest request)
		{
			request.Start();
		}

		[PublicAPI]
		public static void StopWorldMusic([NotNull] AudioWorldMusicRequest request)
		{
			request.Stop();
			StartCoroutine((IEnumerator)request);
		}

		[PublicAPI]
		public static IEnumerator StartUIMusic(AudioReferenceWithParameters audioReference)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			Guid eventGuid = audioReference.get_eventGuid();
			if (eventGuid == Guid.Empty)
			{
				yield break;
			}
			int count = s_uiMusicStack.Count;
			if (count > 0)
			{
				MusicInstance musicInstance2 = s_uiMusicStack[count - 1];
				if (musicInstance2.guid == eventGuid)
				{
					musicInstance2.referenceCounter++;
					s_uiMusicStack[count - 1] = musicInstance2;
					yield break;
				}
			}
			while (!isReady)
			{
				if (AssetManagerError.op_Implicit(error) != 0)
				{
					yield break;
				}
				yield return null;
			}
			if (!TryGetDefaultBankName(AudioReferenceWithParameters.op_Implicit(audioReference), out string bankName))
			{
				Log.Warning($"Could not start requested UI music with guid {eventGuid} because no default bank could be found.", 1376, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioManager.cs");
				yield break;
			}
			MusicInstance musicInstance = new MusicInstance(eventGuid);
			s_uiMusicStack.Add(musicInstance);
			AudioBankLoadRequest bankLoadRequest = LoadBankAsync(bankName);
			while (!bankLoadRequest.isDone)
			{
				yield return null;
			}
			if (AssetManagerError.op_Implicit(bankLoadRequest.error) != 0)
			{
				s_uiMusicStack.Remove(musicInstance);
				yield break;
			}
			count = s_uiMusicStack.Count;
			PLAYBACK_STATE val2 = default(PLAYBACK_STATE);
			int num = default(int);
			for (int i = 0; i < count; i++)
			{
				if (!(s_uiMusicStack[i] == musicInstance))
				{
					continue;
				}
				if (TryCreateInstance(eventGuid, out EventInstance eventInstance))
				{
					audioReference.ApplyParameters(eventInstance);
					if (i == count - 1)
					{
						Vector3 val = (null != s_listenerPosition) ? s_listenerPosition.get_transform().get_position() : Vector3.get_zero();
						eventInstance.set3DAttributes(FMODUtility.To3DAttributes(val));
						eventInstance.setParameterValue("Music_Menu", 0f);
						eventInstance.start();
						if (i > 0)
						{
							EventInstance eventInstance2 = s_uiMusicStack[i - 1].eventInstance;
							if (eventInstance2.isValid())
							{
								eventInstance2.getPlaybackState(ref val2);
								eventInstance2.getTimelinePosition(ref num);
								Log.Info(val2 + ", " + num, 1422, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioManager.cs");
								if ((int)eventInstance2.setParameterValue("Music_Menu", 1f) != 0)
								{
									eventInstance.stop(0);
								}
							}
						}
					}
					musicInstance.eventInstance = eventInstance;
					s_uiMusicStack[i] = musicInstance;
					yield break;
				}
				s_uiMusicStack.RemoveAt(i);
				break;
			}
			UnloadBank(bankName);
		}

		[PublicAPI]
		public static IEnumerator StopUIMusic(AudioReferenceWithParameters audioReference)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			Guid eventGuid = audioReference.get_eventGuid();
			if (eventGuid == Guid.Empty || !isReady)
			{
				yield break;
			}
			int count = s_uiMusicStack.Count;
			int num = 0;
			MusicInstance musicInstance;
			while (true)
			{
				if (num < count)
				{
					musicInstance = s_uiMusicStack[num];
					if (musicInstance.guid == eventGuid)
					{
						break;
					}
					musicInstance = default(MusicInstance);
					num++;
					continue;
				}
				yield break;
			}
			musicInstance.referenceCounter--;
			if (musicInstance.referenceCounter > 0)
			{
				s_uiMusicStack[num] = musicInstance;
				yield break;
			}
			s_uiMusicStack.RemoveAt(num);
			EventInstance eventInstance = musicInstance.eventInstance;
			if (num != count - 1)
			{
				yield break;
			}
			if (num > 0)
			{
				EventInstance eventInstance2 = s_uiMusicStack[num - 1].eventInstance;
				if (eventInstance2.isValid())
				{
					Vector3 val = (null != s_listenerPosition) ? s_listenerPosition.get_transform().get_position() : Vector3.get_zero();
					eventInstance2.set3DAttributes(FMODUtility.To3DAttributes(val));
					eventInstance2.setParameterValue("Music_Menu", 0f);
					eventInstance2.start();
				}
			}
			if (eventInstance.isValid())
			{
				if ((int)eventInstance.setParameterValue("Music_Menu", 1f) != 0)
				{
					eventInstance.stop(0);
				}
				PLAYBACK_STATE val2 = default(PLAYBACK_STATE);
				do
				{
					yield return null;
					eventInstance.getPlaybackState(ref val2);
				}
				while ((int)val2 != 2);
				eventInstance.release();
				eventInstance.clearHandle();
				if (TryGetDefaultBankName(musicInstance.guid, out string bankName))
				{
					UnloadBank(bankName);
				}
			}
		}

		[PublicAPI]
		public static void AddAudioContext([NotNull] AudioContext context)
		{
			s_audioContexts.Add(context);
		}

		[PublicAPI]
		public static void RemoveAudioContext([NotNull] AudioContext context)
		{
			s_audioContexts.Remove(context);
		}

		[PublicAPI]
		public static void Pause(bool paused)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			if (isReady)
			{
				Bus val = default(Bus);
				RESULT bus = s_studioSystem.getBus("bus:/", ref val);
				if ((int)bus != 0)
				{
					Log.Warning($"Could not get bus: {bus}", 1574, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioManager.cs");
				}
				else
				{
					val.setPaused(paused);
				}
			}
		}

		[PublicAPI]
		public static void Mute(bool muted)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			if (isReady)
			{
				Bus val = default(Bus);
				RESULT bus = s_studioSystem.getBus("bus:/", ref val);
				if ((int)bus != 0)
				{
					Log.Warning($"Could not get bus: {bus}", 1594, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioManager.cs");
				}
				else
				{
					val.setMute(muted);
				}
			}
		}

		[PublicAPI]
		public static bool TryGetVolume(AudioBusIdentifier busIdentifier, out float volume)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			if (!isReady)
			{
				volume = 0f;
				return false;
			}
			if (!TryGetBus(busIdentifier, out Bus bus))
			{
				volume = 0f;
				return false;
			}
			float num = default(float);
			RESULT volume2 = bus.getVolume(ref volume, ref num);
			if ((int)volume2 != 0)
			{
				Log.Warning($"Could not get volume for bus {busIdentifier}: {volume2}", 1619, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioManager.cs");
				return false;
			}
			return true;
		}

		[PublicAPI]
		public static bool SetVolume(AudioBusIdentifier busIdentifier, float volume)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			if (!isReady)
			{
				return false;
			}
			if (!TryGetBus(busIdentifier, out Bus bus))
			{
				return false;
			}
			RESULT val = bus.setVolume(volume);
			if ((int)val != 0)
			{
				Log.Warning($"Could not set volume for bus {busIdentifier}: {val}", 1642, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioManager.cs");
				return false;
			}
			return true;
		}

		private static bool TryGetBus(AudioBusIdentifier busIdentifier, out Bus bus)
		{
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			string text;
			switch (busIdentifier)
			{
			case AudioBusIdentifier.Master:
				text = "bus:/";
				break;
			case AudioBusIdentifier.Music:
				text = "bus:/MUSIC";
				break;
			case AudioBusIdentifier.SFX:
				text = "bus:/SFX";
				break;
			case AudioBusIdentifier.UI:
				text = "bus:/UI";
				break;
			default:
				throw new ArgumentOutOfRangeException("busIdentifier", busIdentifier, null);
			}
			RESULT bus2 = s_studioSystem.getBus(text, ref bus);
			if ((int)bus2 != 0)
			{
				Log.Warning($"Could not get bus: {bus2}", 1675, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioManager.cs");
				return false;
			}
			return true;
		}

		[PublicAPI]
		public static void RegisterListenerPosition([NotNull] AudioListenerPosition instance)
		{
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			if (null != s_listenerPosition && instance != s_listenerPosition)
			{
				Log.Warning("Registering a listener while another listener is already registered, previous listener will be ignored.", 1693, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioManager.cs");
			}
			s_listenerPosition = instance;
			if (isReady)
			{
				s_studioSystem.setListenerAttributes(0, FMODUtility.To3DAttributes(s_listenerPosition.get_transform()));
			}
		}

		[PublicAPI]
		public static void UpdateListenerPosition([NotNull] AudioListenerPosition instance)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			if (instance == s_listenerPosition)
			{
				s_studioSystem.setListenerAttributes(0, FMODUtility.To3DAttributes(s_listenerPosition.get_transform()));
			}
		}

		[PublicAPI]
		public static void UnRegisterListenerPosition([NotNull] AudioListenerPosition instance)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			if (instance == s_listenerPosition)
			{
				s_studioSystem.setListenerAttributes(0, FMODUtility.To3DAttributes(Vector3.get_zero()));
				s_listenerPosition = null;
			}
		}

		[Conditional("UNITY_EDITOR")]
		private static void LogBankLoading(string bankName)
		{
		}

		[Conditional("UNITY_EDITOR")]
		private static void LogBankLoaded(string bankName)
		{
		}

		[Conditional("UNITY_EDITOR")]
		private static void LogBankUnloaded(string bankName)
		{
		}

		[Conditional("UNITY_EDITOR")]
		private static void LogBankCancelled(string bankName)
		{
		}
	}
}
