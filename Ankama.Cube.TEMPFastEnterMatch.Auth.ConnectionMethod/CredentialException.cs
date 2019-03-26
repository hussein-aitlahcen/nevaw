using System;

namespace Ankama.Cube.TEMPFastEnterMatch.Auth.ConnectionMethod
{
	public class CredentialException : Exception
	{
		public CredentialException(string message)
			: base(message)
		{
		}
	}
}
