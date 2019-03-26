using Ankama.Cube.UI.Components;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ankama.Cube.UI
{
	public class OptionSlider : MonoBehaviour
	{
		[SerializeField]
		private Slider m_slider;

		[SerializeField]
		private RawTextField m_text;

		private unsafe void OnEnable()
		{
			SetTextValue(m_slider.get_value());
			m_slider.get_onValueChanged().AddListener(new UnityAction<float>((object)this, (IntPtr)(void*)/*OpCode not supported: LdFtn*/));
		}

		private void OnValueChanged(float value)
		{
			SetTextValue(value);
		}

		private void SetTextValue(float value)
		{
			value *= 100f;
			string text = null;
			text = ((value <= 0.1f) ? RuntimeData.FormattedText(33654) : ((!(value < 1f) || !(value > 0.1f)) ? Mathf.RoundToInt(value).ToString() : "1"));
			m_text.SetText(text);
		}

		public OptionSlider()
			: this()
		{
		}
	}
}
