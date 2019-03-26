using Ankama.Cube.UI.Components;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Ankama.Cube.UI.PlayerLayer
{
	public class PlayerLayerNavButton : MonoBehaviour
	{
		private Action m_method;

		private PlayerLayerNavRoot m_root;

		private AnimatedToggleButton m_toggle;

		[SerializeField]
		private Transform m_parent;

		public unsafe void Initialise(PlayerLayerNavRoot root)
		{
			m_root = root;
			m_toggle = this.GetComponent<AnimatedToggleButton>();
			m_toggle.onValueChanged.AddListener(new UnityAction<bool>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		}

		public void OnClic(bool ison)
		{
			m_root.OnClic(this);
		}

		public void OnDeselect()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			ShortcutExtensions.DOScale(m_parent, Vector3.get_one(), 0.15f);
		}

		public void OnValidate()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			ShortcutExtensions.DOScale(m_parent, Vector3.get_one() * 1.2f, 0.15f);
		}

		public Action GetMethod()
		{
			return m_method;
		}

		public void SetMethode(Action action)
		{
			m_method = action;
		}

		public void ForceClic()
		{
			m_toggle.set_isOn(true);
		}

		public PlayerLayerNavButton()
			: this()
		{
		}
	}
}
