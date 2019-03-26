using Ankama.Cube.Configuration;
using Ankama.Cube.Extensions;
using Ankama.Cube.Network;
using Ankama.Cube.Player;
using Ankama.Cube.UI;
using Ankama.Cube.Utility;
using Com.Ankama.Haapi.Swagger.Api;
using Com.Ankama.Haapi.Swagger.Model;
using System;
using System.Collections;

namespace Ankama.Cube.States
{
	public class SelectLoginUIState : LoadSceneStateContext
	{
		private SelectLoginUI m_ui;

		protected override IEnumerator Load()
		{
			UILoader<SelectLoginUI> loader = new UILoader<SelectLoginUI>(this, "SelectLoginUI", "core/scenes/ui/login", disableOnLoad: true);
			yield return loader.Load();
			m_ui = loader.ui;
			m_ui.get_gameObject().SetActive(true);
			if (ApplicationConfig.haapiAllowed)
			{
				m_ui.OnConnectGuest += OnConnectGuest;
				m_ui.OnCreateGuest += OnCreateGuest;
			}
			else
			{
				m_ui.HideGuestSelection();
			}
			m_ui.OnRegularAccount += OnRegularAccount;
		}

		protected override IEnumerator Unload()
		{
			yield return _003C_003En__0();
			if (ApplicationConfig.haapiAllowed)
			{
				m_ui.OnConnectGuest -= OnConnectGuest;
				m_ui.OnCreateGuest -= OnCreateGuest;
			}
			m_ui.OnRegularAccount -= OnRegularAccount;
		}

		private void OnRegularAccount()
		{
			m_ui.interactable = false;
			StatesUtility.DoTransition(new LoginUIState(), this);
		}

		private void OnCreateGuest()
		{
			m_ui.interactable = false;
			HaapiManager.ExecuteRequest(() => HaapiManager.accountApi.CreateGuest((long?)ApplicationConfig.GameAppId, RuntimeData.currentCultureCode.GetLanguage(), string.Empty, string.Empty), OnCreateGuestSuccess, OnCreateGuestError);
		}

		private void OnCreateGuestSuccess(RAccountApi<Account> response)
		{
			string firstHeaderValue = response.RestResponse.get_Headers().GetFirstHeaderValue("X-Password");
			PlayerPreferences.guestLogin = response.Data.get_Login();
			PlayerPreferences.guestPassword = firstHeaderValue;
			PlayerPreferences.Save();
			ConnectionHandler.instance.Connect();
		}

		private void OnCreateGuestError(Exception obj)
		{
			m_ui.interactable = true;
		}

		private void OnConnectGuest()
		{
			ConnectionHandler.instance.Connect();
		}
	}
}
