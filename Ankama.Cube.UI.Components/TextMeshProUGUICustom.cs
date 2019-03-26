using TMPro;
using UnityEngine;

namespace Ankama.Cube.UI.Components
{
	[AddComponentMenu("")]
	public class TextMeshProUGUICustom : TextMeshProUGUI
	{
		protected override void Awake()
		{
			this.set_text("");
			this.Awake();
		}

		protected override void OnDestroy()
		{
			this.OnDestroy();
			TMP_SubMeshUI[] subTextObjects = base.m_subTextObjects;
			int num = subTextObjects.Length;
			for (int i = 1; i < num; i++)
			{
				TMP_SubMeshUI val = subTextObjects[i];
				if (!(null == val))
				{
					Object.Destroy(val.get_gameObject());
					subTextObjects[i] = null;
					continue;
				}
				break;
			}
		}

		public TextMeshProUGUICustom()
			: this()
		{
		}
	}
}
