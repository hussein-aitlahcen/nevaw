using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace Ankama.Cube.Maps.Objects
{
	public class FloorMechanismObjectContext : CharacterObjectContext
	{
		private readonly FloorMechanismObject m_characterObject;

		public override Transform transform => m_characterObject.get_transform();

		public override CharacterObject characterObject => m_characterObject;

		public FloorMechanismObjectContext(FloorMechanismObject characterObject)
		{
			m_characterObject = characterObject;
		}

		protected override void InitializeEventInstance(EventInstance eventInstance)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			if (!(null == m_characterObject))
			{
				ATTRIBUTES_3D val = FMODUtility.To3DAttributes(m_characterObject.get_transform());
				eventInstance.set3DAttributes(val);
			}
		}

		public override void GetVisualEffectTransformation(out Quaternion rotation, out Vector3 scale)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			rotation = m_characterObject.get_transform().get_rotation();
			scale = Vector3.get_one();
		}
	}
}
