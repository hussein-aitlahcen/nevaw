using Ankama.Cube.Audio;
using Ankama.Cube.Utility;
using JetBrains.Annotations;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Ankama.Cube.Player
{
	[PublicAPI]
	public static class PlayerPreferences
	{
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct PrefKeys
		{
			public const string AuthenticationUseGuest = "Waven.Authentication.UseGuest";

			public const string AuthenticationAutoLogin = "Waven.Authentication.AutoLogin";

			public const string AuthenticationLastServer = "Waven.Authentication.LastServer";

			public const string AuthenticationLastLogin = "Waven.Authentication.LastLogin";

			public const string AuthenticationLastPassword = "Waven.Authentication.LastPassword";

			public const string AuthenticationGuestLogin = "Waven.Authentication.GuestLogin";

			public const string AuthenticationGuestPassword = "Waven.Authentication.GuestPassword";

			public const string AuthenticationRememberPassword = "Waven.Authentication.RememberPassword";

			public const string GameStartState = "Waven.Game.StartState";

			public const string GameSelectedSquadIdentifier = "Waven.Game.SelectedSquadIdentifier";

			public const string GamePreferredSquad = "Waven.Game.PreferredSquad";

			public const string GamePreferredFightDef = "Waven.Game.PreferredFightDef";

			public const string PreferredGraphicQualityIndex = "Waven.Graphics.Alpha.PreferredGraphicQualityIndexV2";

			public const string AudioMasterVolume = "Waven.Audio.MasterVolume";

			public const string AudioMusicVolume = "Waven.Audio.MusicVolume";

			public const string AudioFxVolume = "Waven.Audio.FxVolume";

			public const string AudioUiVolume = "Waven.Audio.UiVolume";
		}

		private const string EncryptionKey = "a585c7e0-8291-4f8e-b1c8-a3dc05ca0b49";

		private static bool s_dirty;

		private static bool s_useGuest;

		private static bool s_autoLogin;

		private static string s_lastServer;

		private static string s_lastLogin;

		private static string s_lastPassword;

		private static string s_guestLogin;

		private static string s_guestPassword;

		private static bool s_rememberPassword;

		private static int s_startState;

		private static int s_preferredFightDef;

		private static int s_graphicPresetIndex;

		private static float s_audioMasterVolume;

		private static float s_audioMusicVolume;

		private static float s_audioFxVolume;

		private static float s_audioUiVolume;

		public static bool useGuest
		{
			get
			{
				return s_useGuest;
			}
			set
			{
				UpdatePref(ref s_useGuest, value);
			}
		}

		public static bool autoLogin
		{
			get
			{
				return s_autoLogin;
			}
			set
			{
				UpdatePref(ref s_autoLogin, value);
			}
		}

		public static string lastServer
		{
			get
			{
				return s_lastServer;
			}
			set
			{
				UpdatePref(ref s_lastServer, value);
			}
		}

		public static string lastLogin
		{
			get
			{
				return Decrypt(s_lastLogin);
			}
			set
			{
				EncryptAndUpdate(value, ref s_lastLogin);
			}
		}

		public static string lastPassword
		{
			get
			{
				return Decrypt(s_lastPassword);
			}
			set
			{
				EncryptAndUpdate(value, ref s_lastPassword);
			}
		}

		public static string guestLogin
		{
			get
			{
				return Decrypt(s_guestLogin);
			}
			set
			{
				EncryptAndUpdate(value, ref s_guestLogin);
			}
		}

		public static string guestPassword
		{
			get
			{
				return Decrypt(s_guestPassword);
			}
			set
			{
				EncryptAndUpdate(value, ref s_guestPassword);
			}
		}

		public static bool rememberPassword
		{
			get
			{
				return s_rememberPassword;
			}
			set
			{
				UpdatePref(ref s_rememberPassword, value);
			}
		}

		public static int startState
		{
			get
			{
				return s_startState;
			}
			set
			{
				UpdatePref(ref s_startState, value);
			}
		}

		public static int preferredFightDef
		{
			get
			{
				return s_preferredFightDef;
			}
			set
			{
				UpdatePref(ref s_preferredFightDef, value);
			}
		}

		public static int graphicPresetIndex
		{
			get
			{
				return s_graphicPresetIndex;
			}
			set
			{
				UpdatePref(ref s_graphicPresetIndex, value);
			}
		}

		public static float audioMasterVolume
		{
			get
			{
				return s_audioMasterVolume;
			}
			set
			{
				UpdatePref(ref s_audioMasterVolume, value);
			}
		}

		public static float audioMusicVolume
		{
			get
			{
				return s_audioMusicVolume;
			}
			set
			{
				UpdatePref(ref s_audioMusicVolume, value);
			}
		}

		public static float audioFxVolume
		{
			get
			{
				return s_audioFxVolume;
			}
			set
			{
				UpdatePref(ref s_audioFxVolume, value);
			}
		}

		public static float audioUiVolume
		{
			get
			{
				return s_audioUiVolume;
			}
			set
			{
				UpdatePref(ref s_audioUiVolume, value);
			}
		}

		public static void Load()
		{
			s_useGuest = (PlayerPrefs.GetInt("Waven.Authentication.UseGuest", 1) != 0);
			s_autoLogin = (PlayerPrefs.GetInt("Waven.Authentication.AutoLogin", 0) != 0);
			s_lastServer = PlayerPrefs.GetString("Waven.Authentication.LastServer", "localhost");
			s_lastLogin = PlayerPrefs.GetString("Waven.Authentication.LastLogin", string.Empty);
			s_lastPassword = PlayerPrefs.GetString("Waven.Authentication.LastPassword", string.Empty);
			s_guestLogin = PlayerPrefs.GetString("Waven.Authentication.GuestLogin", string.Empty);
			s_guestPassword = PlayerPrefs.GetString("Waven.Authentication.GuestPassword", string.Empty);
			s_rememberPassword = (PlayerPrefs.GetInt("Waven.Authentication.RememberPassword", 0) != 0);
			s_startState = PlayerPrefs.GetInt("Waven.Game.StartState", 1);
			s_preferredFightDef = PlayerPrefs.GetInt("Waven.Game.PreferredFightDef", -1);
			s_graphicPresetIndex = PlayerPrefs.GetInt("Waven.Graphics.Alpha.PreferredGraphicQualityIndexV2", -1);
			s_audioMasterVolume = PlayerPrefs.GetFloat("Waven.Audio.MasterVolume", 1f);
			s_audioMusicVolume = PlayerPrefs.GetFloat("Waven.Audio.MusicVolume", 1f);
			s_audioFxVolume = PlayerPrefs.GetFloat("Waven.Audio.FxVolume", 1f);
			s_audioUiVolume = PlayerPrefs.GetFloat("Waven.Audio.UiVolume", 1f);
		}

		public static void Save()
		{
			if (s_dirty)
			{
				PlayerPrefs.SetInt("Waven.Authentication.UseGuest", s_useGuest ? 1 : 0);
				PlayerPrefs.SetInt("Waven.Authentication.AutoLogin", s_autoLogin ? 1 : 0);
				PlayerPrefs.SetString("Waven.Authentication.LastServer", s_lastServer ?? "localhost");
				PlayerPrefs.SetString("Waven.Authentication.LastLogin", s_lastLogin);
				PlayerPrefs.SetString("Waven.Authentication.LastPassword", s_lastPassword);
				PlayerPrefs.SetString("Waven.Authentication.GuestLogin", s_guestLogin);
				PlayerPrefs.SetString("Waven.Authentication.GuestPassword", s_guestPassword);
				PlayerPrefs.SetInt("Waven.Authentication.RememberPassword", s_rememberPassword ? 1 : 0);
				PlayerPrefs.SetInt("Waven.Game.StartState", s_startState);
				PlayerPrefs.SetInt("Waven.Game.PreferredFightDef", s_preferredFightDef);
				PlayerPrefs.SetInt("Waven.Graphics.Alpha.PreferredGraphicQualityIndexV2", s_graphicPresetIndex);
				PlayerPrefs.SetFloat("Waven.Audio.MasterVolume", s_audioMasterVolume);
				PlayerPrefs.SetFloat("Waven.Audio.MusicVolume", s_audioMusicVolume);
				PlayerPrefs.SetFloat("Waven.Audio.FxVolume", s_audioFxVolume);
				PlayerPrefs.SetFloat("Waven.Audio.UiVolume", s_audioUiVolume);
			}
		}

		public static void InitializeAudioPreference()
		{
			AudioManager.SetVolume(AudioBusIdentifier.Master, audioMasterVolume);
			AudioManager.SetVolume(AudioBusIdentifier.Music, audioMusicVolume);
			AudioManager.SetVolume(AudioBusIdentifier.SFX, audioFxVolume);
			AudioManager.SetVolume(AudioBusIdentifier.UI, audioUiVolume);
		}

		private static void UpdatePref(ref string value, string update)
		{
			if (!string.Equals(update, value))
			{
				value = update;
				s_dirty = true;
			}
		}

		private static void UpdatePref(ref float value, float update)
		{
			if (update != value)
			{
				value = update;
				s_dirty = true;
			}
		}

		private static void UpdatePref(ref int value, int update)
		{
			if (update != value)
			{
				value = update;
				s_dirty = true;
			}
		}

		private static void UpdatePref(ref bool value, bool update)
		{
			if (update != value)
			{
				value = update;
				s_dirty = true;
			}
		}

		private static string Decrypt(string word)
		{
			if (!string.IsNullOrEmpty(word))
			{
				return StringCipher.Decrypt(word, "a585c7e0-8291-4f8e-b1c8-a3dc05ca0b49");
			}
			return word;
		}

		private static void EncryptAndUpdate(string value, ref string previousValue)
		{
			string text = StringCipher.Encrypt(value, "a585c7e0-8291-4f8e-b1c8-a3dc05ca0b49");
			if (!string.Equals(text, previousValue))
			{
				previousValue = text;
				s_dirty = true;
			}
		}
	}
}
