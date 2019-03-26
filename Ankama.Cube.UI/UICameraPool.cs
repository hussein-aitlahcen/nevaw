using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.UI
{
	public class UICameraPool
	{
		private readonly List<UICamera> m_freeCameras = new List<UICamera>();

		private readonly List<UICamera> m_busyCameras = new List<UICamera>();

		private readonly Camera m_prefab;

		public List<UICamera> busyCameras => m_busyCameras;

		public UICameraPool(Camera prefab)
		{
			m_prefab = prefab;
		}

		public void ReleaseAll()
		{
			int count = m_busyCameras.Count;
			for (int i = 0; i < count; i++)
			{
				UICamera uICamera = m_busyCameras[i];
				uICamera.camera.get_gameObject().SetActive(false);
				uICamera.Clean();
				m_freeCameras.Add(uICamera);
			}
			m_busyCameras.Clear();
		}

		public UICamera Get()
		{
			UICamera uICamera;
			if (m_freeCameras.Count == 0)
			{
				uICamera = new UICamera(Object.Instantiate<Camera>(m_prefab, m_prefab.get_transform().get_parent()));
			}
			else
			{
				uICamera = m_freeCameras[0];
				m_freeCameras.RemoveAt(0);
			}
			m_busyCameras.Add(uICamera);
			uICamera.camera.get_gameObject().SetActive(true);
			return uICamera;
		}
	}
}
