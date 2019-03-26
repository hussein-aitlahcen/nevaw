using DataEditor;
using Newtonsoft.Json.Linq;
using System;

namespace Ankama.Cube.Data
{
	[Serializable]
	public abstract class IsoObjectDefinition : EditableData
	{
		protected IObjectAreaDefinition m_areaDefinition;

		public IObjectAreaDefinition areaDefinition => m_areaDefinition;

		public override void PopulateFromJson(JObject jsonObject)
		{
			this.PopulateFromJson(jsonObject);
			m_areaDefinition = IObjectAreaDefinitionUtils.FromJsonProperty(jsonObject, "areaDefinition");
		}

		protected IsoObjectDefinition()
			: this()
		{
		}
	}
}
