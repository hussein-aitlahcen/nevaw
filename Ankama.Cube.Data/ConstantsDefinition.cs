using DataEditor;
using Newtonsoft.Json.Linq;
using System;

namespace Ankama.Cube.Data
{
	[Serializable]
	public sealed class ConstantsDefinition : EditableData
	{
		private int m_agonyThreshold;

		public int agonyThreshold => m_agonyThreshold;

		public override void PopulateFromJson(JObject jsonObject)
		{
			this.PopulateFromJson(jsonObject);
			m_agonyThreshold = Serialization.JsonTokenValue<int>(jsonObject, "agonyThreshold", 20);
		}

		public ConstantsDefinition()
			: this()
		{
		}
	}
}
