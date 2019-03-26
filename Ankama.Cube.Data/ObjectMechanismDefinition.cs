using Ankama.AssetManagement.AssetReferences;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class ObjectMechanismDefinition : MechanismDefinition, IDefinitionWithTooltip, IDefinitionWithDescription, IDefinitionWithPrecomputedData
	{
		[LocalizedString("OBJECT_MECHANISM_{id}_NAME", "Mechanisms", 1)]
		[SerializeField]
		private int m_i18nNameId;

		[LocalizedString("OBJECT_MECHANISM_{id}_DESCRIPTION", "Mechanisms", 3)]
		[SerializeField]
		private int m_i18nDescriptionId;

		private ILevelOnlyDependant m_baseMecaLife;

		[SerializeField]
		private AssetReference m_illustrationReference;

		public int i18nNameId => m_i18nNameId;

		public int i18nDescriptionId => m_i18nDescriptionId;

		public ILevelOnlyDependant baseMecaLife => m_baseMecaLife;

		public AssetReference illustrationReference => m_illustrationReference;

		public override void PopulateFromJson(JObject jsonObject)
		{
			base.PopulateFromJson(jsonObject);
			m_baseMecaLife = ILevelOnlyDependantUtils.FromJsonProperty(jsonObject, "baseMecaLife");
		}
	}
}
