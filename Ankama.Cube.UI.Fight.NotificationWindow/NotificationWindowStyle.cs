using DG.Tweening;
using UnityEngine;

namespace Ankama.Cube.UI.Fight.NotificationWindow
{
	[CreateAssetMenu(menuName = "Waven/UI/NotificationWindow/NotificationWindowStyle", order = 1000)]
	public class NotificationWindowStyle : ScriptableObject
	{
		[SerializeField]
		public float fadeInDuration = 0.3f;

		[SerializeField]
		public Ease fadeInEase = 19;

		[SerializeField]
		public Ease scaleFadeInEase = 27;

		[SerializeField]
		public float fadeOutDuration = 0.3f;

		[SerializeField]
		public Ease fadeOutEase = 19;

		[SerializeField]
		public float displayDuration = 5f;

		public NotificationWindowStyle()
			: this()
		{
		}//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)

	}
}
