using Ankama.AssetManagement.AssetReferences;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class ReserveDefinition : EditableData, IDefinitionWithTooltip, IDefinitionWithDescription, IDefinitionWithPrecomputedData
	{
		private God m_god;

		[LocalizedString("RESERVE_{id}_Name", "Effects", 1)]
		[SerializeField]
		private int m_i18nNameId;

		[LocalizedString("RESERVE_{id}_DESCRIPTION", "Effects", 3)]
		[SerializeField]
		private int m_i18nDescriptionId;

		private PrecomputedData m_precomputedData;

		[SerializeField]
		private AssetReference m_illustrationReference;

		[SerializeField]
		private AssetReference m_tooltipIllustrationReference;

		[SerializeField]
		private string m_bundleName;

		public God god => m_god;

		public int i18nNameId => m_i18nNameId;

		public int i18nDescriptionId => m_i18nDescriptionId;

		public PrecomputedData precomputedData => m_precomputedData;

		public AssetReference illustrationReference => m_illustrationReference;

		public AssetReference tooltipIllustrationReference => m_tooltipIllustrationReference;

		public string bundleName => m_bundleName;

		public string illustrationBundleName => "core/ui/spells";

		public override void PopulateFromJson(JObject jsonObject)
		{
			this.PopulateFromJson(jsonObject);
			m_god = (God)Serialization.JsonTokenValue<int>(jsonObject, "god", 0);
			m_precomputedData = PrecomputedData.FromJsonProperty(jsonObject, "precomputedData");
		}

		public ReserveDefinition()
			: this()
		{
		}
	}
}
