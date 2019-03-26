using DG.Tweening;

namespace Ankama.Cube.Extensions
{
	public static class DOTweenExtensions
	{
		public static bool HasOvershootParam(this Ease ease)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Invalid comparison between Unknown and I4
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Invalid comparison between Unknown and I4
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Invalid comparison between Unknown and I4
			if ((int)ease != 27 && (int)ease != 26)
			{
				return (int)ease == 28;
			}
			return true;
		}

		public static bool HasAmplitudeParam(this Ease ease)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Invalid comparison between Unknown and I4
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Invalid comparison between Unknown and I4
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Invalid comparison between Unknown and I4
			if ((int)ease != 23 && (int)ease != 24)
			{
				return (int)ease == 25;
			}
			return true;
		}
	}
}
