using System;

namespace Ankama.Cube.Data
{
	[Serializable]
	public enum ListComparison
	{
		ContainsAll = 1,
		ContainsAtLeastOne,
		ContainsExactlyOne,
		ContainsNone
	}
}
