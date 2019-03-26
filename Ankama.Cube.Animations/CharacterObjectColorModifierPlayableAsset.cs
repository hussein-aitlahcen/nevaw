using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Ankama.Cube.Animations
{
	public sealed class CharacterObjectColorModifierPlayableAsset : PlayableAsset, ITimelineClipAsset
	{
		[SerializeField]
		private Color m_startColor = Color.get_white();

		[SerializeField]
		private Color m_endColor = Color.get_white();

		public Color startColor => m_startColor;

		public Color endColor => m_endColor;

		public ClipCaps clipCaps
		{
			get;
		} = 2;


		public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return Playable.get_Null();
		}

		public CharacterObjectColorModifierPlayableAsset()
			: this()
		{
		}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)

	}
}
