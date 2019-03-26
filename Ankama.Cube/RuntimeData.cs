using Ankama.AssetManagement;
using Ankama.AssetManagement.AssetBundles;
using Ankama.AssetManagement.AssetReferences;
using Ankama.Cube.Data;
using Ankama.Cube.Data.UI;
using Ankama.Cube.Data.UI.Localization.TextFormatting;
using Ankama.Cube.Data.UI.Localization.TextFormatting.ValueProviders.FightValueProviders;
using Ankama.Utilities;
using DataEditor;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Ankama.Cube
{
	public static class RuntimeData
	{
		public delegate void CultureCodeChangedEventHandler(CultureCode cultureCode, FontLanguage fontLanguage);

		private struct DeserializationTask
		{
			private readonly EditableData[] m_assets;

			private readonly int m_startIndex;

			private readonly int m_endIndex;

			private readonly Type m_assetType;

			private Task m_task;

			private CancellationTokenSource m_cancellationTokenSource;

			private CancellationToken m_cancellationToken;

			public bool isFaultedOrCancelled
			{
				get
				{
					if (m_task != null && !m_task.IsFaulted)
					{
						return m_task.IsCanceled;
					}
					return true;
				}
			}

			public bool isCompleted
			{
				get
				{
					if (m_task != null)
					{
						return m_task.IsCompleted;
					}
					return true;
				}
			}

			public string errorMessage => "[RuntimeData] Failed to parse an asset of type " + m_assetType.Name + "'.\n" + $"Reason: {m_task.Exception?.InnerException}";

			public DeserializationTask(EditableData[] assets, int startIndex, int endIndex, Type assetType)
			{
				m_assets = assets;
				m_startIndex = startIndex;
				m_endIndex = endIndex;
				m_assetType = assetType;
				m_cancellationTokenSource = null;
				m_task = null;
			}

			public void Run()
			{
				m_cancellationTokenSource = new CancellationTokenSource();
				m_cancellationToken = m_cancellationTokenSource.Token;
				m_task = Task.Run((Action)Deserialize, m_cancellationToken);
			}

			public IEnumerator Cancel()
			{
				if (m_cancellationTokenSource != null && m_task != null)
				{
					m_cancellationTokenSource.Cancel();
					while (!m_task.IsCompleted)
					{
						yield return null;
					}
					m_cancellationTokenSource.Dispose();
					m_task.Dispose();
					m_cancellationTokenSource = null;
					m_task = null;
				}
			}

			public void Dispose()
			{
				if (m_cancellationTokenSource != null)
				{
					m_cancellationTokenSource.Dispose();
					m_cancellationTokenSource = null;
				}
				if (m_task != null)
				{
					m_task.Dispose();
					m_task = null;
				}
			}

			private void Deserialize()
			{
				EditableData[] assets = m_assets;
				int endIndex = m_endIndex;
				int num = m_startIndex;
				while (true)
				{
					if (num < endIndex)
					{
						if (m_cancellationToken.IsCancellationRequested)
						{
							break;
						}
						assets[num].LoadFromJson();
						num++;
						continue;
					}
					return;
				}
				m_cancellationToken.ThrowIfCancellationRequested();
			}
		}

		public static readonly Dictionary<int, FightDefinition> fightDefinitions;

		public static readonly Dictionary<int, WeaponDefinition> weaponDefinitions;

		public static readonly Dictionary<int, CompanionDefinition> companionDefinitions;

		public static readonly Dictionary<int, SummoningDefinition> summoningDefinitions;

		public static readonly Dictionary<int, FloorMechanismDefinition> floorMechanismDefinitions;

		public static readonly Dictionary<int, ObjectMechanismDefinition> objectMechanismDefinitions;

		public static readonly Dictionary<int, CharacterSkinDefinition> characterSkinDefinitions;

		public static readonly Dictionary<int, SpellDefinition> spellDefinitions;

		public static readonly Dictionary<int, SquadDefinition> squadDefinitions;

		public static readonly Dictionary<God, ReserveDefinition> reserveDefinitions;

		public static readonly Dictionary<God, GodDefinition> godDefinitions;

		public static DataAvailabilityDefinition availabilityDefinition;

		public static ConstantsDefinition constantsDefinition;

		private static bool s_setupCultureCodeVariant;

		private static bool s_loadedTextCollectionsBundle;

		private static LocalizedTextData s_localizedTextData;

		private static readonly Dictionary<string, int> s_textCollectionsRefCount;

		private static readonly Dictionary<int, string> s_textCollectionsData;

		private static readonly Dictionary<string, FontCollection> s_fontCollections;

		private static readonly Dictionary<string, FontCollection> s_fontCollectionsByName;

		private static bool s_loadedDataBundleName;

		private static readonly List<DeserializationTask> s_deserializationTasks;

		public static readonly TextFormatter textFormatter;

		private const int DeserializationBatchSize = 128;

		public static CultureCode currentCultureCode
		{
			get;
			private set;
		}

		public static FontLanguage currentFontLanguage
		{
			get;
			private set;
		}

		public static bool isReady
		{
			get;
			private set;
		}

		public static AssetManagerError error
		{
			get;
			private set;
		}

		public static KeywordContext currentKeywordContext
		{
			get;
			set;
		}

		public static event CultureCodeChangedEventHandler CultureCodeChanged;

		public static bool InitializeLanguage(CultureCode cultureCode)
		{
			FontLanguage fontLanguage = cultureCode.GetFontLanguage();
			currentCultureCode = CultureCode.FR_FR;
			currentFontLanguage = fontLanguage;
			textFormatter.pluralRules = cultureCode.GetPluralRules();
			s_localizedTextData = Resources.Load<LocalizedTextData>("Localization/LocalizedTextData");
			if (null == s_localizedTextData)
			{
				Log.Error("Failed to load localized text data from resources at path 'Localization/LocalizedTextData'.", 161, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				return false;
			}
			s_localizedTextData.Initialize();
			string text = $"Localization/{currentCultureCode}/Boot";
			TextCollection textCollection = Resources.Load<TextCollection>(text);
			if (null == textCollection)
			{
				Log.Error("Failed to load boot text collection from resources at path '" + text + "'.", 173, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				return false;
			}
			LoadTextCollectionData(textCollection);
			return true;
		}

		public static bool InitializeFonts()
		{
			FontCollection[] array = Resources.LoadAll<FontCollection>("GameData/UI/Fonts");
			if (array.Length == 0)
			{
				Log.Error("Could not find font collections.", 191, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				return false;
			}
			FontCollection[] array2 = array;
			foreach (FontCollection fontCollection in array2)
			{
				string identifier = fontCollection.identifier;
				string name = fontCollection.get_name();
				if (string.IsNullOrEmpty(identifier))
				{
					Log.Error("Font collection named '" + name + "' doesn't have an identifier.", 202, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
					return false;
				}
				if (!s_fontCollections.ContainsKey(identifier))
				{
					s_fontCollections.Add(identifier, fontCollection);
					if (!s_fontCollectionsByName.ContainsKey(name))
					{
						s_fontCollectionsByName.Add(name, fontCollection);
					}
					else
					{
						Log.Warning("Multiple font collections share the same name '" + name + "', subsequent assets will be ignored.", 222, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
					}
					continue;
				}
				Log.Error("Multiple font collections share the same identifier '" + identifier + "'.", 212, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				return false;
			}
			if (!s_fontCollectionsByName.TryGetValue("Default", out FontCollection value))
			{
				Log.Error("Could not load default font collection", 230, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				return false;
			}
			value.Load();
			return true;
		}

		public static IEnumerator Load()
		{
			if (isReady)
			{
				Log.Warning("Load called while runtime data is already ready.", 248, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				yield break;
			}
			Log.Info("Loading application text collection...", 252, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
			if (!AssetManager.AddActiveVariant(currentCultureCode.ToString().ToLowerInvariant()))
			{
				error = new AssetManagerError(10, $"Could not setup variant for culture code {currentCultureCode}.");
				Log.Error($"Error while loading text collections bundle: {error}", 261, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				yield break;
			}
			s_setupCultureCodeVariant = true;
			AssetBundleLoadRequest textCollectionsBundleLoadRequest = AssetManager.LoadAssetBundle("core/localization");
			while (!textCollectionsBundleLoadRequest.get_isDone())
			{
				yield return null;
			}
			if (AssetManagerError.op_Implicit(textCollectionsBundleLoadRequest.get_error()) != 0)
			{
				error = textCollectionsBundleLoadRequest.get_error();
				Log.Error($"Error while loading text collections bundle: {error}", 277, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				yield break;
			}
			s_loadedTextCollectionsBundle = true;
			yield return LoadTextCollectionAsync("Application");
			Log.Info("Loading data assets...", 285, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
			AssetBundleLoadRequest bundleLoadRequest = AssetManager.LoadAssetBundle("core/data");
			while (!bundleLoadRequest.get_isDone())
			{
				yield return null;
			}
			if (AssetManagerError.op_Implicit(bundleLoadRequest.get_error()) != 0)
			{
				Log.Error($"Error while loading data bundle: {error}", 297, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				error = bundleLoadRequest.get_error();
				yield break;
			}
			s_loadedDataBundleName = true;
			DataAvailability minValidAvailability = DataAvailability.Wip;
			yield return LoadDataDefinition(delegate(DataAvailabilityDefinition asset)
			{
				availabilityDefinition = asset;
			});
			if (AssetManagerError.op_Implicit(error) != 0)
			{
				Log.Error(string.Format("Error while loading {0}: {1}", "DataAvailabilityDefinition", error), 311, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				yield break;
			}
			yield return LoadDataDefinitions(fightDefinitions);
			if (AssetManagerError.op_Implicit(error) != 0)
			{
				Log.Error(string.Format("Error while loading {0}: {1}", "fightDefinitions", error), 321, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				yield break;
			}
			yield return LoadDataDefinitions(summoningDefinitions);
			if (AssetManagerError.op_Implicit(error) != 0)
			{
				Log.Error(string.Format("Error while loading {0}: {1}", "summoningDefinitions", error), 331, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				yield break;
			}
			HashSet<int> validCompanionIds = new HashSet<int>();
			foreach (CompanionAvailability companion in availabilityDefinition.companions)
			{
				if (companion.availability >= minValidAvailability)
				{
					validCompanionIds.Add(companion.companion.value);
				}
			}
			_003C_003Ec__DisplayClass49_0 @object;
			yield return LoadDataDefinitions(companionDefinitions, @object._003CLoad_003Eg__CompanionsPredicate_007C1);
			if (AssetManagerError.op_Implicit(error) != 0)
			{
				Log.Error(string.Format("Error while loading {0}: {1}", "companionDefinitions", error), 353, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				yield break;
			}
			yield return LoadDataDefinitions(weaponDefinitions);
			if (AssetManagerError.op_Implicit(error) != 0)
			{
				Log.Error(string.Format("Error while loading {0}: {1}", "weaponDefinitions", error), 363, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				yield break;
			}
			yield return LoadDataDefinitions(floorMechanismDefinitions);
			if (AssetManagerError.op_Implicit(error) != 0)
			{
				Log.Error(string.Format("Error while loading {0}: {1}", "floorMechanismDefinitions", error), 373, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				yield break;
			}
			yield return LoadDataDefinitions(objectMechanismDefinitions);
			if (AssetManagerError.op_Implicit(error) != 0)
			{
				Log.Error(string.Format("Error while loading {0}: {1}", "objectMechanismDefinitions", error), 383, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				yield break;
			}
			yield return LoadDataDefinitions(characterSkinDefinitions);
			if (AssetManagerError.op_Implicit(error) != 0)
			{
				Log.Error(string.Format("Error while loading {0}: {1}", "characterSkinDefinitions", error), 393, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				yield break;
			}
			Dictionary<int, ReserveDefinition> reserveDefinitionsById = new Dictionary<int, ReserveDefinition>();
			yield return LoadDataDefinitions(reserveDefinitionsById);
			if (AssetManagerError.op_Implicit(error) != 0)
			{
				Log.Error(string.Format("Error while loading {0}: {1}", "reserveDefinitions", error), 404, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				yield break;
			}
			Dictionary<int, GodDefinition> godDefinitionsById = new Dictionary<int, GodDefinition>();
			yield return LoadDataDefinitions(godDefinitionsById);
			if (AssetManagerError.op_Implicit(error) != 0)
			{
				Log.Error(string.Format("Error while loading {0}: {1}", "godDefinitions", error), 415, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				yield break;
			}
			yield return LoadDataDefinitions(spellDefinitions);
			if (AssetManagerError.op_Implicit(error) != 0)
			{
				Log.Error(string.Format("Error while loading {0}: {1}", "spellDefinitions", error), 425, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				yield break;
			}
			yield return LoadDataDefinitions(squadDefinitions);
			if (AssetManagerError.op_Implicit(error) != 0)
			{
				Log.Error(string.Format("Error while loading {0}: {1}", "squadDefinitions", error), 435, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				yield break;
			}
			yield return LoadDataDefinition(delegate(ConstantsDefinition asset)
			{
				constantsDefinition = asset;
			});
			if (AssetManagerError.op_Implicit(error) != 0)
			{
				Log.Error(string.Format("Error while loading {0}: {1}", "ConstantsDefinition", error), 445, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				yield break;
			}
			Log.Info("Loading text collections...", 451, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
			yield return EnumeratorUtility.ParallelSafeExecution(new IEnumerator[8]
			{
				LoadTextCollectionAsync("Effects"),
				LoadTextCollectionAsync("Companions"),
				LoadTextCollectionAsync("Mechanisms"),
				LoadTextCollectionAsync("Spells"),
				LoadTextCollectionAsync("Gods"),
				LoadTextCollectionAsync("Summonings"),
				LoadTextCollectionAsync("Weapons"),
				LoadTextCollectionAsync("UI")
			});
			Log.Info("Waiting for deserialization tasks completion...", 465, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
			List<DeserializationTask> deserializationTasks = s_deserializationTasks;
			int deserializationTaskCount = deserializationTasks.Count;
			while (true)
			{
				bool flag = false;
				for (int i = 0; i < deserializationTaskCount; i++)
				{
					DeserializationTask deserializationTask = deserializationTasks[i];
					if (deserializationTask.isFaultedOrCancelled)
					{
						error = new AssetManagerError(10, deserializationTask.errorMessage);
						Log.Error((object)error, 481, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
						yield break;
					}
					if (!deserializationTask.isCompleted)
					{
						flag = true;
					}
				}
				if (!flag)
				{
					break;
				}
				yield return null;
			}
			for (int j = 0; j < deserializationTaskCount; j++)
			{
				deserializationTasks[j].Dispose();
			}
			deserializationTasks.Clear();
			foreach (ReserveDefinition value in reserveDefinitionsById.Values)
			{
				reserveDefinitions.Add(value.god, value);
			}
			foreach (GodDefinition value2 in godDefinitionsById.Values)
			{
				godDefinitions.Add(value2.god, value2);
			}
			isReady = true;
			Log.Info($"Loaded {characterSkinDefinitions.Count} character skins definitions.", 524, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
			Log.Info($"Loaded {companionDefinitions.Count} companion definitions.", 525, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
			Log.Info($"Loaded {floorMechanismDefinitions.Count} floor mechanism definitions.", 526, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
			Log.Info($"Loaded {godDefinitionsById.Count} god definitions.", 527, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
			Log.Info($"Loaded {weaponDefinitions.Count} hero definitions.", 528, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
			Log.Info($"Loaded {objectMechanismDefinitions.Count} object mechanism definitions.", 529, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
			Log.Info($"Loaded {squadDefinitions.Count} squad definitions.", 530, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
			Log.Info($"Loaded {spellDefinitions.Count} spell definitions.", 531, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
			Log.Info($"Loaded {summoningDefinitions.Count} summoning definitions.", 532, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
			Log.Info($"Loaded {reserveDefinitionsById.Count} reserve definitions.", 533, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
		}

		public static IEnumerator LoadOffline()
		{
			if (isReady)
			{
				Log.Warning("LoadOffline called while runtime data is already ready.", 546, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				yield break;
			}
			if (!AssetManager.AddActiveVariant(currentCultureCode.ToString().ToLowerInvariant()))
			{
				error = new AssetManagerError(10, $"Could not setup variant for culture code {currentCultureCode}.");
				Log.Error($"Error while loading text collections bundle: {error}", 557, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				yield break;
			}
			s_setupCultureCodeVariant = true;
			AssetBundleLoadRequest textCollectionsBundleLoadRequest = AssetManager.LoadAssetBundle("core/localization");
			while (!textCollectionsBundleLoadRequest.get_isDone())
			{
				yield return null;
			}
			if (AssetManagerError.op_Implicit(textCollectionsBundleLoadRequest.get_error()) != 0)
			{
				error = textCollectionsBundleLoadRequest.get_error();
				Log.Error($"Error while loading text collections bundle: {error}", 573, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
			}
			else
			{
				s_loadedTextCollectionsBundle = true;
				yield return LoadTextCollectionAsync("Application");
			}
		}

		public static IEnumerator Unload()
		{
			int deserializationTaskCount = s_deserializationTasks.Count;
			int num;
			for (int i = 0; i < deserializationTaskCount; i = num)
			{
				yield return s_deserializationTasks[i].Cancel();
				num = i + 1;
			}
			s_deserializationTasks.Clear();
			if (s_loadedDataBundleName)
			{
				AssetBundleUnloadRequest unloadRequest2 = AssetManager.UnloadAssetBundle("core/data");
				while (!unloadRequest2.get_isDone())
				{
					yield return null;
				}
				s_loadedDataBundleName = false;
			}
			if (s_setupCultureCodeVariant)
			{
				string text = currentCultureCode.ToString().ToLowerInvariant();
				if (!AssetManager.RemoveActiveVariant(text))
				{
					Log.Warning("Error while trying to unload runtime data, could not remove variant: " + text + ".", 617, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				}
				s_setupCultureCodeVariant = false;
			}
			if (s_loadedTextCollectionsBundle)
			{
				foreach (string key in s_textCollectionsRefCount.Keys)
				{
					if (!s_localizedTextData.TryGetTextCollectionReference(key, out AssetReference textCollectionReference))
					{
						Log.Error("Could not load text collection asset named '" + key + "' because it doesn't exist in the localized text data.", 633, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
					}
					else
					{
						TextCollection textCollection = textCollectionReference.LoadFromAssetBundle<TextCollection>("core/localization");
						if (null == textCollection)
						{
							Log.Error("Could not unload text collection asset named '" + key + "'.", 643, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
						}
						else
						{
							UnloadTextCollectionData(textCollection);
						}
					}
				}
				s_textCollectionsRefCount.Clear();
				AssetBundleUnloadRequest unloadRequest2 = AssetManager.UnloadAssetBundle("core/localization");
				while (!unloadRequest2.get_isDone())
				{
					yield return null;
				}
				s_loadedTextCollectionsBundle = false;
			}
			fightDefinitions.Clear();
			weaponDefinitions.Clear();
			companionDefinitions.Clear();
			summoningDefinitions.Clear();
			floorMechanismDefinitions.Clear();
			objectMechanismDefinitions.Clear();
			spellDefinitions.Clear();
			squadDefinitions.Clear();
			reserveDefinitions.Clear();
			godDefinitions.Clear();
			error = AssetManagerError.op_Implicit(0);
			isReady = false;
		}

		public static void Release()
		{
			if (s_fontCollectionsByName != null && s_fontCollectionsByName.TryGetValue("Default", out FontCollection value))
			{
				value.Unload();
			}
		}

		public static IEnumerator ChangeLanguage(CultureCode cultureCode)
		{
			if (cultureCode == currentCultureCode)
			{
				yield break;
			}
			AssetBundleUnloadRequest unloadRequest = AssetManager.UnloadAssetBundle("core/localization");
			while (!unloadRequest.get_isDone())
			{
				yield return null;
			}
			if (AssetManagerError.op_Implicit(unloadRequest.get_error()) != 0)
			{
				Log.Error($"Failed to change language from {currentCultureCode} to {cultureCode} because the asset bundle could not be unloaded.", 716, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				yield break;
			}
			if (!AssetManager.RemoveActiveVariant(currentCultureCode.ToString().ToLowerInvariant()))
			{
				Log.Error($"Failed to change language from {currentCultureCode} to {cultureCode} because the asset bundle variant could not be unset.", 726, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				yield break;
			}
			if (!AssetManager.AddActiveVariant(cultureCode.ToString().ToLowerInvariant()))
			{
				Log.Error($"Failed to change language from {currentCultureCode} to {cultureCode} because the asset bundle variant could not be set.", 736, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				yield break;
			}
			AssetBundleLoadRequest loadRequest = AssetManager.LoadAssetBundle("core/localization");
			while (!loadRequest.get_isDone())
			{
				yield return null;
			}
			if (AssetManagerError.op_Implicit(loadRequest.get_error()) != 0)
			{
				Log.Error($"Failed to change language from {currentCultureCode} to {cultureCode} because the asset bundle could not be loaded.", 751, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				yield break;
			}
			foreach (string key in s_textCollectionsRefCount.Keys)
			{
				if (!s_localizedTextData.TryGetTextCollectionReference(key, out AssetReference textCollectionReference))
				{
					Log.Error("Could not load text collection asset named '" + key + "' because it doesn't exist in the localized text data.", 763, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				}
				else
				{
					TextCollection textCollection = textCollectionReference.LoadFromAssetBundle<TextCollection>("core/localization");
					if (null == textCollection)
					{
						Log.Error("Could not load text collection asset named '" + key + "'.", 773, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
					}
					else
					{
						textCollection.FeedDictionary(s_textCollectionsData);
					}
				}
			}
			SetCultureCode(cultureCode);
		}

		public static void SetCultureCode(CultureCode cultureCode)
		{
			FontLanguage fontLanguage = cultureCode.GetFontLanguage();
			currentCultureCode = cultureCode;
			currentFontLanguage = fontLanguage;
			textFormatter.pluralRules = cultureCode.GetPluralRules();
			RuntimeData.CultureCodeChanged?.Invoke(cultureCode, fontLanguage);
		}

		public static IEnumerator LoadFontCollection([NotNull] string fontCollectionName)
		{
			if (!s_fontCollectionsByName.TryGetValue(fontCollectionName, out FontCollection value))
			{
				Log.Error("Could not find a font collection named '" + fontCollectionName + "'.", 801, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
			}
			else
			{
				yield return value.LoadAsync();
			}
		}

		public static void UnloadFontCollection([NotNull] string fontCollectionName)
		{
			if (!s_fontCollectionsByName.TryGetValue(fontCollectionName, out FontCollection value))
			{
				Log.Error("Could not find a font collection named '" + fontCollectionName + "'.", 815, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
			}
			else
			{
				value.Unload();
			}
		}

		public static bool LoadTextCollection([NotNull] string textCollectionName)
		{
			if (s_textCollectionsRefCount.TryGetValue(textCollectionName, out int value))
			{
				value++;
				s_textCollectionsRefCount[textCollectionName] = value;
				return true;
			}
			if (!s_localizedTextData.TryGetTextCollectionReference(textCollectionName, out AssetReference textCollectionReference))
			{
				Log.Error("Could not load text collection asset named '" + textCollectionName + "' because it doesn't exist in the localized text data.", 837, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				return false;
			}
			TextCollection textCollection = textCollectionReference.LoadFromAssetBundle<TextCollection>("core/localization");
			if (null == textCollection)
			{
				Log.Error("Could not load text collection asset named '" + textCollectionName + "'.", 847, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				return false;
			}
			LoadTextCollectionData(textCollection);
			s_textCollectionsRefCount.Add(textCollectionName, 1);
			return true;
		}

		public static bool UnloadTextCollection(string textCollectionName)
		{
			if (!s_textCollectionsRefCount.TryGetValue(textCollectionName, out int value))
			{
				Log.Warning("Tried to unload text collection named '" + textCollectionName + "' but it was not loaded.", 864, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				return false;
			}
			value--;
			if (value == 0)
			{
				s_textCollectionsRefCount.Remove(textCollectionName);
			}
			else
			{
				s_textCollectionsRefCount[textCollectionName] = value;
			}
			if (!s_localizedTextData.TryGetTextCollectionReference(textCollectionName, out AssetReference textCollectionReference))
			{
				Log.Error("Could not load text collection asset named '" + textCollectionName + "' because it doesn't exist in the localized text data.", 881, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				return false;
			}
			TextCollection textCollection = textCollectionReference.LoadFromAssetBundle<TextCollection>("core/localization");
			if (null == textCollection)
			{
				Log.Error("Could not unload text collection asset named '" + textCollectionName + "'.", 891, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				return false;
			}
			UnloadTextCollectionData(textCollection);
			return true;
		}

		public static IEnumerator LoadTextCollectionAsync(string textCollectionName)
		{
			if (s_textCollectionsRefCount.TryGetValue(textCollectionName, out int value))
			{
				value++;
				s_textCollectionsRefCount[textCollectionName] = value;
				yield break;
			}
			if (!s_localizedTextData.TryGetTextCollectionReference(textCollectionName, out AssetReference textCollectionReference))
			{
				Log.Error("Could not load text collection asset named '" + textCollectionName + "' because it doesn't exist in the localized text data.", 914, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				yield break;
			}
			AssetLoadRequest<TextCollection> loadRequest = textCollectionReference.LoadFromAssetBundleAsync<TextCollection>("core/localization");
			while (!loadRequest.get_isDone())
			{
				yield return null;
			}
			if (AssetManagerError.op_Implicit(loadRequest.get_error()) != 0)
			{
				Log.Error($"Could not load text collection asset named '{textCollectionName}': {loadRequest.get_error()}.", 929, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				yield break;
			}
			TextCollection asset = loadRequest.get_asset();
			if (null == asset)
			{
				Log.Error("Found text collection asset named '" + textCollectionName + "' but it is invalid.", 936, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				yield break;
			}
			LoadTextCollectionData(asset);
			s_textCollectionsRefCount.Add(textCollectionName, 1);
		}

		public static IEnumerator UnloadTextCollectionAsync(string textCollectionName)
		{
			if (!s_textCollectionsRefCount.TryGetValue(textCollectionName, out int value))
			{
				Log.Warning("Tried to unload text collection named '" + textCollectionName + "' but it was not loaded.", 952, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				yield break;
			}
			value--;
			if (value == 0)
			{
				s_textCollectionsRefCount.Remove(textCollectionName);
			}
			else
			{
				s_textCollectionsRefCount[textCollectionName] = value;
			}
			if (!s_localizedTextData.TryGetTextCollectionReference(textCollectionName, out AssetReference textCollectionReference))
			{
				Log.Error("Could not unload text collection asset named '" + textCollectionName + "' because it doesn't exist in the localized text data.", 969, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				yield break;
			}
			AssetLoadRequest<TextCollection> unloadRequest = textCollectionReference.LoadFromAssetBundleAsync<TextCollection>("core/localization");
			while (!unloadRequest.get_isDone())
			{
				yield return null;
			}
			if (AssetManagerError.op_Implicit(unloadRequest.get_error()) != 0)
			{
				Log.Error($"Could not unload text collection asset named '{textCollectionName}': {unloadRequest.get_error()}.", 984, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				yield break;
			}
			TextCollection asset = unloadRequest.get_asset();
			if (null == asset)
			{
				Log.Error("Found text collection asset named '" + textCollectionName + "' but it is invalid.", 992, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
			}
			else
			{
				UnloadTextCollectionData(asset);
			}
		}

		public static bool TryGetFontCollection(string identifier, out FontCollection fontCollection)
		{
			return s_fontCollections.TryGetValue(identifier, out fontCollection);
		}

		public static bool TryGetTextKeyId([NotNull] string name, out int id)
		{
			return s_localizedTextData.TryGetKeyId(name, out id);
		}

		public static string FormattedText(int textKeyId, params string[] args)
		{
			if (args == null || args.Length == 0)
			{
				return FormattedText(textKeyId);
			}
			return FormattedText(textKeyId, new IndexedValueProvider(args));
		}

		public static string FormattedText(string name, params string[] args)
		{
			if (args == null || args.Length == 0)
			{
				return FormattedText(name);
			}
			return FormattedText(name, new IndexedValueProvider(args));
		}

		public static string FormattedText(string name, IValueProvider valueProvider = null)
		{
			if (!TryGetText(name, out string value))
			{
				Log.Error("Text key with name " + name + " does not exist.", 1047, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				return string.Empty;
			}
			try
			{
				FormatterParams formatterParams = new FormatterParams(textFormatter, valueProvider);
				formatterParams.context = currentKeywordContext;
				FormatterParams formatterParams2 = formatterParams;
				return textFormatter.Format(value, formatterParams2);
			}
			catch (Exception ex)
			{
				Log.Error($"Text value '{value}' could not be formatted with specified {valueProvider} params: {ex.Message}", 1061, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				return value;
			}
		}

		public static string FormattedText(int textKeyId, IValueProvider valueProvider = null)
		{
			if (!TryGetText(textKeyId, out string value))
			{
				Log.Error($"Text key with id {textKeyId} does not exist.", 1072, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				return string.Empty;
			}
			try
			{
				FormatterParams formatterParams = new FormatterParams(textFormatter, valueProvider);
				formatterParams.context = currentKeywordContext;
				FormatterParams formatterParams2 = formatterParams;
				return textFormatter.Format(value, formatterParams2);
			}
			catch (Exception ex)
			{
				Log.Error($"Text value '{value}' could not be formatted with specified {valueProvider} params: {ex.Message}", 1086, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				return value;
			}
		}

		public static bool TryGetText(int id, [NotNull] out string value)
		{
			return s_textCollectionsData.TryGetValue(id, out value);
		}

		public static bool TryGetText([NotNull] string name, [NotNull] out string value)
		{
			if (!s_localizedTextData.TryGetKeyId(name, out int id))
			{
				Log.Warning("Could not found a text key named '" + name + "'.", 1140, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
				value = string.Empty;
				return false;
			}
			return s_textCollectionsData.TryGetValue(id, out value);
		}

		private static IEnumerator LoadDataDefinition<T>([NotNull] Action<T> assignationCallback) where T : EditableData
		{
			AllAssetsLoadRequest<T> assetLoadRequest = AssetManager.LoadAllAssetsAsync<T>("core/data");
			while (!assetLoadRequest.get_isDone())
			{
				yield return null;
			}
			if (AssetManagerError.op_Implicit(assetLoadRequest.get_error()) != 0)
			{
				error = assetLoadRequest.get_error();
				yield break;
			}
			T[] assets = assetLoadRequest.get_assets();
			int num = assets.Length;
			if (num == 0)
			{
				error = new AssetManagerError(10, "[RuntimeData] Could not find any data definition of type T.");
				yield break;
			}
			assignationCallback(assets[0]);
			if (num > 1)
			{
				Log.Warning("Data definition of type T is loaded using LoadDataDefinition but multiple assets were found.", 1283, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
			}
			EditableData[] assets2 = (EditableData[])assets;
			DeserializationTask item = new DeserializationTask(assets2, 0, num, typeof(T));
			item.Run();
			s_deserializationTasks.Add(item);
		}

		private static IEnumerator LoadDataDefinitions<T>(Dictionary<int, T> dictionary, Predicate<int> filter = null) where T : EditableData
		{
			AllAssetsLoadRequest<T> assetLoadRequest = AssetManager.LoadAllAssetsAsync<T>("core/data");
			while (!assetLoadRequest.get_isDone())
			{
				yield return null;
			}
			if (AssetManagerError.op_Implicit(assetLoadRequest.get_error()) != 0)
			{
				error = assetLoadRequest.get_error();
				yield break;
			}
			T[] assets = assetLoadRequest.get_assets();
			T[] array;
			if (filter == null)
			{
				array = assets;
			}
			else
			{
				int num = assets.Length;
				List<T> list = new List<T>();
				for (int i = 0; i < num; i++)
				{
					T val = assets[i];
					if (int.TryParse(val.get_name(), out int result) && filter(result))
					{
						list.Add(val);
					}
				}
				array = list.ToArray();
			}
			int num2 = array.Length;
			for (int j = 0; j < num2; j++)
			{
				T val2 = array[j];
				if (!int.TryParse(val2.get_name(), out int result2))
				{
					error = new AssetManagerError(10, "[RuntimeData] ID of asset of type " + typeof(T).Name + " and named '" + val2.get_name() + "' cannot be inferred.");
					dictionary.Clear();
					yield break;
				}
				if (dictionary.ContainsKey(result2))
				{
					error = new AssetManagerError(10, string.Format("[RuntimeData] Duplicate asset of type {0} with id {1} from bundle named '{2}'.", typeof(T).Name, result2, "core/data"));
					dictionary.Clear();
					yield break;
				}
				dictionary.Add(result2, val2);
			}
			DeserializationTask item = default(DeserializationTask);
			for (int k = 0; k < num2; k += 128)
			{
				int num3 = k;
				int endIndex = Math.Min(num3 + 128, num2);
				EditableData[] assets2 = (EditableData[])array;
				item = new DeserializationTask(assets2, num3, endIndex, typeof(T));
				item.Run();
				s_deserializationTasks.Add(item);
			}
		}

		private static void LoadTextCollectionData([NotNull] TextCollection textCollection)
		{
			textCollection.FeedDictionary(s_textCollectionsData);
			Log.Info($"Text collection data now has {s_textCollectionsData.Count} entries after loading {textCollection.get_name()}.", 1384, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
		}

		private static void UnloadTextCollectionData([NotNull] TextCollection textCollection)
		{
			textCollection.StarveDictionary(s_textCollectionsData);
			Log.Info($"Text collection data now has {s_textCollectionsData.Count} entries after unloading {textCollection.get_name()}.", 1392, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\RuntimeData.cs");
		}

		public static bool IsPlayable(God god)
		{
			IReadOnlyList<GodAvailability> gods = availabilityDefinition.gods;
			int count = gods.Count;
			for (int i = 0; i < count; i++)
			{
				GodAvailability godAvailability = gods[i];
				if (godAvailability.god == god)
				{
					return godAvailability.availability != DataAvailability.NotUsed;
				}
			}
			return false;
		}

		static RuntimeData()
		{
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			fightDefinitions = new Dictionary<int, FightDefinition>();
			weaponDefinitions = new Dictionary<int, WeaponDefinition>();
			companionDefinitions = new Dictionary<int, CompanionDefinition>();
			summoningDefinitions = new Dictionary<int, SummoningDefinition>();
			floorMechanismDefinitions = new Dictionary<int, FloorMechanismDefinition>();
			objectMechanismDefinitions = new Dictionary<int, ObjectMechanismDefinition>();
			characterSkinDefinitions = new Dictionary<int, CharacterSkinDefinition>();
			spellDefinitions = new Dictionary<int, SpellDefinition>();
			squadDefinitions = new Dictionary<int, SquadDefinition>();
			reserveDefinitions = new Dictionary<God, ReserveDefinition>(GodComparer.instance);
			godDefinitions = new Dictionary<God, GodDefinition>(GodComparer.instance);
			currentCultureCode = CultureCode.Default;
			currentFontLanguage = FontLanguage.Latin;
			error = AssetManagerError.op_Implicit(0);
			s_textCollectionsRefCount = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
			s_textCollectionsData = new Dictionary<int, string>();
			s_fontCollections = new Dictionary<string, FontCollection>(StringComparer.Ordinal);
			s_fontCollectionsByName = new Dictionary<string, FontCollection>(StringComparer.OrdinalIgnoreCase);
			s_deserializationTasks = new List<DeserializationTask>(16);
			textFormatter = new TextFormatter(new IParserRule[4]
			{
				new LineBreakParserRule(),
				new ParserRulesGroupStartWith("\\", new EscapedBraceParserRule(), new VertivalTabParserRule(), new NonBreakableSpaceParserRule()),
				new ConditionalParserRule(),
				new BoundedParserRule('{', '}', new KeywordParserRule(), new WordSubstitutionParserRule(), new ParserRulesGroupByFirstLetter(new RangeParserRule(), new DynamicValueParserRule(), new ObjectReferenceParserRule("spell", ObjectReference.Type.Spell), new ObjectReferenceParserRule("companion", ObjectReference.Type.Companion), new ObjectReferenceParserRule("summoning", ObjectReference.Type.Summoning), new ObjectReferenceParserRule("floorMechanism", ObjectReference.Type.FloorMechanism), new ObjectReferenceParserRule("objectMechanism", ObjectReference.Type.ObjectMechanism), new ObjectReferenceParserRule("weapon", ObjectReference.Type.Weapon), new EffectValueParserRule("damage")
				{
					getModificationValue = ((IFightValueProvider valueProvider) => valueProvider.GetDamageModifierValue())
				}, new EffectValueParserRule("explosion"), new EffectValueParserRule("herd"), new EffectValueParserRule("clan"), new EffectValueParserRule("heal")
				{
					getModificationValue = ((IFightValueProvider valueProvider) => valueProvider.GetHealModifierValue())
				}, new EffectValueParserRule("blindage"), new EffectValueParserRule("cell"), new EffectValueParserRule("armor"), new EffectValueParserRule("maxLifeGain"), new EffectValueParserRule("frozen"), new ParserRulesGroupStartWith("add", new EffectValueParserRule("addEarth"), new EffectValueParserRule("addFire"), new EffectValueParserRule("addAir"), new EffectValueParserRule("addWater"), new EffectValueParserRule("addReserve"))), new ValueParserRule(), new PluralParserRule(), new SelectParserRule())
			});
			currentKeywordContext = KeywordContext.FightSolo;
		}
	}
}
