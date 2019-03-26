using System.Collections;

namespace Ankama.Cube.Maps.Objects
{
	public interface IObjectWithActivation : ICharacterObject, IMovableIsoObject, IIsoObject
	{
		IEnumerator ActivatedByAlly();

		IEnumerator ActivatedByOpponent();

		IEnumerator WaitForActivationEnd();

		void PlayDetectionAnimation();
	}
}
