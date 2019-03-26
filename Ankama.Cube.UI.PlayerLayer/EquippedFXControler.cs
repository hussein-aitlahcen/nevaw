using Ankama.Cube.Data;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ankama.Cube.UI.PlayerLayer
{
	public class EquippedFXControler : MonoBehaviour
	{
		[Serializable]
		public struct ColorByElement
		{
			[SerializeField]
			public Element Element;

			public Color Color;
		}

		[SerializeField]
		private List<ColorByElement> m_elementToColor;

		[SerializeField]
		private List<Image> m_ImageToColor;

		public void SetEquipped(bool b)
		{
			this.get_gameObject().SetActive(b);
			if (b)
			{
				foreach (Image item in m_ImageToColor)
				{
					DOTweenModuleUI.DOFade(item, 1f, 0.1f);
				}
			}
		}

		public void SetElement(Element element)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			Color color = Color.get_white();
			foreach (ColorByElement item in m_elementToColor)
			{
				if (item.Element == element)
				{
					color = item.Color;
				}
			}
			foreach (Image item2 in m_ImageToColor)
			{
				item2.set_color(color);
			}
		}

		public EquippedFXControler()
			: this()
		{
		}
	}
}
