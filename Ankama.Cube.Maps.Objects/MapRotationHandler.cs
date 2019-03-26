using Ankama.Cube.Data;
using Ankama.Cube.Extensions;
using UnityEngine;

namespace Ankama.Cube.Maps.Objects
{
	public class MapRotationHandler : MonoBehaviour
	{
		private void Start()
		{
			CameraHandler.AddMapRotationListener(OnMapRotationChanged);
		}

		private void OnDestroy()
		{
			CameraHandler.RemoveMapRotationListener(OnMapRotationChanged);
		}

		private void OnMapRotationChanged(DirectionAngle previousRotation, DirectionAngle newRotation)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			Transform transform = this.get_transform();
			transform.set_rotation(transform.get_rotation() * (previousRotation.GetRotation() * newRotation.GetInverseRotation()));
		}

		public MapRotationHandler()
			: this()
		{
		}
	}
}
