using Ankama.Cube.Network.Spin2;
using Com.Ankama.Haapi.Swagger.Model;
using System;

namespace Ankama.Cube.Configuration
{
	public static class HaapiHelper
	{
		public static SpinConnectionError From(ErrorAccountLogin error)
		{
			if (Enum.TryParse(error.get_Reason(), out ErrorAccountLoginCode result))
			{
				switch (result)
				{
				case ErrorAccountLoginCode.FAILED:
					return new SpinConnectionError(SpinProtocol.ConnectionErrors.BadCredentials);
				case ErrorAccountLoginCode.BAN:
					return new SpinConnectionError(SpinProtocol.ConnectionErrors.AccountKnonwButBanned);
				case ErrorAccountLoginCode.LOCKED:
					return new SpinConnectionError(SpinProtocol.ConnectionErrors.AccountKnonwButBlocked);
				case ErrorAccountLoginCode.DELETED:
				case ErrorAccountLoginCode.NOACCOUNT:
					return new SpinConnectionError(SpinProtocol.ConnectionErrors.NoneOrOtherOrUnknown);
				case ErrorAccountLoginCode.BLACKLIST:
				case ErrorAccountLoginCode.BRUTEFORCE:
					return new SpinConnectionError(SpinProtocol.ConnectionErrors.IpAddressRefused);
				case ErrorAccountLoginCode.BETACLOSED:
					return new SpinConnectionError(SpinProtocol.ConnectionErrors.BetaAccessRequired);
				case ErrorAccountLoginCode.OTPTIMEFAILED:
				case ErrorAccountLoginCode.SECURITYCARD:
					return new SpinConnectionError(SpinProtocol.ConnectionErrors.InvalidAuthenticationInfo);
				case ErrorAccountLoginCode.ACCOUNT_LINKED:
				case ErrorAccountLoginCode.ACCOUNT_INVALID:
				case ErrorAccountLoginCode.ACCOUNT_SHIELDED:
				case ErrorAccountLoginCode.ACCOUNT_NO_CERTIFY:
					return new SpinConnectionError(SpinProtocol.ConnectionErrors.NoneOrOtherOrUnknown);
				case ErrorAccountLoginCode.RESETANKAMA:
				case ErrorAccountLoginCode.PARTNER:
				case ErrorAccountLoginCode.MAILNOVALID:
					return new SpinConnectionError(SpinProtocol.ConnectionErrors.NoneOrOtherOrUnknown);
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
			return null;
		}
	}
}
