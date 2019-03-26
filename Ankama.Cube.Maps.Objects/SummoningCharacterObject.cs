using Ankama.Cube.Data;
using Ankama.Cube.Fight;
using JetBrains.Annotations;
using UnityEngine;

namespace Ankama.Cube.Maps.Objects
{
	[UsedImplicitly]
	public sealed class SummoningCharacterObject : FightCharacterObject
	{
		[SerializeField]
		private SummoningDefinition m_definition;

		public override IsoObjectDefinition definition
		{
			get
			{
				return m_definition;
			}
			protected set
			{
				m_definition = (SummoningDefinition)value;
			}
		}

		public override KeywordReference[] keywordReferences => m_definition.precomputedData.keywordReferences;

		public override void Destroy()
		{
			FightObjectFactory.ReleaseSummoningCharacterObject(this);
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
