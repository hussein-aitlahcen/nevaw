using Ankama.Cube.Protocols.CommonProtocol;
using Google.Protobuf.Reflection;
using Google.Protobuf.WellKnownTypes;
using System;

namespace Ankama.Cube.Protocols.PlayerProtocol
{
	public static class PlayerProtocolReflection
	{
		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor => descriptor;

		static PlayerProtocolReflection()
		{
			//IL_0261: Unknown result type (might be due to invalid IL or missing references)
			//IL_0267: Expected O, but got Unknown
			//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bf: Expected O, but got Unknown
			//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ff: Expected O, but got Unknown
			//IL_0349: Unknown result type (might be due to invalid IL or missing references)
			//IL_034f: Expected O, but got Unknown
			//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ee: Expected O, but got Unknown
			//IL_041d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0423: Expected O, but got Unknown
			//IL_0445: Unknown result type (might be due to invalid IL or missing references)
			//IL_044b: Expected O, but got Unknown
			//IL_046d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0473: Expected O, but got Unknown
			//IL_0495: Unknown result type (might be due to invalid IL or missing references)
			//IL_049b: Expected O, but got Unknown
			//IL_04c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c8: Expected O, but got Unknown
			//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f5: Expected O, but got Unknown
			//IL_0556: Unknown result type (might be due to invalid IL or missing references)
			//IL_055c: Expected O, but got Unknown
			//IL_055c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0562: Expected O, but got Unknown
			//IL_0562: Unknown result type (might be due to invalid IL or missing references)
			//IL_0568: Expected O, but got Unknown
			//IL_058a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0590: Expected O, but got Unknown
			//IL_05b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b8: Expected O, but got Unknown
			//IL_05da: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e0: Expected O, but got Unknown
			//IL_060a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0610: Expected O, but got Unknown
			//IL_0633: Unknown result type (might be due to invalid IL or missing references)
			//IL_0639: Expected O, but got Unknown
			//IL_065c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0662: Expected O, but got Unknown
			//IL_068d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0693: Expected O, but got Unknown
			//IL_06be: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c4: Expected O, but got Unknown
			//IL_06e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ed: Expected O, but got Unknown
			//IL_06ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f7: Expected O, but got Unknown
			descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String("ChRwbGF5ZXJQcm90b2NvbC5wcm90bxoeZ29vZ2xlL3Byb3RvYnVmL3dyYXBw" + "ZXJzLnByb3RvGhRjb21tb25Qcm90b2NvbC5wcm90byJRChBQbGF5ZXJQdWJs" + "aWNJbmZvEhAKCG5pY2tuYW1lGAEgASgJEgsKA2dvZBgCIAEoBRIQCgh3ZWFw" + "b25JZBgDIAEoBRIMCgRza2luGAQgASgFIpYBCghEZWNrSW5mbxInCgJpZBgB" + "IAEoCzIbLmdvb2dsZS5wcm90b2J1Zi5JbnQzMlZhbHVlEgwKBG5hbWUYAiAB" + "KAkSCwoDZ29kGAMgASgFEg4KBndlYXBvbhgEIAEoBRISCgpjb21wYW5pb25z" + "GAUgAygFEg4KBnNwZWxscxgGIAMoBRISCgpzdW1tb25pbmdzGAcgAygFIkcK" + "CEhlcm9JbmZvEgsKA2dvZBgBIAEoBRIOCgZnZW5kZXIYAiABKAUSEAoId2Vh" + "cG9uSWQYAyABKAUSDAoEc2tpbhgEIAEoBSKBAQoQR2V0UGxheWVyRGF0YUNt" + "ZBITCgthY2NvdW50RGF0YRgBIAEoCBISCgpvY2N1cGF0aW9uGAIgASgIEhAK" + "CGhlcm9EYXRhGAMgASgIEg0KBWRlY2tzGAQgASgIEhIKCmNvbXBhbmlvbnMY" + "BSABKAgSDwoHd2VhcG9ucxgGIAEoCCKRCwoPUGxheWVyRGF0YUV2ZW50Ei0K" + "B2FjY291bnQYASABKAsyHC5QbGF5ZXJEYXRhRXZlbnQuQWNjb3VudERhdGES" + "NQoNY29tcGFuaW9uRGF0YRgCIAEoCzIeLlBsYXllckRhdGFFdmVudC5Db21w" + "YW5pb25EYXRhEjsKEHdlYXBvbkxldmVsc0RhdGEYAyABKAsyIS5QbGF5ZXJE" + "YXRhRXZlbnQuV2VhcG9uTGV2ZWxzRGF0YRJBChNzZWxlY3RlZFdlYXBvbnNE" + "YXRhGAQgASgLMiQuUGxheWVyRGF0YUV2ZW50LlNlbGVjdGVkV2VhcG9uc0Rh" + "dGESKQoFZGVja3MYBSABKAsyGi5QbGF5ZXJEYXRhRXZlbnQuRGVja3NEYXRh" + "EjMKCm9jY3VwYXRpb24YBiABKAsyHy5QbGF5ZXJEYXRhRXZlbnQuT2NjdXBh" + "dGlvbkRhdGESJwoEaGVybxgHIAEoCzIZLlBsYXllckRhdGFFdmVudC5IZXJv" + "RGF0YRJACgxkZWNrc1VwZGF0ZXMYCCABKAsyKi5QbGF5ZXJEYXRhRXZlbnQu" + "RGVja0luY3JlbWVudGFsVXBkYXRlRGF0YRpvCgtBY2NvdW50RGF0YRIMCgRo" + "YXNoGAEgASgFEi4KCG5pY2tOYW1lGAIgASgLMhwuZ29vZ2xlLnByb3RvYnVm" + "LlN0cmluZ1ZhbHVlEg0KBWFkbWluGAMgASgIEhMKC2FjY291bnRUeXBlGAQg" + "ASgJGqcBCglEZWNrc0RhdGESHgoLY3VzdG9tRGVja3MYASADKAsyCS5EZWNr" + "SW5mbxJECg1zZWxlY3RlZERlY2tzGAIgAygLMi0uUGxheWVyRGF0YUV2ZW50" + "LkRlY2tzRGF0YS5TZWxlY3RlZERlY2tzRW50cnkaNAoSU2VsZWN0ZWREZWNr" + "c0VudHJ5EgsKA2tleRgBIAEoBRINCgV2YWx1ZRgCIAEoBToCOAEaIQoOT2Nj" + "dXBhdGlvbkRhdGESDwoHaW5GaWdodBgBIAEoCBojCghIZXJvRGF0YRIXCgRp" + "bmZvGAEgASgLMgkuSGVyb0luZm8aIwoNQ29tcGFuaW9uRGF0YRISCgpjb21w" + "YW5pb25zGAEgAygFGpIBChBXZWFwb25MZXZlbHNEYXRhEkkKDHdlYXBvbkxl" + "dmVscxgBIAMoCzIzLlBsYXllckRhdGFFdmVudC5XZWFwb25MZXZlbHNEYXRh" + "LldlYXBvbkxldmVsc0VudHJ5GjMKEVdlYXBvbkxldmVsc0VudHJ5EgsKA2tl" + "eRgBIAEoBRINCgV2YWx1ZRgCIAEoBToCOAEaoQEKE1NlbGVjdGVkV2VhcG9u" + "c0RhdGESUgoPc2VsZWN0ZWRXZWFwb25zGAEgAygLMjkuUGxheWVyRGF0YUV2" + "ZW50LlNlbGVjdGVkV2VhcG9uc0RhdGEuU2VsZWN0ZWRXZWFwb25zRW50cnka" + "NgoUU2VsZWN0ZWRXZWFwb25zRW50cnkSCwoDa2V5GAEgASgFEg0KBXZhbHVl" + "GAIgASgFOgI4ARqLAgoZRGVja0luY3JlbWVudGFsVXBkYXRlRGF0YRIeCgtk" + "ZWNrVXBkYXRlZBgBIAMoCzIJLkRlY2tJbmZvEhUKDWRlY2tSZW1vdmVkSWQY" + "AiADKAUSXwoVZGVja1NlbGVjdGlvbnNVcGRhdGVkGAMgAygLMkAuUGxheWVy" + "RGF0YUV2ZW50LkRlY2tJbmNyZW1lbnRhbFVwZGF0ZURhdGEuU2VsZWN0ZWRE" + "ZWNrUGVyV2VhcG9uGlYKFVNlbGVjdGVkRGVja1BlcldlYXBvbhIQCgh3ZWFw" + "b25JZBgBIAEoBRIrCgZkZWNrSWQYAiABKAsyGy5nb29nbGUucHJvdG9idWYu" + "SW50MzJWYWx1ZSIbCgxDaGFuZ2VHb2RDbWQSCwoDZ29kGAEgASgFIjIKFENo" + "YW5nZUdvZFJlc3VsdEV2ZW50EhoKBnJlc3VsdBgBIAEoDjIKLkNtZFJlc3Vs" + "dCImCgtTYXZlRGVja0NtZBIXCgRpbmZvGAEgASgLMgkuRGVja0luZm8iQQoT" + "U2F2ZURlY2tSZXN1bHRFdmVudBIaCgZyZXN1bHQYASABKA4yCi5DbWRSZXN1" + "bHQSDgoGZGVja0lkGAIgASgFIhsKDVJlbW92ZURlY2tDbWQSCgoCaWQYASAB" + "KAUiMwoVUmVtb3ZlRGVja1Jlc3VsdEV2ZW50EhoKBnJlc3VsdBgBIAEoDjIK" + "LkNtZFJlc3VsdCJ1ChZTZWxlY3REZWNrQW5kV2VhcG9uQ21kEiYKDXNlbGVj" + "dGVkRGVja3MYASADKAsyDy5TZWxlY3REZWNrSW5mbxIzCg5zZWxlY3RlZFdl" + "YXBvbhgCIAEoCzIbLmdvb2dsZS5wcm90b2J1Zi5JbnQzMlZhbHVlIk8KDlNl" + "bGVjdERlY2tJbmZvEhAKCHdlYXBvbklkGAEgASgFEisKBmRlY2tJZBgCIAEo" + "CzIbLmdvb2dsZS5wcm90b2J1Zi5JbnQzMlZhbHVlIjwKHlNlbGVjdERlY2tB" + "bmRXZWFwb25SZXN1bHRFdmVudBIaCgZyZXN1bHQYASABKA4yCi5DbWRSZXN1" + "bHRCPgoVY29tLmFua2FtYS5jdWJlLnByb3RvqgIkQW5rYW1hLkN1YmUuUHJv" + "dG9jb2xzLlBsYXllclByb3RvY29sYgZwcm90bzM="), (FileDescriptor[])new FileDescriptor[2]
			{
				WrappersReflection.get_Descriptor(),
				CommonProtocolReflection.Descriptor
			}, new GeneratedClrTypeInfo((Type[])null, (GeneratedClrTypeInfo[])new GeneratedClrTypeInfo[14]
			{
				new GeneratedClrTypeInfo(typeof(PlayerPublicInfo), PlayerPublicInfo.Parser, new string[4]
				{
					"Nickname",
					"God",
					"WeaponId",
					"Skin"
				}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
				new GeneratedClrTypeInfo(typeof(DeckInfo), DeckInfo.Parser, new string[7]
				{
					"Id",
					"Name",
					"God",
					"Weapon",
					"Companions",
					"Spells",
					"Summonings"
				}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
				new GeneratedClrTypeInfo(typeof(HeroInfo), HeroInfo.Parser, new string[4]
				{
					"God",
					"Gender",
					"WeaponId",
					"Skin"
				}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
				new GeneratedClrTypeInfo(typeof(GetPlayerDataCmd), GetPlayerDataCmd.Parser, new string[6]
				{
					"AccountData",
					"Occupation",
					"HeroData",
					"Decks",
					"Companions",
					"Weapons"
				}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
				new GeneratedClrTypeInfo(typeof(PlayerDataEvent), PlayerDataEvent.Parser, new string[8]
				{
					"Account",
					"CompanionData",
					"WeaponLevelsData",
					"SelectedWeaponsData",
					"Decks",
					"Occupation",
					"Hero",
					"DecksUpdates"
				}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])new GeneratedClrTypeInfo[8]
				{
					new GeneratedClrTypeInfo(typeof(PlayerDataEvent.Types.AccountData), PlayerDataEvent.Types.AccountData.Parser, new string[4]
					{
						"Hash",
						"NickName",
						"Admin",
						"AccountType"
					}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
					new GeneratedClrTypeInfo(typeof(PlayerDataEvent.Types.DecksData), PlayerDataEvent.Types.DecksData.Parser, new string[2]
					{
						"CustomDecks",
						"SelectedDecks"
					}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])new GeneratedClrTypeInfo[1]),
					new GeneratedClrTypeInfo(typeof(PlayerDataEvent.Types.OccupationData), PlayerDataEvent.Types.OccupationData.Parser, new string[1]
					{
						"InFight"
					}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
					new GeneratedClrTypeInfo(typeof(PlayerDataEvent.Types.HeroData), PlayerDataEvent.Types.HeroData.Parser, new string[1]
					{
						"Info"
					}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
					new GeneratedClrTypeInfo(typeof(PlayerDataEvent.Types.CompanionData), PlayerDataEvent.Types.CompanionData.Parser, new string[1]
					{
						"Companions"
					}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
					new GeneratedClrTypeInfo(typeof(PlayerDataEvent.Types.WeaponLevelsData), PlayerDataEvent.Types.WeaponLevelsData.Parser, new string[1]
					{
						"WeaponLevels"
					}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])new GeneratedClrTypeInfo[1]),
					new GeneratedClrTypeInfo(typeof(PlayerDataEvent.Types.SelectedWeaponsData), PlayerDataEvent.Types.SelectedWeaponsData.Parser, new string[1]
					{
						"SelectedWeapons"
					}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])new GeneratedClrTypeInfo[1]),
					new GeneratedClrTypeInfo(typeof(PlayerDataEvent.Types.DeckIncrementalUpdateData), PlayerDataEvent.Types.DeckIncrementalUpdateData.Parser, new string[3]
					{
						"DeckUpdated",
						"DeckRemovedId",
						"DeckSelectionsUpdated"
					}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])new GeneratedClrTypeInfo[1]
					{
						new GeneratedClrTypeInfo(typeof(PlayerDataEvent.Types.DeckIncrementalUpdateData.Types.SelectedDeckPerWeapon), PlayerDataEvent.Types.DeckIncrementalUpdateData.Types.SelectedDeckPerWeapon.Parser, new string[2]
						{
							"WeaponId",
							"DeckId"
						}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null)
					})
				}),
				new GeneratedClrTypeInfo(typeof(ChangeGodCmd), ChangeGodCmd.Parser, new string[1]
				{
					"God"
				}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
				new GeneratedClrTypeInfo(typeof(ChangeGodResultEvent), ChangeGodResultEvent.Parser, new string[1]
				{
					"Result"
				}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
				new GeneratedClrTypeInfo(typeof(SaveDeckCmd), SaveDeckCmd.Parser, new string[1]
				{
					"Info"
				}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
				new GeneratedClrTypeInfo(typeof(SaveDeckResultEvent), SaveDeckResultEvent.Parser, new string[2]
				{
					"Result",
					"DeckId"
				}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
				new GeneratedClrTypeInfo(typeof(RemoveDeckCmd), RemoveDeckCmd.Parser, new string[1]
				{
					"Id"
				}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
				new GeneratedClrTypeInfo(typeof(RemoveDeckResultEvent), RemoveDeckResultEvent.Parser, new string[1]
				{
					"Result"
				}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
				new GeneratedClrTypeInfo(typeof(SelectDeckAndWeaponCmd), SelectDeckAndWeaponCmd.Parser, new string[2]
				{
					"SelectedDecks",
					"SelectedWeapon"
				}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
				new GeneratedClrTypeInfo(typeof(SelectDeckInfo), SelectDeckInfo.Parser, new string[2]
				{
					"WeaponId",
					"DeckId"
				}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null),
				new GeneratedClrTypeInfo(typeof(SelectDeckAndWeaponResultEvent), SelectDeckAndWeaponResultEvent.Parser, new string[1]
				{
					"Result"
				}, (string[])null, (Type[])null, (GeneratedClrTypeInfo[])null)
			}));
		}
	}
}
