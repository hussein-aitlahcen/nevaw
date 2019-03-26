using Ankama.Cube.Data;
using JetBrains.Annotations;
using UnityEngine;

namespace Ankama.Cube.Maps.Objects
{
	[UsedImplicitly]
	public sealed class MovablePropObject : MovableIsoObject
	{
		[SerializeField]
		private PropDefinition m_definition;

		public override IsoObjectDefinition definition
		{
			get
			{
				return m_definition;
			}
			protected set
			{
				m_definition = (PropDefinition)value;
			}
		}
	}
}
