using UnityEngine;

namespace Ankama.Cube.Animations
{
	public interface ITimelineContextProvider
	{
		Object GetTimelineBinding();

		ITimelineContext GetTimelineContext();
	}
}
