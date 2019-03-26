using Ankama.Cube.Data;
using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

namespace Ankama.Cube.Maps.Objects
{
	public interface ICharacterObject : IMovableIsoObject, IIsoObject
	{
		[PublicAPI]
		Direction direction
		{
			get;
			set;
		}

		[PublicAPI]
		IEnumerator AddPropertyEffect([NotNull] AttachableEffect attachableEffect, PropertyId propertyId);

		[PublicAPI]
		IEnumerator RemovePropertyEffect([NotNull] AttachableEffect attachableEffect, PropertyId propertyId);

		[PublicAPI]
		IEnumerator Spawn();

		[PublicAPI]
		IEnumerator Die();

		[PublicAPI]
		void CheckParentCellIndicator();

		[PublicAPI]
		void ShowSpellTargetFeedback(bool isSelected);

		[PublicAPI]
		void HideSpellTargetFeedback();

		void SetPosition([NotNull] IMap map, Vector2 position);

		void ChangeDirection(Direction newDirection);
	}
}
