using JetBrains.Annotations;
using UnityEngine;

namespace Ankama.Cube.Animations
{
	[ExecuteInEditMode]
	public class GameObjectRotation : MonoBehaviour
	{
		[SerializeField]
		[UsedImplicitly]
		private Vector3 m_axis;

		[SerializeField]
		[UsedImplicitly]
		private Space m_space;

		private void Update()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			this.get_transform().Rotate(m_axis * Time.get_deltaTime(), m_space);
		}

		public GameObjectRotation()
			: this()
		{
		}
	}
}
