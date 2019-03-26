using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.Maps.Objects
{
	public interface IObjectWithAssemblage : ICharacterObject, IMovableIsoObject, IIsoObject
	{
		void RefreshAssemblage(IEnumerable<Vector2Int> otherObjectInAssemblagePositions);
	}
}
