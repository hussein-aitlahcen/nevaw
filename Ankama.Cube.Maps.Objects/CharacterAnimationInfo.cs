using Ankama.Cube.Data;
using Ankama.Cube.Extensions;
using System;
using UnityEngine;

namespace Ankama.Cube.Maps.Objects
{
	public struct CharacterAnimationInfo
	{
		public readonly Vector2 position;

		public readonly Direction direction;

		public readonly string animationName;

		public readonly string timelineKey;

		public readonly bool flipX;

		public readonly bool loops;

		public readonly CharacterAnimationParameters parameters;

		public CharacterAnimationInfo(Vector2 position, string animationName, string timelineKey, bool loops, Direction direction, DirectionAngle mapRotation)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			this.position = position;
			this.direction = direction;
			switch (direction.Rotate(mapRotation))
			{
			case Direction.None:
				this.animationName = animationName;
				flipX = false;
				break;
			case Direction.East:
				this.animationName = animationName + "4";
				flipX = true;
				break;
			case Direction.SouthEast:
				this.animationName = animationName + "1";
				flipX = false;
				break;
			case Direction.South:
				this.animationName = animationName + "2";
				flipX = false;
				break;
			case Direction.SouthWest:
				this.animationName = animationName + "1";
				flipX = true;
				break;
			case Direction.West:
				this.animationName = animationName + "4";
				flipX = false;
				break;
			case Direction.NorthWest:
				this.animationName = animationName + "5";
				flipX = false;
				break;
			case Direction.North:
				this.animationName = animationName + "6";
				flipX = false;
				break;
			case Direction.NorthEast:
				this.animationName = animationName + "5";
				flipX = true;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			this.timelineKey = timelineKey;
			this.loops = loops;
			parameters = new CharacterAnimationParameters(animationName, timelineKey, loops, direction, Direction.None);
		}

		public CharacterAnimationInfo(Vector2 position, string animationName, string timelineKey, bool loops, Direction previousDirection, Direction direction, DirectionAngle mapRotation)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			this.position = position;
			this.direction = direction;
			Direction direction2 = previousDirection.Rotate(mapRotation);
			Direction direction3 = direction.Rotate(mapRotation);
			switch (direction2)
			{
			case Direction.SouthEast:
				switch (direction3)
				{
				case Direction.SouthWest:
					this.animationName = animationName + "31";
					break;
				case Direction.NorthEast:
					this.animationName = animationName + "35";
					break;
				case Direction.NorthWest:
					this.animationName = animationName + "35";
					break;
				default:
					throw new ArgumentOutOfRangeException("direction", $"Incompatible change of direction: {previousDirection} to {direction}.");
				}
				flipX = true;
				break;
			case Direction.SouthWest:
				switch (direction3)
				{
				case Direction.SouthEast:
					this.animationName = animationName + "31";
					break;
				case Direction.NorthWest:
					this.animationName = animationName + "35";
					break;
				case Direction.NorthEast:
					this.animationName = animationName + "35";
					break;
				default:
					throw new ArgumentOutOfRangeException("direction", $"Incompatible change of direction: {previousDirection} to {direction}.");
				}
				flipX = false;
				break;
			case Direction.NorthWest:
				switch (direction3)
				{
				case Direction.SouthWest:
					this.animationName = animationName + "71";
					break;
				case Direction.NorthEast:
					this.animationName = animationName + "75";
					break;
				case Direction.SouthEast:
					this.animationName = animationName + "71";
					break;
				default:
					throw new ArgumentOutOfRangeException("direction", $"Incompatible change of direction: {previousDirection} to {direction}.");
				}
				flipX = true;
				break;
			case Direction.NorthEast:
				switch (direction3)
				{
				case Direction.SouthEast:
					this.animationName = animationName + "71";
					break;
				case Direction.NorthWest:
					this.animationName = animationName + "75";
					break;
				case Direction.SouthWest:
					this.animationName = animationName + "71";
					break;
				default:
					throw new ArgumentOutOfRangeException("direction", $"Incompatible change of direction: {previousDirection} to {direction}.");
				}
				flipX = false;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			this.timelineKey = timelineKey;
			this.loops = loops;
			parameters = new CharacterAnimationParameters(animationName, timelineKey, loops, previousDirection, direction);
		}
	}
}
