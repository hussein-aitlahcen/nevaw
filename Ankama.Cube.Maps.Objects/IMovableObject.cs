using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

namespace Ankama.Cube.Maps.Objects
{
	public interface IMovableObject : ICharacterObject, IMovableIsoObject, IIsoObject
	{
		[PublicAPI]
		IEnumerator Pull(Vector2Int[] movementCells);

		[PublicAPI]
		IEnumerator Push(Vector2Int[] movementCells);

		[PublicAPI]
		void Teleport(Vector2Int target);
	}
}
