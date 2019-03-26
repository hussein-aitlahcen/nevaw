using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Fight
{
	public sealed class CardNumberCounterRework : MonoBehaviour
	{
		[Header("Links")]
		[SerializeField]
		private UISpriteTextRenderer m_text;

		private int m_count;

		public IEnumerator Increment()
		{
			m_count++;
			m_text.text = $"x{m_count}";
			yield break;
		}

		public IEnumerator Decrement()
		{
			m_count--;
			m_text.text = $"x{m_count}";
			yield break;
		}

		public CardNumberCounterRework()
			: this()
		{
		}
	}
}
