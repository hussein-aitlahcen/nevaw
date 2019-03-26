using System;
using UnityEngine;
using UnityEngine.UI;

public static class UIExtension
{
	public static T WithAlpha<T>(this T v, float a) where T : Graphic
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		Color color = v.get_color();
		color.a = a;
		v.set_color(color);
		return v;
	}

	public unsafe static T WithRGB<T>(this T v, Color color) where T : Graphic
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		color.a = ((IntPtr)(void*)v.get_color()).a;
		v.set_color(color);
		return v;
	}

	public static void Desaturate<T>(this T v, float desaturationFactor) where T : Graphic
	{
		v.get_material().SetFloat("_DesaturationFactor", desaturationFactor);
	}
}
