using System;

namespace Ankama.Cube.Network
{
	public interface IAccountLoadingHandler : IDisposable
	{
		event Action OnAccountLoaded;

		void LoadAccount();
	}
}
