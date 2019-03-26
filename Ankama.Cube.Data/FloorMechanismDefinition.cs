using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class FloorMechanismDefinition : MechanismDefinition, IDefinitionWithTooltip, IDefinitionWithDescription, IDefinitionWithPrecomputedData
	{
		[LocalizedString("FLOOR_MECHANISM_{id}_NAME", "Mechanisms", 1)]
		[SerializeField]
		private int m_i18nNameId;

		[LocalizedString("FLOOR_MECHANISM_{id}_DESCRIPTION", "Mechanisms", 3)]
		[SerializeField]
		private int m_i18nDescriptionId;

		private FloorMechanismType m_floorType;

		private ILevelOnlyDependant m_activationValue;

		private ActionType m_activationType;

		private FloorMechanismActivationType m_activationTrigger;

		private bool m_removeOnActivation;

		public int i18nNameId => m_i18nNameId;

		public int i18nDescriptionId => m_i18nDescriptionId;

		public FloorMechanismType floorType => m_floorType;

		public ILevelOnlyDependant activationValue => m_activationValue;

		public ActionType activationType => m_activationType;

		public FloorMechanismActivationType activationTrigger => m_activationTrigger;

		public bool removeOnActivation => m_removeOnActivation;

		public override void PopulateFromJson(JObject jsonObject)
		{
			base.PopulateFromJson(jsonObject);
			m_floorType = (FloorMechanismType)Serialization.JsonTokenValue<int>(jsonObject, "floorType", 1);
			m_activationValue = ILevelOnlyDependantUtils.FromJsonProperty(jsonObject, "activationValue");
			m_activationType = (ActionType)Serialization.JsonTokenValue<int>(jsonObject, "activationType", 0);
			m_activationTrigger = (FloorMechanismActivationType)Serialization.JsonTokenValue<int>(jsonObject, "activationTrigger", 0);
			m_removeOnActivation = Serialization.JsonTokenValue<bool>(jsonObject, "removeOnActivation", true);
		}
	}
}
