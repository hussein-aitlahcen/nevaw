using UnityEngine;

namespace Ankama.Cube.Extensions
{
	public static class MaterialExtensions
	{
		public static bool GetBool(this Material mat, string name)
		{
			return Mathf.RoundToInt(mat.GetFloat(name)) == 1;
		}

		public static bool GetBool(this Material mat, int nameId)
		{
			return Mathf.RoundToInt(mat.GetFloat(nameId)) == 1;
		}

		public static void SetAlphaColor(this Material mat, float value)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			Color color = mat.get_color();
			color.a = value;
			mat.set_color(color);
		}
	}
}
