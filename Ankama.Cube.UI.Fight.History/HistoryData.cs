using DG.Tweening;
using UnityEngine;

namespace Ankama.Cube.UI.Fight.History
{
	public class HistoryData : ScriptableObject
	{
		[Header("Layout")]
		[SerializeField]
		public int maxElements = 5;

		[SerializeField]
		public int maxHiddableElements = 2;

		[SerializeField]
		public float spacing = 5f;

		[SerializeField]
		public float positionTweenDuration = 0.2f;

		[SerializeField]
		public Ease postitionTweenEase = 1;

		[SerializeField]
		public float outAlphaTweenDuration = 0.1f;

		[SerializeField]
		public Vector3 inScalePunchValue = Vector3.get_one();

		[SerializeField]
		public float inScalePunchDuration = 0.1f;

		[Header("Element")]
		[SerializeField]
		public Sprite playerBg;

		[SerializeField]
		public Sprite opponentBg;

		[SerializeField]
		public Color elementsDepthColor = Color.get_white();

		[SerializeField]
		public float elementsOverDuration = 0.1f;

		[SerializeField]
		public float elementsOverOffset = 20f;

		[SerializeField]
		public Ease elementsOverEase = 1;

		public HistoryData()
			: this()
		{
		}//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)

	}
}
