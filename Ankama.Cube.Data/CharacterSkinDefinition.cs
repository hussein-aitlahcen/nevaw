using Ankama.AssetManagement.AssetReferences;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class CharacterSkinDefinition : EditableData
	{
		[LocalizedString("CHARACTER_SKIN_{id}_NAME", "CharacterSkins", 1)]
		[SerializeField]
		private int m_i18nNameId;

		[SerializeField]
		private BundleCategory m_bundleCategory;

		[SerializeField]
		private AssetReference m_maleAnimatedCharacterDataReference;

		[SerializeField]
		private AssetReference m_femaleAnimatedCharacterDataReference;

		public int i18nNameId => m_i18nNameId;

		public BundleCategory bundleCategory => m_bundleCategory;

		public AssetReference maleAnimatedCharacterDataReference => m_maleAnimatedCharacterDataReference;

		public AssetReference femaleAnimatedCharacterDataReference => m_femaleAnimatedCharacterDataReference;

		public override void PopulateFromJson(JObject jsonObject)
		{
			this.PopulateFromJson(jsonObject);
		}

		public AssetReference GetAnimatedCharacterDataReference(Gender gender)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			switch (gender)
			{
			case Gender.Male:
				return m_maleAnimatedCharacterDataReference;
			case Gender.Female:
				return m_femaleAnimatedCharacterDataReference;
			default:
				throw new ArgumentOutOfRangeException("gender", gender, null);
			}
		}

		public CharacterSkinDefinition()
			: this()
		{
		}
	}
}
