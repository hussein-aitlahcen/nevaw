using Ankama.Cube.Data;
using Ankama.Cube.Maps.Feedbacks;
using System.Collections;

namespace Ankama.Cube.Maps.Objects
{
	public interface IObjectWithCounterEffects
	{
		IEnumerator InitializeFloatingCounterEffect(FloatingCounterEffect floatingCounterEffect, int value);

		IEnumerator ChangeFloatingCounterEffect(FloatingCounterEffect floatingCounterEffect);

		IEnumerator RemoveFloatingCounterEffect();

		void ClearFloatingCounterEffect();

		FloatingCounterFeedback GetCurrentFloatingCounterFeedback();
	}
}
