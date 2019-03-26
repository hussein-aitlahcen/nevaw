using System.Collections;

namespace Ankama.Cube.Maps.Objects
{
	public interface IObjectWithActivationAnimation : ICharacterObject, IMovableIsoObject, IIsoObject
	{
		IEnumerator PlayActivationAnimation();
	}
}
