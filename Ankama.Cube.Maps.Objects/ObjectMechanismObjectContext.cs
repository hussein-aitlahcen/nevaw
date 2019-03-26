using Ankama.Cube.Extensions;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace Ankama.Cube.Maps.Objects
{
	public sealed class ObjectMechanismObjectContext : CharacterObjectContext
	{
		private readonly ObjectMechanismObject m_characterObject;

		public override Transform transform => m_characterObject.get_transform();

		public override CharacterObject characterObject => m_characterObject;

		public ObjectMechanismObjectContext(ObjectMechanismObject characterObject)
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
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			rotation = m_characterObject.mapRotation.GetInverseRotation();
			scale = (m_characterObject.alliedWithLocalPlayer ? new Vector3(1f, 1f, 1f) : new Vector3(-1f, 1f, 1f));
		}
	}
}
