using System;

namespace Ankama.Cube.Data
{
	public static class CharacterHeightExtensions
	{
		private const float MinusculeHeight = 0.7f;

		private const float TinyHeight = 0.9f;

		private const float SmallHeight = 1.1f;

		private const float NormalHeight = 1.25f;

		private const float TallHeight = 1.4f;

		private const float HugeHeight = 1.6f;

		public static float GetHeight(this CharacterHeight value)
		{
			switch (value)
			{
			case CharacterHeight.Minuscule:
				return 0.7f;
			case CharacterHeight.Tiny:
				return 0.9f;
			case CharacterHeight.Small:
				return 1.1f;
			case CharacterHeight.Normal:
				return 1.25f;
			case CharacterHeight.Tall:
				return 1.4f;
			case CharacterHeight.Huge:
				return 1.6f;
			default:
				throw new ArgumentOutOfRangeException("value", value, null);
			}
		}
	}
}
