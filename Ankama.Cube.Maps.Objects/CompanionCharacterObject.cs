using Ankama.Cube.Data;
using Ankama.Cube.Fight;
using JetBrains.Annotations;
using UnityEngine;

namespace Ankama.Cube.Maps.Objects
{
	[UsedImplicitly]
	public sealed class CompanionCharacterObject : FightCharacterObject
	{
		[SerializeField]
		private CompanionDefinition m_definition;

		public override IsoObjectDefinition definition
		{
			get
			{
				return m_definition;
			}
			protected set
			{
				m_definition = (CompanionDefinition)value;
			}
		}

		public override KeywordReference[] keywordReferences => m_definition.precomputedData.keywordReferences;

		public override void Destroy()
		{
			FightObjectFactory.ReleaseCompanionCharacterObject(this);
		}

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
