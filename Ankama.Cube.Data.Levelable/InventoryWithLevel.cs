using System.Collections;
using System.Collections.Generic;

namespace Ankama.Cube.Data.Levelable
{
	public class InventoryWithLevel : IInventoryWithLevel, IEnumerable<int>, IEnumerable, ILevelProvider
	{
		private readonly Dictionary<int, int> m_levels = new Dictionary<int, int>();

		private readonly int? m_defaultLevel;

		public InventoryWithLevel(int? defaultLevel = default(int?))
		{
			m_defaultLevel = defaultLevel;
		}

		public void UpdateAllLevels(IDictionary<int, int> levels)
		{
			m_levels.Clear();
			foreach (KeyValuePair<int, int> level in levels)
			{
				m_levels.Add(level.Key, level.Value);
			}
		}

		public bool TryGetLevel(int id, out int level)
		{
			if (m_levels.TryGetValue(id, out level))
			{
				return true;
			}
			if (!m_defaultLevel.HasValue)
			{
				return false;
			}
			level = m_defaultLevel.Value;
			return true;
		}

		public IEnumerator<int> GetEnumerator()
		{
			return m_levels.Keys.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
