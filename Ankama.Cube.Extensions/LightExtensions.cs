using UnityEngine;

namespace Ankama.Cube.Extensions
{
	public static class LightExtensions
	{
		public static float GetSortingWeight(this Light l)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Invalid comparison between Unknown and I4
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Invalid comparison between Unknown and I4
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			float num = 0f + (float)(((int)l.get_type() == 1) ? 1000 : 0) + (float)(((int)l.get_renderMode() != 2) ? 100 : 0) + (float)(((int)l.get_shadows() != 0) ? 10 : 0);
			Color val = l.get_color();
			val = val.get_linear();
			return num + val.get_grayscale() * l.get_intensity();
		}

		public static bool HasSoftShadow(this Light l)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Invalid comparison between Unknown and I4
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Invalid comparison between Unknown and I4
			if ((int)QualitySettings.get_shadows() == 2)
			{
				return (int)l.get_shadows() == 2;
			}
			return false;
		}
	}
}
