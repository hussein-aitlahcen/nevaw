using Ankama.Cube.Data;
using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

namespace Ankama.Cube.Maps.Objects
{
	public interface IObjectWithMovement : ICharacterObject, IMovableIsoObject, IIsoObject
	{
		[PublicAPI]
		int movementPoints
		{
			get;
		}

		[PublicAPI]
		int baseMovementPoints
		{
			get;
		}

		[PublicAPI]
		void SetMovementPoints(int value);

		[PublicAPI]
		IEnumerator Move(Vector2Int[] movementCells);

		[PublicAPI]
		IEnumerator MoveToAction(Vector2Int[] movementCells, Direction actionDirection, bool hasFollowUpAnimation = true);
	}
}
