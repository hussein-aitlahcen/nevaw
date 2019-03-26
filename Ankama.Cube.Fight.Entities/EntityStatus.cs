using Ankama.Cube.Data;
using System.Collections.Generic;

namespace Ankama.Cube.Fight.Entities
{
	public abstract class EntityStatus : IEntity
	{
		private static readonly IReadOnlyCollection<PropertyId> NoProperties = (IReadOnlyCollection<PropertyId>)new PropertyId[0];

		protected readonly int m_id;

		private readonly Dictionary<CaracId, int> m_caracs = new Dictionary<CaracId, int>(CaracIdComparer.instance);

		private HashSet<PropertyId> m_properties;

		public int id => m_id;

		public bool isDirty
		{
			get;
			private set;
		}

		public abstract EntityType type
		{
			get;
		}

		public IReadOnlyCollection<PropertyId> properties
		{
			get
			{
				IReadOnlyCollection<PropertyId> properties = m_properties;
				return properties ?? NoProperties;
			}
		}

		protected EntityStatus(int id)
		{
			m_id = id;
		}

		public void MarkForRemoval()
		{
			isDirty = true;
		}

		public int GetCarac(CaracId carac, int defaultValue = 0)
		{
			if (m_caracs.TryGetValue(carac, out int value))
			{
				return value;
			}
			return defaultValue;
		}

		public void SetCarac(CaracId carac, int value)
		{
			m_caracs[carac] = value;
		}

		public void AddProperty(PropertyId property)
		{
			if (m_properties == null)
			{
				m_properties = new HashSet<PropertyId>();
			}
			m_properties.Add(property);
		}

		public void RemoveProperty(PropertyId property)
		{
			m_properties?.Remove(property);
		}

		public bool HasProperty(PropertyId property)
		{
			if (m_properties != null)
			{
				return m_properties.Contains(property);
			}
			return false;
		}

		public bool HasAnyProperty(params PropertyId[] properties)
		{
			HashSet<PropertyId> properties2 = m_properties;
			if (properties2 == null)
			{
				return false;
			}
			int num = properties.Length;
			for (int i = 0; i < num; i++)
			{
				if (properties2.Contains(properties[i]))
				{
					return true;
				}
			}
			return false;
		}
	}
}
