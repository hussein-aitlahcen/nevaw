using System.Collections;

namespace Ankama.Cube.Maps.Objects
{
	public interface IBossEvolution
	{
		IEnumerator PlayLevelChangeAnim(int valueBefore, int valueAfter);

		void OnLevelChangeAnimEvent();
	}
}
