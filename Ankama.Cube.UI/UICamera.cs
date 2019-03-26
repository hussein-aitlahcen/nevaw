using Ankama.Cube.SRP;
using System.Collections.Generic;
using UnityEngine;

namespace Ankama.Cube.UI
{
	public class UICamera : IBlurCamera
	{
		public Camera camera;

		public List<AbstractUI> uis;

		public UICamera child;

		private bool m_hasBlur;

		private bool m_isBlurEnabled;

		private AbstractUI m_linkedBlurUI;

		public float factor
		{
			get
			{
				if (!(m_linkedBlurUI != null))
				{
					return 0f;
				}
				return m_linkedBlurUI.blurAmount;
			}
		}

		public bool hasBlur => m_hasBlur;

		public bool isBlurEnabled => m_isBlurEnabled;

		public bool isFullBlur
		{
			get
			{
				if (m_linkedBlurUI != null)
				{
					return m_linkedBlurUI.blurAmount >= 1f;
				}
				return false;
			}
		}

		public UICamera(Camera cam)
		{
			camera = cam;
			m_isBlurEnabled = false;
			uis = new List<AbstractUI>();
		}

		private void EnableBlur(bool value)
		{
			if (m_isBlurEnabled != value)
			{
				m_isBlurEnabled = value;
				if (value)
				{
					CubeSRP.s_blurCamera[camera] = this;
				}
				else
				{
					CubeSRP.s_blurCamera.Remove(camera);
				}
			}
		}

		public void ActiveBlurFor(AbstractUI linkedUI)
		{
			m_hasBlur = true;
			EnableBlur(value: true);
			m_linkedBlurUI = linkedUI;
			m_linkedBlurUI.onBlurFactorIsFull = OnBlurFactorIsFull;
			m_linkedBlurUI.onBlurFactorStartDecrease = OnBlurFactorStartDecrease;
		}

		public void Clean()
		{
			m_hasBlur = false;
			EnableBlur(value: false);
			if (m_linkedBlurUI != null)
			{
				m_linkedBlurUI.onBlurFactorIsFull = null;
				m_linkedBlurUI.onBlurFactorStartDecrease = null;
				m_linkedBlurUI = null;
			}
			uis.Clear();
			child = null;
		}

		public void NeedToDisplayBlur(bool value)
		{
			if (m_hasBlur)
			{
				EnableBlur(value);
			}
		}

		private void OnBlurFactorIsFull()
		{
			if (m_isBlurEnabled && child != null)
			{
				child.NeedToDisplayBlurRecursive(value: false);
			}
		}

		private void OnBlurFactorStartDecrease()
		{
			if (m_isBlurEnabled && child != null)
			{
				child.NeedToDisplayBlurRecursive(value: true);
			}
		}

		private void NeedToDisplayBlurRecursive(bool value)
		{
			if (m_hasBlur)
			{
				EnableBlur(value);
			}
			if (child != null)
			{
				if (!m_hasBlur)
				{
					child.NeedToDisplayBlur(value);
				}
				else if (!value)
				{
					child.NeedToDisplayBlur(value: false);
				}
				else if (isFullBlur)
				{
					child.NeedToDisplayBlur(value: false);
				}
				else
				{
					child.NeedToDisplayBlur(value: true);
				}
			}
		}
	}
}
