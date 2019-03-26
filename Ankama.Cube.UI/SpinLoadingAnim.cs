using System;
using UnityEngine;

namespace Ankama.Cube.UI
{
	public class SpinLoadingAnim : MonoBehaviour
	{
		[SerializeField]
		private SpinLoadingAnimData m_datas;

		private float m_time;

		private unsafe void Update()
		{
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			if (!(m_datas == null))
			{
				m_time += Time.get_deltaTime() * m_datas.speed;
				float num = ((m_datas.offsetType == SpinLoadingAnimData.OffsetType.Value) ? m_datas.offset : (((IntPtr)(void*)this.get_transform().get_position()).y + ((IntPtr)(void*)this.get_transform().get_position()).x)) + (float)Mathf.FloorToInt(m_time / m_datas.step) * m_datas.step;
				this.get_transform().set_localRotation(Quaternion.Euler(0f, 0f, num));
			}
		}

		public SpinLoadingAnim()
			: this()
		{
		}
	}
}
