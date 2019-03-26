using Ankama.Cube.SRP;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Playables;

namespace Ankama.Cube.Animations
{
	public sealed class LightIntensityPlayableBehaviour : PlayableBehaviour
	{
		private readonly AnimationCurve m_curve;

		private float m_originalIntensity;

		[UsedImplicitly]
		public LightIntensityPlayableBehaviour()
			: this()
		{
		}

		public LightIntensityPlayableBehaviour(AnimationCurve curve)
			: this()
		{
			m_curve = curve;
		}

		public override void PrepareFrame(Playable playable, FrameData info)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			if (m_curve != null)
			{
				float num = (float)PlayableExtensions.GetTime<Playable>(playable);
				float num2 = (float)PlayableExtensions.GetDuration<Playable>(playable);
				float num3 = num / num2;
				float num4 = m_curve.Evaluate(num3);
				CubeSRP.SetLightIntensityFactor((object)this, num4);
			}
		}

		public override void OnBehaviourPause(Playable playable, FrameData info)
		{
			CubeSRP.RemoveLightIntensityFactor((object)this);
		}

		public override void OnPlayableDestroy(Playable playable)
		{
			CubeSRP.RemoveLightIntensityFactor((object)this);
		}
	}
}
