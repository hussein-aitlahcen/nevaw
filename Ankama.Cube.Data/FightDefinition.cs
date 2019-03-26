using Ankama.AssetManagement.AssetReferences;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class FightDefinition : EditableData
	{
		[LocalizedString("FIGHT_{id}_NAME", "Fight", 1)]
		[SerializeField]
		private int m_i18nNameId;

		private int m_maxSpellInHand;

		private FightType m_fightType;

		private int m_playersPerTeam;

		private int m_fightsCount;

		private bool m_versusAI;

		private IFightAdditionalDataDefinition m_additionalData;

		[SerializeField]
		private AssetReference m_illustrationReference;

		[SerializeField]
		private AssetReference m_fullIllustrationReference;

		[SerializeField]
		private string m_bundleName;

		public int i18nNameId => m_i18nNameId;

		public int maxSpellInHand => m_maxSpellInHand;

		public FightType fightType => m_fightType;

		public int playersPerTeam => m_playersPerTeam;

		public int fightsCount => m_fightsCount;

		public bool versusAI => m_versusAI;

		public IFightAdditionalDataDefinition additionalData => m_additionalData;

		public string bundleName => m_bundleName;

		public AssetReference illustrationReference => m_illustrationReference;

		public AssetReference fullIllustrationReference => m_fullIllustrationReference;

		public override void PopulateFromJson(JObject jsonObject)
		{
			this.PopulateFromJson(jsonObject);
			m_maxSpellInHand = Serialization.JsonTokenValue<int>(jsonObject, "maxSpellInHand", 7);
			m_fightType = (FightType)Serialization.JsonTokenValue<int>(jsonObject, "fightType", 0);
			m_playersPerTeam = Serialization.JsonTokenValue<int>(jsonObject, "playersPerTeam", 0);
			m_fightsCount = Serialization.JsonTokenValue<int>(jsonObject, "fightsCount", 0);
			m_versusAI = Serialization.JsonTokenValue<bool>(jsonObject, "versusAI", false);
			m_additionalData = IFightAdditionalDataDefinitionUtils.FromJsonProperty(jsonObject, "additionalData");
		}

		public FightDefinition()
			: this()
		{
		}
	}
}
