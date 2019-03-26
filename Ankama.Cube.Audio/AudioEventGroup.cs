using FMODUnity;
using System;
using UnityEngine;

namespace Ankama.Cube.Audio
{
	[Serializable]
	public struct AudioEventGroup
	{
		[SerializeField]
		private AudioReferenceWithParameters[] m_sounds;

		[SerializeField]
		private float[] m_stats;

		[NonSerialized]
		private int m_index;

		public bool isValid => m_index >= 0;

		public AudioReferenceWithParameters instance => m_sounds[m_index];

		public void Collapse()
		{
			int num = m_sounds.Length;
			if (num <= 1)
			{
				m_index = num - 1;
				return;
			}
			float num2 = Random.get_value();
			float[] stats = m_stats;
			int num3 = num - 1;
			for (int i = 0; i < num3; i++)
			{
				float num4 = stats[i];
				if (num2 < num4)
				{
					m_index = i;
					return;
				}
				num2 -= num4;
			}
			m_index = num3;
		}
	}
}
