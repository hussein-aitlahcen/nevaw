using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Ankama.Cube.Data
{
	[Serializable]
	public abstract class MechanismDefinition : AnimatedIsoObjectDefinition, IEffectList, IEditableContent, IFamilyList
	{
		protected PrecomputedData m_precomputedData;

		protected List<Family> m_families;

		protected IAreaDefinition m_effectAreaDefinition;

		public PrecomputedData precomputedData => m_precomputedData;

		public IReadOnlyList<Family> families => m_families;

		public IAreaDefinition effectAreaDefinition => m_effectAreaDefinition;

		public override void PopulateFromJson(JObject jsonObject)
		{
			base.PopulateFromJson(jsonObject);
			m_precomputedData = PrecomputedData.FromJsonProperty(jsonObject, "precomputedData");
			m_families = Serialization.JsonArrayAsList<Family>(jsonObject, "families");
			m_effectAreaDefinition = IAreaDefinitionUtils.FromJsonProperty(jsonObject, "effectAreaDefinition");
		}
	}
}
