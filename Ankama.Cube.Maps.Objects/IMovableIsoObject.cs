using JetBrains.Annotations;
using UnityEngine;

namespace Ankama.Cube.Maps.Objects
{
	public interface IMovableIsoObject : IIsoObject
	{
		[PublicAPI]
		void SetCellObject([NotNull] CellObject containerCell);

		[PublicAPI]
		void SetCellObjectInnerPosition(Vector2 innerPosition);
	}
}
