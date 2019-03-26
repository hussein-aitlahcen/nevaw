using Ankama.Cube.Data;
using System.Collections.Generic;

namespace Ankama.Cube.Fight.Entities
{
	public interface IEntity
	{
		int id
		{
			get;
		}

		EntityType type
		{
			get;
		}

		bool isDirty
		{
			get;
		}

		IReadOnlyCollection<PropertyId> properties
		{
			get;
		}

		void SetCarac(CaracId caracId, int value);

		int GetCarac(CaracId caracId, int defaultValue = 0);

		bool HasProperty(PropertyId property);

		bool HasAnyProperty(params PropertyId[] properties);
	}
}
