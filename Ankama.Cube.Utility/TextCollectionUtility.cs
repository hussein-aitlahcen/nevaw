using Ankama.Cube.Fight;
using Ankama.Cube.Network.Spin2;
using Ankama.Cube.Protocols.ServerProtocol;
using Ankama.Utilities;
using System.Runtime.InteropServices;

namespace Ankama.Cube.Utility
{
	public static class TextCollectionUtility
	{
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct ApplicationKeys
		{
			public const int Quit = 75192;

			public const int LevelShort = 68066;

			public const int DeckBuildingModificationTitle = 56031;

			public const int DeckBuildingDeleteTitle = 52822;

			public const int DeckBuildingSaveConfirm = 57158;

			public const int DeckBuildingCancelConfirm = 179;

			public const int DeckBuildingDeleteConfirm = 76361;

			public const int DeckBuildingNewDeckName = 92537;

			public const int DeckBuildingCloneDeckName = 84166;

			public const int DeckBuildingEditDeck = 65445;

			public const int DeckBuildingCreateDeck = 50970;

			public const int NoValidDeckForThisGod = 64793;

			public const int CurrentDeckName = 66030;

			public const int DefaultDeck = 63105;

			public const int DeckBuildingNoSlotTitle = 4176;

			public const int DeckBuildingNoSlotDesc = 52887;
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct BootKeys
		{
			public const int Ok = 27169;

			public const int Yes = 9912;

			public const int No = 68421;

			public const int Quit = 75192;

			public const int Cancel = 26918;

			public const int Duplicate = 48064;

			public const int EDIT = 38763;

			public const int On = 86438;

			public const int Off = 33654;

			public const int InitializationErrorTitle = 77080;

			public const int ServerMaintenanceExpectedTitle = 75142;

			public const int ServerMaintenance = 85153;

			public const int ServerUnreachable = 34942;

			public const int ZaapIsRequired = 21217;

			public const int ConnectionError = 20267;

			public const int TryingToReconnect = 30166;

			public const int Disconnect = 59515;

			public const int SelectLoginWelcome = 45852;

			public const int SelectLoginYesCreateGuest = 51147;

			public const int SelectLoginPlay = 19445;

			public const int SelectLoginPlayWithAnotherAccount = 84332;

			public const int SelectLoginRegularAccount = 74237;

			public const int SelectLoginTitle = 34597;

			public const int LeaveGameConfirmationMessage = 75182;
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct InitializationFailureKeys
		{
			public static string GetFormattedText(Main.InitializationFailure failure)
			{
				int textKey = GetTextKey(failure);
				if (textKey == 0)
				{
					return $"InitializationFailure: {failure}";
				}
				return RuntimeData.FormattedText(textKey);
			}

			private static int GetTextKey(Main.InitializationFailure failure)
			{
				switch (failure)
				{
				case Main.InitializationFailure.RuntimeDataInitialisation:
					return 35319;
				case Main.InitializationFailure.BootConfigInitialisation:
					return 68683;
				case Main.InitializationFailure.ApplicationConfigInitialisation:
					return 96065;
				case Main.InitializationFailure.UnvalidVersion:
					return 78024;
				case Main.InitializationFailure.ServerStatusError:
					return 26201;
				case Main.InitializationFailure.ServerStatusMaintenance:
					return 38177;
				case Main.InitializationFailure.RuntimeDataLoad:
					return 96151;
				case Main.InitializationFailure.AssetManagerInitialisation:
					return 9761;
				default:
					Log.Warning($"Reason '{failure}' not handled", 102, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Utility\\TextCollectionUtility.cs");
					return 0;
				}
			}
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct AuthenticationErrorKeys
		{
			public static string GetFormattedText(SpinProtocol.ConnectionErrors error)
			{
				int textKey = GetTextKey(error);
				if (textKey == 0)
				{
					return $"SpinError: {error}";
				}
				return RuntimeData.FormattedText(textKey);
			}

			private static int GetTextKey(SpinProtocol.ConnectionErrors error)
			{
				switch (error)
				{
				case SpinProtocol.ConnectionErrors.NoneOrOtherOrUnknown:
					return 31467;
				case SpinProtocol.ConnectionErrors.BadCredentials:
					return 78816;
				case SpinProtocol.ConnectionErrors.InvalidAuthenticationInfo:
					return 96929;
				case SpinProtocol.ConnectionErrors.SubscriptionRequired:
					return 29339;
				case SpinProtocol.ConnectionErrors.AdminRightsRequired:
					return 19831;
				case SpinProtocol.ConnectionErrors.AccountKnonwButBanned:
					return 9342;
				case SpinProtocol.ConnectionErrors.AccountKnonwButBlocked:
					return 89278;
				case SpinProtocol.ConnectionErrors.IpAddressRefused:
					return 82601;
				case SpinProtocol.ConnectionErrors.BetaAccessRequired:
					return 26700;
				case SpinProtocol.ConnectionErrors.ServerTimeout:
					return 20997;
				case SpinProtocol.ConnectionErrors.ServerError:
					return 89386;
				default:
					Log.Warning($"Error '{error}' not handled", 140, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Utility\\TextCollectionUtility.cs");
					return 0;
				}
			}
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct DisconnectionReasonKeys
		{
			public const int DisconnectionWhileAuthentication = 94930;

			public const int ErrorWhileAuthentication = 86405;

			public const int UnknownError = 36698;

			public static string GetFormattedText(DisconnectedByServerEvent.Types.Reason reason)
			{
				int textKey = GetTextKey(reason);
				if (textKey == 0)
				{
					return $"DisconnectionReason: {reason}";
				}
				return RuntimeData.FormattedText(textKey);
			}

			private static int GetTextKey(DisconnectedByServerEvent.Types.Reason reason)
			{
				switch (reason)
				{
				case DisconnectedByServerEvent.Types.Reason.Unknown:
					return 36698;
				case DisconnectedByServerEvent.Types.Reason.Error:
					return 64692;
				case DisconnectedByServerEvent.Types.Reason.ServerIsStopping:
					return 37906;
				case DisconnectedByServerEvent.Types.Reason.UnableToLoadAccount:
					return 86571;
				case DisconnectedByServerEvent.Types.Reason.LoggedInAgainWithSameAccount:
					return 18083;
				default:
					Log.Warning($"Reason '{reason}' not handled", 171, "C:\\BuildAgents\\AgentB\\work\\cub_client_win64_develop\\client\\DofusCube.Unity\\Assets\\Core\\Code\\Utility\\TextCollectionUtility.cs");
					return 0;
				}
			}
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct LoginKeys
		{
			public const int NeedPseudoMessage = 98703;
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct MatchmakingKeys
		{
			public const int OpponentDeclineInvitation = 32703;

			public const int OpponentInviteYou = 22824;

			public const int InvitationFail = 18236;

			public const int AllyLeaveGroup = 34361;

			public const int JoinGroupFail = 80127;

			public const int CreationError = 54306;
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct FightKeys
		{
			public const int ResignConfirmationMessage = 99373;

			public const int LeaveFightConfirmationMessage = 76997;

			public const int OpponentResignMessage = 42185;

			public const int EndOfTurn = 51179;

			public const int OpponentTurn = 85537;

			public const int Allies = 61373;

			public const int Opponents = 30091;
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct FightMessageKeys
		{
			public const int TeamMatchPointEarned_CompanionDeath = 72725;

			public const int TeamMatchPointEarned_FirstVictory = 30898;

			public const int TeamMatchPointEarned_HeroDeath = 583;

			public const int MessageInfo_LowLife = 78839;

			public const int MessageInfo_HeroDead = 12120;

			public const int MessageInfo_FullCompanion = 76419;

			public const int MessageInfo_OwnCompanion = 23018;

			public const int MessageInfo_ReceivedCompanion = 16244;

			public const int BossMode_PointEarn = 45919;
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct EffectKeys
		{
			public const int Damage = 89927;

			public const int Heal = 4272;

			public const int Blindage = 91717;

			public const int Armor = 99288;

			public const int CellDistance = 12480;

			public const int ActionPoints = 96095;

			public const int EarthPoints = 9999;

			public const int AirPoints = 50151;

			public const int FirePoints = 53492;

			public const int WaterPoints = 59689;
		}

		public const string ApplicationTextCollectionName = "Application";

		public const string BootTextCollectionName = "Boot";

		public const string GodsTextCollectionName = "Gods";

		public const string FightTextCollectionName = "Fight";

		public const string EffectsTextCollectionName = "Effects";

		public const string CompanionsTextCollectionName = "Companions";

		public const string MechanismsTextCollectionName = "Mechanisms";

		public const string SpellsTextCollectionName = "Spells";

		public const string SummoningsTextCollectionName = "Summonings";

		public const string WeaponsTextCollectionName = "Weapons";

		public const string UITextCollectionName = "UI";

		public static string GetFormattedText(CastValidity castValidity)
		{
			return RuntimeData.FormattedText($"CastValidity.{castValidity}");
		}
	}
}
