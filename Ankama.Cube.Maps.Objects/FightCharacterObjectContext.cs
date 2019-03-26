using Ankama.Cube.Data;
using Ankama.Cube.Extensions;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace Ankama.Cube.Maps.Objects
{
	public sealed class FightCharacterObjectContext : CharacterObjectContext
	{
		public const string Life = "life";

		private readonly FightCharacterObject m_characterObject;

		public override Transform transform => m_characterObject.get_transform();

		public override CharacterObject characterObject => m_characterObject;

		public FightCharacterObjectContext(FightCharacterObject characterObject)
		{
			m_characterObject = characterObject;
		}

		protected override void InitializeEventInstance(EventInstance eventInstance)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			if (!(null == m_characterObject))
			{
				ATTRIBUTES_3D val = FMODUtility.To3DAttributes(m_characterObject.get_transform());
				eventInstance.set3DAttributes(val);
				eventInstance.setParameterValue("life", (float)m_characterObject.life);
			}
		}

		public override void GetVisualEffectTransformation(out Quaternion rotation, out Vector3 scale)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			rotation = m_characterObject.direction.GetRotation();
			scale = Vector3.get_one();
		}

		public void UpdateDirection(Direction from, Direction to)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			Quaternion rotation = from.DirectionAngleTo(to).GetRotation();
			int count = m_visualEffectInstances.Count;
			for (int i = 0; i < count; i++)
			{
				Transform transform = m_visualEffectInstances[i].get_transform();
				transform.set_rotation(transform.get_rotation() * rotation);
			}
		}
	}
}
