using Ankama.Cube.Data;
using UnityEngine;

namespace Ankama.Cube.UI.Fight
{
	public sealed class ActionPointCounterRework : MonoBehaviour
	{
		[SerializeField]
		private PointCounterRework m_counter;

		public void SetValue(int value)
		{
			if (null != m_counter)
			{
				m_counter.SetValue(value);
			}
		}

		public void ChangeValue(int value)
		{
			if (null != m_counter)
			{
				m_counter.ChangeValue(value);
			}
		}

		public void ShowPreview(int value, ValueModifier modifier)
		{
		}

		public void HidePreview(bool cancelled)
		{
		}

		public ActionPointCounterRework()
			: this()
		{
		}
	}
}
