using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Ankama.Cube.Demo.UI
{
	[RequireComponent(typeof(Image))]
	public class ImagePositionToShader : MonoBehaviour
	{
		[SerializeField]
		private Transform m_transform;

		private int m_positionID;

		private Material m_material;

		protected void Awake()
		{
			m_positionID = Shader.PropertyToID("_Position");
			Image component = this.GetComponent<Image>();
			m_material = Object.Instantiate<Material>(component.get_material());
			component.set_material(m_material);
		}

		public void Update()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			m_material.SetVector(m_positionID, Vector4.op_Implicit(m_transform.get_localPosition()));
		}

		public void SetColor(Color c1, Color c2)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			m_material.SetColor("_Color1", c1);
			m_material.SetColor("_Color2", c2);
		}

		public void TweenColor(Color c1, Color c2, float duration)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			ShortcutExtensions.DOColor(m_material, c1, "_Color1", duration);
			ShortcutExtensions.DOColor(m_material, c2, "_Color2", duration);
		}

		private void OnDestroy()
		{
			Object.Destroy(m_material);
		}

		public ImagePositionToShader()
			: this()
		{
		}
	}
}
