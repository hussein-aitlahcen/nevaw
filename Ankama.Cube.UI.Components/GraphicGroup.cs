using UnityEngine;
using UnityEngine.UI;

namespace Ankama.Cube.UI.Components
{
	[ExecuteInEditMode]
	public class GraphicGroup : MonoBehaviour
	{
		[SerializeField]
		private Graphic[] m_children;

		[SerializeField]
		private Color m_color;

		public Color color
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return m_color;
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0027: Unknown result type (might be due to invalid IL or missing references)
				m_color = value;
				for (int i = 0; i < m_children.Length; i++)
				{
					Graphic val = m_children[i];
					if (val != this && val != null)
					{
						val.set_color(value);
					}
				}
			}
		}

		public float alpha
		{
			get
			{
				return m_color.a;
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				Color color = m_color;
				color.a = value;
				this.color = color;
			}
		}

		private void Start()
		{
			m_children = this.GetComponentsInChildren<Graphic>(true);
		}

		private void OnTransformChildrenChanged()
		{
			m_children = this.GetComponentsInChildren<Graphic>(true);
		}

		public GraphicGroup()
			: this()
		{
		}
	}
}
