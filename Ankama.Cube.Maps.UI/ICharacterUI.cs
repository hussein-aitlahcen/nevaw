using UnityEngine;

namespace Ankama.Cube.Maps.UI
{
	public interface ICharacterUI
	{
		Color color
		{
			get;
			set;
		}

		int sortingOrder
		{
			get;
			set;
		}
	}
}
