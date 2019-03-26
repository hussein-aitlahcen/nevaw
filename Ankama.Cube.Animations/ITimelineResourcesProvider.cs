using System.Collections;

namespace Ankama.Cube.Animations
{
	public interface ITimelineResourcesProvider
	{
		IEnumerator LoadResources();

		void UnloadResources();
	}
}
