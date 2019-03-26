using DataEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Ankama.Cube.Data
{
	[Serializable]
	public abstract class AnimatedIsoObjectDefinition : IsoObjectDefinition
	{
		protected Id<CharacterSkinDefinition> m_defaultSkin;

		protected List<Id<CharacterSkinDefinition>> m_skins;

		public Id<CharacterSkinDefinition> defaultSkin => m_defaultSkin;

		public IReadOnlyList<Id<CharacterSkinDefinition>> skins => m_skins;

		public override void PopulateFromJson(JObject jsonObject)
		{
			base.PopulateFromJson(jsonObject);
			m_defaultSkin = Serialization.JsonTokenIdValue<CharacterSkinDefinition>(jsonObject, "defaultSkin");
			JArray val = Serialization.JsonArray(jsonObject, "skins");
			m_skins = new List<Id<CharacterSkinDefinition>>((val != null) ? val.get_Count() : 0);
			if (val != null)
			{
				foreach (JToken item2 in val)
				{
					Id<CharacterSkinDefinition> item = Serialization.JsonTokenIdValue<CharacterSkinDefinition>(item2);
					m_skins.Add(item);
				}
			}
		}
	}
}
