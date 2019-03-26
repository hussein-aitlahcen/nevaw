using Ankama.Cube.Data;
using JetBrains.Annotations;
using UnityEngine;

namespace Ankama.Cube.Maps.Objects
{
	[UsedImplicitly]
	public sealed class HeroCharacterObject : FightCharacterObject
	{
		[SerializeField]
		private WeaponDefinition m_definition;

		public override IsoObjectDefinition definition
		{
			get
			{
				return m_definition;
			}
			protected set
			{
				m_definition = (WeaponDefinition)value;
			}
		}

		public override KeywordReference[] keywordReferences => m_definition.precomputedData.keywordReferences;

		public override int GetTitleKey()
		{
			return m_definition.i18nNameId;
		}

		public override int GetDescriptionKey()
		{
			return m_definition.i18nDescriptionId;
		}
	}
}
