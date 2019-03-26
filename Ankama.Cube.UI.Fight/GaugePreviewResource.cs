using DG.Tweening;
using UnityEngine;

namespace Ankama.Cube.UI.Fight
{
	public class GaugePreviewResource : ScriptableObject
	{
		[Header("Highlight")]
		[SerializeField]
		private bool m_highlightEnabled = true;

		[SerializeField]
		private int m_loopCount = 1;

		[SerializeField]
		private float m_highlightPunch = 0.1f;

		[SerializeField]
		private float m_highlightDuration = 0.3f;

		[SerializeField]
		private int m_highlightVibrato = 1;

		[SerializeField]
		[Range(0f, 1f)]
		private float m_highlightElasticity = 0.1f;

		[Header("Text Value Modification")]
		[SerializeField]
		private bool m_textValueEnabled = true;

		[SerializeField]
		private float m_duration;

		[SerializeField]
		private Ease m_ease;

		public bool highlightEnabled => m_highlightEnabled;

		public int highlightLoopCount => m_loopCount;

		public float highlightPunch => m_highlightPunch;

		public float highlightDuration => m_highlightDuration;

		public int highlightVibrato => m_highlightVibrato;

		public float highlightElasticity => m_highlightElasticity;

		public bool displayText => m_textValueEnabled;

		public float duration => m_duration;

		public Ease ease => m_ease;

		public GaugePreviewResource()
			: this()
		{
		}
	}
}
