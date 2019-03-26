using Ankama.AssetManagement;
using Ankama.AssetManagement.InputManagement;
using Ankama.AssetManagement.StateManagement;
using Ankama.Cube.TEMPFastEnterMatch.Player;
using Ankama.Cube.UI;
using Ankama.Utilities;
using System;
using System.Collections;
using Unity.Cloud.UserReporting;
using Unity.Cloud.UserReporting.Client;
using Unity.Cloud.UserReporting.Plugin;
using UnityEngine;

namespace Ankama.Cube.States
{
	public class BugReportState : LoadSceneStateContext
	{
		private UserReport m_currentBugReport;

		private BugReportUI m_ui;

		private GameObject m_updater;

		private bool m_isCreatingUserReport;

		private bool m_isSubmittingUserReport;

		public static bool isReady
		{
			get;
			private set;
		} = true;


		public void Initialize()
		{
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Expected O, but got Unknown
			isReady = false;
			m_isCreatingUserReport = true;
			m_isSubmittingUserReport = false;
			UnityUserReporting.Configure();
			UserReportingClient currentClient = UnityUserReporting.get_CurrentClient();
			currentClient.TakeScreenshot(1024, 1024, (Action<UserReportScreenshot>)delegate
			{
			});
			currentClient.CreateUserReport((Action<UserReport>)CreateUserReportCallback);
			m_updater = new GameObject("BugReportUpdater", new Type[1]
			{
				typeof(BugReportUpdater)
			});
		}

		protected override IEnumerator Load()
		{
			UILoader<BugReportUI> loader = new UILoader<BugReportUI>(this, "BugReportUI", "core/scenes/ui/option", disableOnLoad: true);
			yield return loader.Load();
			m_ui = loader.ui;
			while (m_isCreatingUserReport)
			{
				yield return null;
			}
			SetThumbnail();
			m_ui.get_gameObject().SetActive(true);
		}

		protected override void Enable()
		{
			m_ui.onSubmitClick = OnSubmitClick;
			m_ui.onCloseClick = OnCloseClick;
		}

		protected unsafe override bool UseInput(InputState inputState)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Invalid comparison between Unknown and I4
			int id = ((IntPtr)(void*)inputState).id;
			if (id == 1)
			{
				if ((int)((IntPtr)(void*)inputState).state == 1 && null != m_ui)
				{
					OnCloseClick();
				}
				return true;
			}
			return true;
		}

		protected override IEnumerator Unload()
		{
			isReady = true;
			if (null != m_updater)
			{
				Object.Destroy(m_updater);
				m_updater = null;
			}
			return this.Unload();
		}

		protected override void Disable()
		{
			m_ui.onSubmitClick = null;
			m_ui.onCloseClick = null;
		}

		private void SetThumbnail()
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Expected O, but got Unknown
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			if (m_currentBugReport != null && !(null == m_ui))
			{
				UserReportScreenshot thumbnail = m_currentBugReport.get_Thumbnail();
				byte[] array = Convert.FromBase64String(thumbnail.get_DataBase64());
				Texture2D val = new Texture2D(1, 1);
				ImageConversion.LoadImage(val, array);
				Sprite thumbnail2 = Sprite.Create(val, new Rect(0f, 0f, (float)val.get_width(), (float)val.get_height()), new Vector2(0.5f, 0.5f));
				m_ui.SetThumbnail(thumbnail2);
			}
		}

		private void OnSubmitClick(string summary, string description)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			if (m_currentBugReport != null && !m_isSubmittingUserReport)
			{
				m_isSubmittingUserReport = true;
				m_currentBugReport.set_Summary(summary);
				UserReportNamedValue val = default(UserReportNamedValue);
				val.set_Name("Description");
				val.set_Value(description);
				UserReportNamedValue item = val;
				m_currentBugReport.get_Fields().Add(item);
				PlayerData instance = PlayerData.instance;
				if (instance != null)
				{
					val = default(UserReportNamedValue);
					val.set_Name("User Nickname");
					val.set_Value(instance.nickName);
					UserReportNamedValue item2 = val;
					m_currentBugReport.get_Fields().Add(item2);
				}
				UnityUserReporting.get_CurrentClient().SendUserReport(m_currentBugReport, (Action<float, float>)UserReportSubmissionProgress, (Action<bool, UserReport>)UserReportSubmissionCallback);
			}
		}

		private void OnCloseClick()
		{
			SetFocusOnLayer("PlayerUI");
			this.get_parent().ClearChildState(0);
		}

		private void SetFocusOnLayer(string layerName)
		{
			StateLayer activeInputLayer = default(StateLayer);
			if (StateManager.TryGetLayer(layerName, ref activeInputLayer))
			{
				StateManager.SetActiveInputLayer(activeInputLayer);
			}
		}

		private void CreateUserReportCallback(UserReport bugReport)
		{
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrEmpty(bugReport.get_ProjectIdentifier()))
			{
				Log.Warning("The user report's project identifier is not set. Please setup cloud services using the Services tab or manually specify a project identifier when calling UnityUserReporting.Configure().", 171, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\States\\OptionLayer\\BugReportState.cs");
			}
			string str = "Unknown";
			string str2 = "0.0";
			foreach (UserReportNamedValue deviceMetadatum in bugReport.get_DeviceMetadata())
			{
				UserReportNamedValue current = deviceMetadatum;
				string name = current.get_Name();
				if (!(name == "Platform"))
				{
					if (name == "Version")
					{
						str2 = current.get_Value();
					}
				}
				else
				{
					str = current.get_Value();
				}
			}
			bugReport.get_Dimensions().Add(new UserReportNamedValue("Platform.Version", str + "." + str2));
			m_currentBugReport = bugReport;
			m_isCreatingUserReport = false;
		}

		private void UserReportSubmissionProgress(float uploadProgress, float downloadProgress)
		{
			if (null != m_ui)
			{
				if (uploadProgress < 1f)
				{
					m_ui.SetProgress(uploadProgress, 1);
				}
				else
				{
					m_ui.SetProgress(downloadProgress, 2);
				}
			}
		}

		private void UserReportSubmissionCallback(bool success, UserReport userReport)
		{
			m_isSubmittingUserReport = false;
			m_currentBugReport = null;
			if (success)
			{
				this.get_parent().ClearChildState(0);
			}
			else if (null != m_ui)
			{
				m_ui.ResetForm();
				m_ui.SetError();
			}
		}
	}
}
