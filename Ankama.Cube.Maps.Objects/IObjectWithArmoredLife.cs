using System.Collections;

namespace Ankama.Cube.Maps.Objects
{
	public interface IObjectWithArmoredLife : ICharacterObject, IMovableIsoObject, IIsoObject
	{
		int life
		{
			get;
		}

		int armor
		{
			get;
		}

		int baseLife
		{
			get;
		}

		void SetArmoredLife(int life, int armor);

		void SetBaseLife(int life);

		IEnumerator PlayHitAnimation();

		IEnumerator PlayLethalHitAnimation();
	}
}
