using Ankama.Utilities;
using System;
using UnityEngine;

namespace Ankama.Cube
{
	public static class Device
	{
		public enum Type
		{
			PC,
			Mobile,
			Tablet
		}

		public delegate void ScreenSateChangedDelegate();

		public static readonly Type currentType;

		private static float s_dpi;

		private static FullScreenMode s_fullScreenMode;

		private static Vector2Int s_screenSize;

		public static bool isStandalone => currentType == Type.PC;

		public static bool isMobile => currentType == Type.Mobile;

		public static bool isTablet => currentType == Type.Tablet;

		public static bool isMobileOrTablet
		{
			get
			{
				switch (currentType)
				{
				case Type.PC:
					return false;
				case Type.Mobile:
				case Type.Tablet:
					return true;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		public static bool fullScreen
		{
			get
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0008: Invalid comparison between Unknown and I4
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_000e: Invalid comparison between Unknown and I4
				FullScreenMode val = s_fullScreenMode;
				if ((int)val > 1)
				{
					if (val - 2 <= 1)
					{
						return false;
					}
					throw new ArgumentOutOfRangeException();
				}
				return true;
			}
		}

		public static float dpi => s_dpi;

		public static event ScreenSateChangedDelegate ScreenStateChanged;

		static Device()
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			if (Application.get_isMobilePlatform())
			{
				float dpi = Screen.get_dpi();
				if (dpi <= 0f)
				{
					currentType = Type.Mobile;
					Log.Warning($"Could not retrieve DPI of the device, defaulted to {currentType}.", 104, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Device.cs");
				}
				else
				{
					Resolution currentResolution = Screen.get_currentResolution();
					int width = currentResolution.get_width();
					int height = currentResolution.get_height();
					float num = Mathf.Sqrt((float)(width * width + height * height)) / dpi;
					if (num < 7f)
					{
						currentType = Type.Mobile;
					}
					else
					{
						currentType = Type.Tablet;
					}
					Log.Info($"Auto-detected device type: {currentType}\n - physical diagonal: {num:n2} in\n - resolution: {width}x{height}\n - dpi: {dpi:n2}", 124, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Device.cs");
				}
			}
			else
			{
				currentType = Type.PC;
			}
			s_dpi = Screen.get_dpi();
			s_fullScreenMode = Screen.get_fullScreenMode();
			s_screenSize = new Vector2Int(Screen.get_width(), Screen.get_height());
		}

		public static void CheckScreenStateChanged()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			bool flag = false;
			FullScreenMode fullScreenMode = Screen.get_fullScreenMode();
			if (fullScreenMode != s_fullScreenMode)
			{
				s_fullScreenMode = fullScreenMode;
				flag = true;
			}
			float dpi = Screen.get_dpi();
			if (!Mathf.Approximately(dpi, s_dpi))
			{
				s_dpi = dpi;
				flag = true;
			}
			Vector2Int val = default(Vector2Int);
			val._002Ector(Screen.get_width(), Screen.get_height());
			if (val != s_screenSize)
			{
				s_screenSize = val;
				flag = true;
			}
			if (flag)
			{
				Log.Info($"Screen State Changed: fullscreen mode: {fullScreenMode}, size: {val.get_x()}x{val.get_y()}, DPI: {dpi}", 175, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Device.cs");
				Device.ScreenStateChanged?.Invoke();
			}
		}

		public static void LogInfo()
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Invalid comparison between Unknown and I4
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Invalid comparison between Unknown and I4
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Invalid comparison between Unknown and I4
			string deviceModel = SystemInfo.get_deviceModel();
			string graphicsDeviceVendor = SystemInfo.get_graphicsDeviceVendor();
			string graphicsDeviceName = SystemInfo.get_graphicsDeviceName();
			string text = StringExtensions.Contains(graphicsDeviceName, graphicsDeviceVendor, StringComparison.OrdinalIgnoreCase) ? graphicsDeviceName : (graphicsDeviceVendor + " " + graphicsDeviceName);
			if ((int)SystemInfo.get_deviceType() == 3)
			{
				text += $" ({SystemInfo.get_graphicsDeviceVendorID()}/{SystemInfo.get_graphicsDeviceID()})";
			}
			string graphicsDeviceVersion = SystemInfo.get_graphicsDeviceVersion();
			string text2 = $"{SystemInfo.get_graphicsMemorySize()} MB";
			Log.Info("[Device Information]\n - model: " + deviceModel + "\n - graphics device: " + text + "\n - graphics driver: " + graphicsDeviceVersion + "\n - graphics memory: " + text2, 204, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Device.cs");
			string operatingSystem = SystemInfo.get_operatingSystem();
			RuntimePlatform platform = Application.get_platform();
			int num = ((int)platform != 8 && (int)platform != 17) ? SystemInfo.get_processorFrequency() : 0;
			string processorType = SystemInfo.get_processorType();
			string text3 = (num == 0 || StringExtensions.Contains(processorType, "@", StringComparison.Ordinal)) ? processorType : $"{processorType} @ {num} MHz";
			int processorCount = SystemInfo.get_processorCount();
			if (processorCount > 0)
			{
				text3 += $" ({processorCount} logical cores)";
			}
			string text4 = (SystemInfo.get_systemMemorySize() > 0) ? $"{SystemInfo.get_systemMemorySize()} MB" : "n/a";
			Log.Info("[System Information]\n - os: " + operatingSystem + "\n - processor: " + text3 + "\n - memory: " + text4, 241, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Device.cs");
		}
	}
}
