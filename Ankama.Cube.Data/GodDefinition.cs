using Ankama.AssetManagement.AssetReferences;
using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class GodDefinition : EditableData, IDefinitionWithPrecomputedData
	{
		private God m_god;

		[LocalizedString("GOD_{id}_NAME", "Gods", 1)]
		[SerializeField]
		private int m_i18nNameId;

		[LocalizedString("GOD_{id}_DESCRIPTION", "Gods", 1)]
		[SerializeField]
		private int m_i18nDescriptionId;

		private PrecomputedData m_precomputedData;

		private Id<WeaponDefinition> m_defaultWeapon;

		private List<AbstractEffectDefinition> m_heroEffects;

		private bool? m_playable;

		[SerializeField]
		private AssetReference m_godIconReference;

		[SerializeField]
		private AssetReference m_statueUIReference;

		[SerializeField]
		private AssetReference m_BGVisualReference;

		[SerializeField]
		private int m_order;

		[SerializeField]
		private Color m_deckBuildingBackgroundColor2;

		[SerializeField]
		private AssetReference m_statuePrefabReference;

		public God god => m_god;

		public int i18nNameId => m_i18nNameId;

		public int i18nDescriptionId => m_i18nDescriptionId;

		public PrecomputedData precomputedData => m_precomputedData;

		public Id<WeaponDefinition> defaultWeapon => m_defaultWeapon;

		public IReadOnlyList<AbstractEffectDefinition> heroEffects => m_heroEffects;

		public int Order => m_order;

		public bool playable
		{
			get
			{
				if (!m_playable.HasValue)
				{
					m_playable = RuntimeData.IsPlayable(m_god);
				}
				return m_playable.Value;
			}
		}

		public AssetReference statuePrefabReference => m_statuePrefabReference;

		public override void PopulateFromJson(JObject jsonObject)
		{
			this.PopulateFromJson(jsonObject);
			m_god = (God)Serialization.JsonTokenValue<int>(jsonObject, "god", 0);
			m_precomputedData = PrecomputedData.FromJsonProperty(jsonObject, "precomputedData");
			m_defaultWeapon = Serialization.JsonTokenIdValue<WeaponDefinition>(jsonObject, "defaultWeapon");
			JArray val = Serialization.JsonArray(jsonObject, "heroEffects");
			m_heroEffects = new List<AbstractEffectDefinition>((val != null) ? val.get_Count() : 0);
			if (val != null)
			{
				foreach (JToken item in val)
				{
					m_heroEffects.Add(AbstractEffectDefinition.FromJsonToken(item));
				}
			}
		}

		public AssetReference GetUIIconReference()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return m_godIconReference;
		}

		public AssetReference GetUIBGReference()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return m_BGVisualReference;
		}

		public AssetReference GetUIStatueReference()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return m_statueUIReference;
		}

		public GodDefinition()
			: this()
		{
		}
	}
}
