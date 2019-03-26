using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Ankama.Cube.Data
{
	[Serializable]
	public abstract class CharacterDefinition : AnimatedIsoObjectDefinition, IEffectList, IEditableContent, IFamilyList
	{
		protected PrecomputedData m_precomputedData;

		protected List<Family> m_families;

		protected ILevelOnlyDependant m_life;

		protected ILevelOnlyDependant m_movementPoints;

		protected ILevelOnlyDependant m_actionValue;

		protected IEntitySelector m_customActionTarget;

		protected ActionType m_actionType;

		protected ActionRange m_actionRange;

		protected AIArchetype m_aiArchetype;

		public PrecomputedData precomputedData => m_precomputedData;

		public IReadOnlyList<Family> families => m_families;

		public ILevelOnlyDependant life => m_life;

		public ILevelOnlyDependant movementPoints => m_movementPoints;

		public ILevelOnlyDependant actionValue => m_actionValue;

		public IEntitySelector customActionTarget => m_customActionTarget;

		public ActionType actionType => m_actionType;

		public ActionRange actionRange => m_actionRange;

		public AIArchetype aiArchetype => m_aiArchetype;

		public override void PopulateFromJson(JObject jsonObject)
		{
			base.PopulateFromJson(jsonObject);
			m_precomputedData = PrecomputedData.FromJsonProperty(jsonObject, "precomputedData");
			m_families = Serialization.JsonArrayAsList<Family>(jsonObject, "families");
			m_life = ILevelOnlyDependantUtils.FromJsonProperty(jsonObject, "life");
			m_movementPoints = ILevelOnlyDependantUtils.FromJsonProperty(jsonObject, "movementPoints");
			m_actionValue = ILevelOnlyDependantUtils.FromJsonProperty(jsonObject, "actionValue");
			m_customActionTarget = IEntitySelectorUtils.FromJsonProperty(jsonObject, "customActionTarget");
			m_actionType = (ActionType)Serialization.JsonTokenValue<int>(jsonObject, "actionType", 0);
			m_actionRange = ActionRange.FromJsonProperty(jsonObject, "actionRange");
			m_aiArchetype = (AIArchetype)Serialization.JsonTokenValue<int>(jsonObject, "aiArchetype", 0);
		}
	}
}
