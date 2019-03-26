using JetBrains.Annotations;
using UnityEngine;

namespace Ankama.Cube.Maps.Objects
{
	public interface IIsoObject
	{
		[PublicAPI]
		GameObject gameObject
		{
			get;
		}

		[PublicAPI]
		Transform transform
		{
			get;
		}

		[PublicAPI]
		CellObject cellObject
		{
			get;
		}
	}
}
