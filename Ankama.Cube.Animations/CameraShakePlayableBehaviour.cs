using Ankama.Cube.Maps;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Playables;

namespace Ankama.Cube.Animations
{
	public sealed class CameraShakePlayableBehaviour : PlayableBehaviour
	{
		private readonly AnimationCurve m_curve;

		[UsedImplicitly]
		public CameraShakePlayableBehaviour()
			: this()
		{
		}

		public CameraShakePlayableBehaviour(AnimationCurve curve)
			: this()
		{
			m_curve = curve;
		}

		public override void PrepareFrame(Playable playable, FrameData info)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			if (m_curve != null)
			{
				CameraHandler current = CameraHandler.current;
				if (!(null == current))
				{
					float num = (float)PlayableExtensions.GetTime<Playable>(playable);
					float num2 = (float)PlayableExtensions.GetDuration<Playable>(playable);
					float num3 = num / num2;
					float value = m_curve.Evaluate(num3);
					current.AddShake(value);
				}
			}
		}
	}
}
