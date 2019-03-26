using System;

namespace Ankama.Cube.TEMPFastEnterMatch.Network
{
	public interface IMessageHandler
	{
		Type messageType
		{
			get;
		}

		void Execute(object data);
	}
}
