using Ankama.Cube.Data;
using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

namespace Ankama.Cube.Maps.Objects
{
	public interface IObjectWithAction : ICharacterObject, IMovableIsoObject, IIsoObject
	{
		[PublicAPI]
		ActionType actionType
		{
			get;
		}

		[PublicAPI]
		int? actionValue
		{
			get;
		}

		[PublicAPI]
		int physicalDamageBoost
		{
			get;
		}

		[PublicAPI]
		int physicalHealBoost
		{
			get;
		}

		[PublicAPI]
		void SetPhysicalDamageBoost(int value);

		[PublicAPI]
		void SetPhysicalHealBoost(int value);

		[PublicAPI]
		void SetActionUsed(bool actionUsed, bool turnEnded);

		IEnumerator PlayActionAnimation(Direction directionToAttack, bool waitForAnimationEndOnMissingLabel);

		IEnumerator PlayRangedActionAnimation(Direction directionToAttack);

		void TriggerActionEffect(Vector2Int target);
	}
}
