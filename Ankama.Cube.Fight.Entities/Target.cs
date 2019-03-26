using Ankama.Cube.Data;
using Ankama.Cube.Protocols.CommonProtocol;
using JetBrains.Annotations;
using System;

namespace Ankama.Cube.Fight.Entities
{
	public struct Target : IEquatable<Target>, IEquatable<Coord>, IEquatable<IEntity>
	{
		public enum Type
		{
			Coord,
			Entity
		}

		public readonly Coord coord;

		public readonly IEntity entity;

		public readonly Type type;

		public Target(Coord coord)
		{
			type = Type.Coord;
			this.coord = coord;
			entity = null;
		}

		public Target(IEntity entity)
		{
			type = Type.Entity;
			coord = default(Coord);
			this.entity = entity;
		}

		[Pure]
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj is Target)
			{
				return Equals((Target)obj);
			}
			switch (type)
			{
			case Type.Coord:
				if (obj is Coord)
				{
					return coord == (Coord)obj;
				}
				return false;
			case Type.Entity:
			{
				IEntity entity = obj as IEntity;
				return this.entity == entity;
			}
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		[Pure]
		public override int GetHashCode()
		{
			switch (type)
			{
			case Type.Coord:
				return coord.GetHashCode();
			case Type.Entity:
				return entity.GetHashCode();
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		[Pure]
		public bool Equals(Target other)
		{
			if (type != other.type)
			{
				return false;
			}
			switch (type)
			{
			case Type.Coord:
				return coord == other.coord;
			case Type.Entity:
				return entity == other.entity;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		[Pure]
		public static bool operator ==(Target value, Target other)
		{
			if (value.type != other.type)
			{
				return false;
			}
			switch (value.type)
			{
			case Type.Coord:
				return value.coord == other.coord;
			case Type.Entity:
				return value.entity == other.entity;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		[Pure]
		public static bool operator !=(Target value, Target other)
		{
			if (value.type != other.type)
			{
				return true;
			}
			switch (value.type)
			{
			case Type.Coord:
				return value.coord != other.coord;
			case Type.Entity:
				return value.entity != other.entity;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		[Pure]
		public bool Equals(Coord other)
		{
			if (type == Type.Coord)
			{
				return coord == other;
			}
			return false;
		}

		[Pure]
		public static bool operator ==(Target value, Coord other)
		{
			if (value.type == Type.Coord)
			{
				return value.coord == other;
			}
			return false;
		}

		[Pure]
		public static bool operator !=(Target value, Coord other)
		{
			if (value.type == Type.Coord)
			{
				return value.coord != other;
			}
			return true;
		}

		[Pure]
		public bool Equals(IEntity other)
		{
			if (type == Type.Entity)
			{
				return entity == other;
			}
			return false;
		}

		[Pure]
		public static bool operator ==(Target value, IEntity other)
		{
			if (value.type == Type.Entity)
			{
				return value.entity == other;
			}
			return false;
		}

		[Pure]
		public static bool operator !=(Target value, IEntity other)
		{
			if (value.type == Type.Entity)
			{
				return value.entity != other;
			}
			return true;
		}

		[Pure]
		public CastTarget ToCastTarget()
		{
			switch (type)
			{
			case Type.Coord:
				return new CastTarget
				{
					Cell = coord.ToCellCoord()
				};
			case Type.Entity:
				return new CastTarget
				{
					EntityId = entity.id
				};
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
	}
}
