using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using UnityEngine;

namespace Ankama.Cube.UI.Components
{
	public static class DOTweenExtensions
	{
		public unsafe static Tweener DOColor(this AbstractTextField target, Color endValue, float duration)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			_003C_003Ec__DisplayClass0_0 _003C_003Ec__DisplayClass0_;
			return TweenSettingsExtensions.SetTarget<TweenerCore<Color, Color, ColorOptions>>(DOTween.To(new DOGetter<Color>((object)_003C_003Ec__DisplayClass0_, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), new DOSetter<Color>((object)_003C_003Ec__DisplayClass0_, (IntPtr)(void*)/*OpCode not supported: LdFtn*/), endValue, duration), (object)target);
		}
	}
}
