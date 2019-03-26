using UnityEngine;

namespace Ankama.Cube.Maps
{
	public class ZaapGodObject : ZaapObject
	{
		[SerializeField]
		private Transform m_statuePosition;

		private GameObject m_statue;

		public void SetStatue(GameObject prefab)
		{
			if (m_statue != null)
			{
				Object.Destroy(m_statue);
				m_statue = null;
			}
			if (!(prefab == null))
			{
				m_statue = Object.Instantiate<GameObject>(prefab, m_statuePosition, false);
			}
		}
	}
}
