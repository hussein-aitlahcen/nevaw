using UnityEngine;

namespace Ankama.Cube.Render
{
	public static class DitheringUtility
	{
		public enum Mode
		{
			Floor,
			Ceil
		}

		public static float GetDitheringLevel(float value, Mode mode)
		{
			if (mode == Mode.Ceil)
			{
				return Mathf.Ceil(value * 15f) / 16f;
			}
			return Mathf.Floor(value * 15f) / 16f;
		}
	}
}
