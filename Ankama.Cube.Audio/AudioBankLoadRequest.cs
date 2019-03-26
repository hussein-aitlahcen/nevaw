using Ankama.AssetManagement;
using Ankama.AssetManagement.AssetBundles;
using Ankama.AssetManagement.AssetReferences;
using Ankama.Utilities;
using FMOD;
using FMOD.Studio;
using JetBrains.Annotations;
using System;
using UnityEngine;

namespace Ankama.Cube.Audio
{
	public class AudioBankLoadRequest : CustomYieldInstruction
	{
		public readonly string bankName;

		public readonly string bundleName;

		private AssetReference m_assetReference;

		private AssetBundleLoadRequest m_bundleLoadRequest;

		private AssetLoadRequest<TextAsset> m_assetLoadRequest;

		private bool m_loadingSamples;

		public bool loadSamples
		{
			get;
			private set;
		}

		public Bank bank
		{
			get;
			private set;
		}

		public bool isDone
		{
			get;
			private set;
		}

		public AssetManagerError error
		{
			[NotNull]
			get;
			private set;
		} = AssetManagerError.op_Implicit(0);


		public override bool keepWaiting => !isDone;

		public AudioBankLoadRequest(string bankName)
			: this()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			this.bankName = bankName;
			bundleName = string.Empty;
			isDone = true;
		}

		public AudioBankLoadRequest(string bankName, string error)
			: this()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			this.bankName = bankName;
			this.error = new AssetManagerError(10, error);
			bundleName = string.Empty;
			isDone = true;
		}

		public AudioBankLoadRequest(string bankName, AssetReference assetReference, string bundleName, bool loadSamples)
			: this()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			this.bankName = bankName;
			this.bundleName = bundleName;
			this.loadSamples = loadSamples;
			m_assetReference = assetReference;
			m_bundleLoadRequest = AssetManager.LoadAssetBundle(bundleName);
			isDone = Update();
		}

		public void RequestSampleLoadingInternal()
		{
			loadSamples = true;
		}

		public bool UpdateInternal()
		{
			isDone = Update();
			return isDone;
		}

		private bool Update()
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Expected I4, but got Unknown
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0204: Expected I4, but got Unknown
			//IL_022d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_0236: Unknown result type (might be due to invalid IL or missing references)
			//IL_0292: Unknown result type (might be due to invalid IL or missing references)
			//IL_029d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
			if (m_bundleLoadRequest != null)
			{
				if (!m_bundleLoadRequest.get_isDone())
				{
					return false;
				}
				if (AssetManagerError.op_Implicit(m_bundleLoadRequest.get_error()) != 0)
				{
					error = m_bundleLoadRequest.get_error();
					return true;
				}
				m_bundleLoadRequest = null;
				m_assetLoadRequest = m_assetReference.LoadFromAssetBundleAsync<TextAsset>(bundleName);
			}
			if (m_assetLoadRequest != null)
			{
				if (!m_assetLoadRequest.get_isDone())
				{
					return false;
				}
				if (AssetManagerError.op_Implicit(m_assetLoadRequest.get_error()) != 0)
				{
					error = m_assetLoadRequest.get_error();
					return true;
				}
				TextAsset asset = m_assetLoadRequest.get_asset();
				m_assetLoadRequest = null;
				System studioSystem = AudioManager.studioSystem;
				Bank bank = default(Bank);
				studioSystem.loadBankMemoryPoint(asset.get_bytes(), 1, ref bank);
				this.bank = bank;
			}
			Bank bank2 = this.bank;
			LOADING_STATE val = default(LOADING_STATE);
			RESULT loadingState = bank2.getLoadingState(ref val);
			if ((int)loadingState != 0)
			{
				Log.Error($"Failed to retrieve loading state for bank named '{bankName}' loaded from bundle named '{bundleName}': {loadingState}.", 121, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioBankLoadRequest.cs");
				error = AssetManagerError.op_Implicit(30);
				bank2 = this.bank;
				bank2.unload();
				bank2 = this.bank;
				bank2.clearHandle();
				return true;
			}
			switch ((int)val)
			{
			case 0:
			case 1:
				Log.Error("Bank named '" + bankName + "' is being either unloaded or being unloaded instead of being loaded.", 132, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioBankLoadRequest.cs");
				error = AssetManagerError.op_Implicit(30);
				return true;
			case 2:
				return false;
			case 3:
				if (loadSamples)
				{
					if (!m_loadingSamples)
					{
						bank2 = this.bank;
						if ((int)bank2.loadSampleData() != 0)
						{
							Log.Warning("Could not load samples for bank named '" + bankName + "'.", 147, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioBankLoadRequest.cs");
							return true;
						}
						m_loadingSamples = true;
					}
					bank2 = this.bank;
					LOADING_STATE val2 = default(LOADING_STATE);
					if ((int)bank2.getSampleLoadingState(ref val2) == 0)
					{
						switch (val2 - 2)
						{
						case 0:
							return false;
						case 2:
							Log.Error("Failed to load samples for bank named '" + bankName + "'.", 165, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioBankLoadRequest.cs");
							bank2 = this.bank;
							bank2.unloadSampleData();
							break;
						}
					}
					else
					{
						Log.Warning("Could not get sample loading state for bank named '" + bankName + "'.", 172, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioBankLoadRequest.cs");
					}
					m_loadingSamples = false;
				}
				return true;
			case 4:
				Log.Error("Failed to load bank named '" + bankName + "'.", 180, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Audio\\AudioBankLoadRequest.cs");
				error = AssetManagerError.op_Implicit(30);
				bank2 = this.bank;
				bank2.unload();
				bank2 = this.bank;
				bank2.clearHandle();
				return true;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		public void Cancel()
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Invalid comparison between Unknown and I4
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Invalid comparison between Unknown and I4
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			if (m_bundleLoadRequest != null)
			{
				AssetManager.UnloadAssetBundle(bundleName);
				m_bundleLoadRequest = null;
			}
			else if (m_assetLoadRequest == null)
			{
				Bank bank = this.bank;
				LOADING_STATE val = default(LOADING_STATE);
				if ((int)bank.getLoadingState(ref val) == 0)
				{
					if (m_loadingSamples)
					{
						bank = this.bank;
						bank.unloadSampleData();
					}
					if ((int)val == 2 || (int)val == 3)
					{
						bank = this.bank;
						bank.unload();
						bank = this.bank;
						bank.clearHandle();
					}
				}
			}
			error = AssetManagerError.op_Implicit(50);
			isDone = true;
		}
	}
}
