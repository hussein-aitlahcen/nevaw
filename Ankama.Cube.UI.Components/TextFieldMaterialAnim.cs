using UnityEngine;

namespace Ankama.Cube.UI.Components
{
	[RequireComponent(typeof(AbstractTextField))]
	[ExecuteInEditMode]
	public class TextFieldMaterialAnim : MonoBehaviour
	{
		[SerializeField]
		[Range(0f, 1f)]
		private float m_softness;

		[SerializeField]
		[Range(-1f, 1f)]
		private float m_dilate;

		private AbstractTextField m_textField;

		internal static readonly int OutlineSoftnessId = Shader.PropertyToID("_OutlineSoftness");

		internal static readonly int FaceDilateId = Shader.PropertyToID("_FaceDilate");

		private void OnEnable()
		{
			UpdateVisual();
		}

		protected void OnDidApplyAnimationProperties()
		{
			UpdateVisual();
		}

		private void UpdateVisual()
		{
			if (m_textField == null)
			{
				m_textField = this.GetComponent<AbstractTextField>();
			}
			if (!(m_textField == null))
			{
				Material material = m_textField.material;
				if (material != null)
				{
					material.SetFloat(OutlineSoftnessId, m_softness);
					material.SetFloat(FaceDilateId, m_dilate);
				}
			}
		}

		public TextFieldMaterialAnim()
			: this()
		{
		}
	}
}
