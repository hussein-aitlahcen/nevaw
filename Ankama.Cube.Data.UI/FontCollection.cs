using Ankama.AssetManagement.AssetReferences;
using Ankama.Cube.UI.Components;
using Ankama.Utilities;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Ankama.Cube.Data.UI
{
	[CreateAssetMenu(fileName = "New Font Collection", menuName = "Waven/Data/UI/Font Collection")]
	public class FontCollection : ScriptableObject
	{
		[Serializable]
		private class FontDataDictionary : SerializableDictionary<FontLanguage, FontData>
		{
			public FontDataDictionary()
				: base((IEqualityComparer<FontLanguage>)FontLanguageComparer.instance)
			{
			}
		}

		public const string ResourceFontCollectionFolder = "GameData/UI/Fonts";

		public const string DefaultFontCollectionName = "Default";

		[UsedImplicitly]
		[HideInInspector]
		[SerializeField]
		private string m_guid;

		[UsedImplicitly]
		[SerializeField]
		private FontDataDictionary m_data = new FontDataDictionary();

		private readonly List<AbstractTextField> m_registeredTextFields = new List<AbstractTextField>();

		[NonSerialized]
		private int m_referenceCount;

		[NonSerialized]
		private FontData m_fontData;

		[NonSerialized]
		private TMP_FontAsset m_fontAsset;

		[NonSerialized]
		private Material m_styleMaterial;

		[NotNull]
		public string identifier => m_guid;

		[NotNull]
		public TMP_FontAsset fontAsset
		{
			get
			{
				if (m_referenceCount == 0)
				{
					Log.Error("Font collection named '" + this.get_name() + "' has not been loaded before use.", 157, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\UI\\FontCollection.cs");
					return TMP_FontAsset.get_defaultFontAsset();
				}
				return m_fontAsset;
			}
		}

		[CanBeNull]
		public Material styleMaterial
		{
			get
			{
				if (m_referenceCount == 0)
				{
					Log.Error("Font collection named '" + this.get_name() + "' has not been loaded before use.", 204, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\UI\\FontCollection.cs");
					return null;
				}
				return m_styleMaterial;
			}
		}

		[CanBeNull]
		public FontData fontData
		{
			get
			{
				if (m_referenceCount == 0)
				{
					Log.Error("Font collection named '" + this.get_name() + "' has not been loaded before use.", 233, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\UI\\FontCollection.cs");
					return null;
				}
				return m_fontData;
			}
		}

		public void Load()
		{
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			if (m_referenceCount > 0)
			{
				m_referenceCount++;
				return;
			}
			FontLanguage currentFontLanguage = RuntimeData.currentFontLanguage;
			if (!((Dictionary<FontLanguage, FontData>)m_data).TryGetValue(currentFontLanguage, out FontData value))
			{
				Log.Error($"Font collection named '{this.get_name()}' doesn't have any font data for font language '{currentFontLanguage}'.", 256, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\UI\\FontCollection.cs");
				return;
			}
			AssetReference fontAssetReference = value.fontAssetReference;
			if (!fontAssetReference.get_hasValue())
			{
				Log.Error($"Font collection named '{this.get_name()}' doesn't have a font setup for font language '{currentFontLanguage}'.", 263, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\UI\\FontCollection.cs");
				return;
			}
			TMP_FontAsset val = fontAssetReference.LoadFromResources<TMP_FontAsset>();
			if (null == val)
			{
				Log.Error($"Font collection named '{this.get_name()}' failed to load font for font language '{currentFontLanguage}'.", 270, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\UI\\FontCollection.cs");
				return;
			}
			AssetReference styleMaterialReference = value.styleMaterialReference;
			Material styleMaterial = styleMaterialReference.get_hasValue() ? styleMaterialReference.LoadFromResources<Material>() : null;
			RuntimeData.CultureCodeChanged += CultureCodeChanged;
			m_fontData = value;
			m_fontAsset = val;
			m_styleMaterial = styleMaterial;
			m_referenceCount = 1;
		}

		public IEnumerator LoadAsync()
		{
			if (m_referenceCount > 0)
			{
				m_referenceCount++;
				yield break;
			}
			FontLanguage fontLanguage = RuntimeData.currentFontLanguage;
			if (!((Dictionary<FontLanguage, FontData>)m_data).TryGetValue(fontLanguage, out FontData currentFontData))
			{
				Log.Error($"Font collection named '{this.get_name()}' doesn't have any font data for font language '{fontLanguage}'.", 300, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\UI\\FontCollection.cs");
				yield break;
			}
			AssetReference fontAssetReference = currentFontData.fontAssetReference;
			if (!fontAssetReference.get_hasValue())
			{
				Log.Error($"Font collection named '{this.get_name()}' doesn't have a font setup for font language '{fontLanguage}'.", 307, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\UI\\FontCollection.cs");
				yield break;
			}
			AssetReferenceRequest<TMP_FontAsset> loadRequest = fontAssetReference.LoadFromResourcesAsync<TMP_FontAsset>();
			while (!loadRequest.get_isDone())
			{
				yield return null;
			}
			TMP_FontAsset asset = loadRequest.get_asset();
			if (null == asset)
			{
				Log.Error($"Font collection named '{this.get_name()}' failed to load font for font language '{fontLanguage}'.", 320, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\UI\\FontCollection.cs");
				yield break;
			}
			AssetReference styleMaterialReference = currentFontData.styleMaterialReference;
			Material val;
			if (styleMaterialReference.get_hasValue())
			{
				AssetReferenceRequest<Material> styleLoadRequest = styleMaterialReference.LoadFromResourcesAsync<Material>();
				while (!styleLoadRequest.get_isDone())
				{
					yield return null;
				}
				val = styleLoadRequest.get_asset();
				if (null == val)
				{
					Log.Warning($"Font collection named '{this.get_name()}' failed to load style material for font language '{fontLanguage}'.", 337, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\UI\\FontCollection.cs");
				}
			}
			else
			{
				val = null;
			}
			RuntimeData.CultureCodeChanged += CultureCodeChanged;
			m_fontData = currentFontData;
			m_fontAsset = asset;
			m_styleMaterial = val;
			m_referenceCount = 1;
		}

		public void Unload()
		{
			if (m_referenceCount == 0)
			{
				Log.Error("Tried to unload font collection named '" + this.get_name() + "' but it is not loaded.", 359, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\UI\\FontCollection.cs");
				return;
			}
			m_referenceCount--;
			if (m_referenceCount <= 0)
			{
				RuntimeData.CultureCodeChanged -= CultureCodeChanged;
				m_styleMaterial = null;
				m_fontAsset = null;
				m_fontData = null;
			}
		}

		public void RegisterTextField([NotNull] AbstractTextField textField)
		{
			if (null == textField)
			{
				throw new ArgumentNullException("textField");
			}
			m_registeredTextFields.Add(textField);
		}

		public void UnregisterTextField(AbstractTextField textField)
		{
			if (null == textField)
			{
				throw new ArgumentNullException("textField");
			}
			m_registeredTextFields.Remove(textField);
		}

		private void CultureCodeChanged(CultureCode cultureCode, FontLanguage fontLanguage)
		{
			if (m_referenceCount != 0)
			{
				ChangeFontLanguage(fontLanguage);
				foreach (AbstractTextField registeredTextField in m_registeredTextFields)
				{
					if (null == registeredTextField)
					{
						Log.Warning("Found a registered null TextField instance.", 429, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\UI\\FontCollection.cs");
					}
					else
					{
						registeredTextField.RefreshText();
					}
				}
			}
		}

		private void ChangeFontLanguage(FontLanguage fontLanguage)
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			if (!((Dictionary<FontLanguage, FontData>)m_data).TryGetValue(fontLanguage, out FontData value))
			{
				Log.Error($"Font collection named '{this.get_name()}' doesn't have any font data for font language '{fontLanguage}'.", 444, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\UI\\FontCollection.cs");
				return;
			}
			AssetReference fontAssetReference = value.fontAssetReference;
			if (!fontAssetReference.get_hasValue())
			{
				Log.Error($"Font collection named '{this.get_name()}' doesn't have a font setup for font language '{fontLanguage}'.", 451, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\UI\\FontCollection.cs");
				return;
			}
			TMP_FontAsset val = fontAssetReference.LoadFromResources<TMP_FontAsset>();
			if (null == val)
			{
				Log.Error($"Font collection named '{this.get_name()}' failed to load font for font language '{fontLanguage}'.", 458, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Data\\UI\\FontCollection.cs");
				return;
			}
			AssetReference styleMaterialReference = value.styleMaterialReference;
			Material styleMaterial = styleMaterialReference.get_hasValue() ? styleMaterialReference.LoadFromResources<Material>() : null;
			if (null != m_styleMaterial)
			{
				Resources.UnloadAsset(m_styleMaterial);
			}
			if (null != m_fontAsset)
			{
				Resources.UnloadAsset(m_fontAsset);
			}
			m_fontData = value;
			m_fontAsset = val;
			m_styleMaterial = styleMaterial;
		}

		public FontCollection()
			: this()
		{
		}
	}
}
