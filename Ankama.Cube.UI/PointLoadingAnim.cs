using DG.Tweening;
using UnityEngine;

namespace Ankama.Cube.UI
{
	public class PointLoadingAnim : MonoBehaviour
	{
		[SerializeField]
		private PointLoadingAnimData m_datas;

		[SerializeField]
		private CanvasGroup[] m_points;

		private Sequence m_loopSequence;

		private void OnEnable()
		{
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			if (!(m_datas == null))
			{
				m_loopSequence = DOTween.Sequence();
				TweenSettingsExtensions.SetLoops<Sequence>(m_loopSequence, -1);
				for (int i = 0; i < m_points.Length; i++)
				{
					CanvasGroup val = m_points[i];
					TweenSettingsExtensions.Insert(m_loopSequence, (float)i * m_datas.delayBetweenPoints, ShortcutExtensions.DOPunchScale(val.get_transform(), Vector3.get_one() * m_datas.scale, m_datas.duration, m_datas.vibrato, m_datas.elasticity));
				}
				TweenSettingsExtensions.AppendInterval(m_loopSequence, m_datas.delayBetweenLoops);
			}
		}

		private void OnDisable()
		{
			TweenExtensions.Kill(m_loopSequence, false);
			m_loopSequence = null;
		}

		public PointLoadingAnim()
			: this()
		{
		}
	}
}
