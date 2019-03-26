using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Components
{
	public class PopupMenu : MonoBehaviour
	{
		[SerializeField]
		private ContainerDrawer m_drawer;

		[SerializeField]
		private Button m_closeButton;

		[SerializeField]
		private GameObject m_closeContainer;

		private unsafe void Awake()
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Expected O, but got Unknown
			m_drawer.open = false;
			m_closeContainer.SetActive(false);
			m_closeButton.get_onClick().AddListener(new UnityAction((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		}

		private void OnClose()
		{
			Close();
		}

		public void Open()
		{
			m_drawer.Open();
			m_closeContainer.SetActive(true);
		}

		public void Close()
		{
			m_drawer.Close();
			m_closeContainer.SetActive(false);
		}

		public PopupMenu()
			: this()
		{
		}
	}
}
