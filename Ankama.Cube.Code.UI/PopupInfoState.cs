using Ankama.AssetManagement.InputManagement;
using Ankama.AssetManagement.StateManagement;
using Ankama.Cube.States;
using Ankama.Utilities;
using System;
using System.Collections;

namespace Ankama.Cube.Code.UI
{
	public class PopupInfoState : LoadSceneStateContext
	{
		private PopupInfoUI m_ui;

		private readonly PopupInfo m_data;

		private bool m_closing;

		public Action onClose;

		public PopupInfoState(PopupInfo data)
		{
			m_data = data;
		}

		protected override IEnumerator Load()
		{
			UILoader<PopupInfoUI> loader = new UILoader<PopupInfoUI>(this, "PopupInfoUI", "core/scenes/ui/popup", disableOnLoad: true);
			yield return loader.Load();
			m_ui = loader.ui;
			m_ui.Initialize(m_data);
			m_ui.get_gameObject().SetActive(true);
			yield return m_ui.OpenCoroutine();
		}

		protected override void Enable()
		{
			m_ui.closeAction = Close;
		}

		protected override IEnumerator Update()
		{
			if (m_data.hasDisplayDuration)
			{
				yield return (object)new WaitForTime(m_data.displayDuration);
				Close();
			}
		}

		protected override void Disable()
		{
			if (null != m_ui)
			{
				m_ui.RemoveListeners();
				m_ui.closeAction = null;
			}
		}

		protected override IEnumerator Unload()
		{
			if (null != m_ui)
			{
				yield return m_ui.CloseCoroutine();
			}
			yield return _003C_003En__0();
		}

		protected unsafe override bool UseInput(InputState inputState)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			if ((int)((IntPtr)(void*)inputState).state != 1)
			{
				return this.UseInput(inputState);
			}
			switch (((IntPtr)(void*)inputState).id)
			{
			case 1:
				return true;
			case 2:
			case 3:
				if (null != m_ui)
				{
					m_ui.DoClickSelected();
				}
				return true;
			case 4:
				if (null != m_ui)
				{
					m_ui.SelectNext();
				}
				return true;
			default:
				return this.UseInput(inputState);
			}
		}

		public void Close()
		{
			if (!m_closing)
			{
				m_closing = true;
				StateContext parent = this.get_parent();
				if (parent != null)
				{
					parent.ClearChildState(0);
				}
				onClose?.Invoke();
			}
		}
	}
}
